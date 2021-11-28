using Fuse.CodeAnalysis.Binding;
using Fuse.CodeAnalysis.Syntax;

namespace Fuse.CodeAnalysis
{
    public class Compilation
    {
        public Compilation(SyntaxTree syntaxTree)
        {
            Syntax = syntaxTree;
        }

        public SyntaxTree Syntax { get; }

        public EvaluationResult Evaluate(Dictionary<VariableSymbol, object> variables)
        {
            Binder binder = new(variables);
            BoundExpression boundExpression = binder.BindExpression(Syntax.Root.Expression);

            ImmutableArray<Diagnostic> diagnostics = Syntax.Diagnostics.Concat(binder.Diagnostics).ToImmutableArray();
            if (diagnostics.Any())
                return new EvaluationResult(diagnostics, null);

            Evaluator evaluator = new(boundExpression, variables);
            object value = evaluator.Evaluate();
            return new EvaluationResult(ImmutableArray<Diagnostic>.Empty, value);
        }
    }
}
