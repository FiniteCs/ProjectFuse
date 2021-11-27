﻿using Fuse.CodeAnalysis.Syntax;
using Xunit;

namespace Fuse.Tests.CodeAnalysis.Syntax
{
    internal sealed class AssertingEnumerator : IDisposable
    {
        private IEnumerator<SyntaxNode> _enumerator;
        private bool _hasErrors;

        public AssertingEnumerator(SyntaxNode node)
        {
            _enumerator = Flatten(node).GetEnumerator();
        }

        private bool MarkFaild()
        {
            _hasErrors = true;
            return false;
        }

        public void Dispose()
        {
            if (!_hasErrors)
                Assert.False(_enumerator.MoveNext());
            _enumerator.Dispose();
        }

        private IEnumerable<SyntaxNode> Flatten(SyntaxNode node)
        {
            var stack = new Stack<SyntaxNode>();
            stack.Push(node);
            while (stack.Count > 0)
            {
                var n = stack.Pop();
                yield return n;

                foreach (var child in n.GetChildren().Reverse())
                    stack.Push(child);
            }
        }

        public void AssetNode(SyntaxKind kind)
        {
            try
            {
                Assert.True(_enumerator.MoveNext());
                Assert.Equal(kind, _enumerator.Current.Kind);
                Assert.IsNotType<SyntaxToken>(_enumerator.Current);
            }
            catch when (MarkFaild())
            {
                throw;
            }
        }

        public void AssetToken(SyntaxKind kind, string text)
        {
            try
            {
                Assert.True(_enumerator.MoveNext());
                Assert.Equal(kind, _enumerator.Current.Kind);
                var token = Assert.IsType<SyntaxToken>(_enumerator.Current);
                Assert.Equal(text, token.Text);
            }
            catch when (MarkFaild())
            {
                throw;
            }
        }
    }
}