﻿using Fuse.CodeAnalysis.Binding;
using Fuse.CodeAnalysis.Syntax;
using System.Threading;

namespace Fuse.CodeAnalysis
{
    public class Compilation
    {
        private BoundGlobalScope _globalScope;
        public Compilation(SyntaxTree syntaxTree)
            : this(null, syntaxTree)
        {
            SyntaxTree = syntaxTree;
        }

        private Compilation(Compilation previous, SyntaxTree syntaxTree)
        {
            Previous = previous;
            SyntaxTree = syntaxTree;
        }

        public SyntaxTree SyntaxTree { get; }
        public Compilation Previous { get; }

        internal BoundGlobalScope GlobalScope
        {
            get
            {
                if (_globalScope == null)
                {
                   var globalScope = Binder.BindGlobalScope(Previous?.GlobalScope, SyntaxTree.Root);
                   Interlocked.CompareExchange(ref _globalScope, globalScope, null);
                }

                return _globalScope;
            }
        }

        public Compilation ContinueWith(SyntaxTree syntaxTree)
        {
            return new Compilation(this, syntaxTree);
        }

        public EvaluationResult Evaluate(Dictionary<VariableSymbol, object> variables)
        {
            ImmutableArray<Diagnostic> diagnostics = SyntaxTree.Diagnostics.Concat(GlobalScope.Diagnostics).ToImmutableArray();
            if (diagnostics.Any())
                return new EvaluationResult(diagnostics, null);

            Evaluator evaluator = new(GlobalScope.Statement, variables);
            object value = evaluator.Evaluate();
            return new EvaluationResult(ImmutableArray<Diagnostic>.Empty, value);
        }
    }
}
