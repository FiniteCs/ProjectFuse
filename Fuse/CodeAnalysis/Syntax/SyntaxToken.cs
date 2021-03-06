using Fuse.CodeAnalysis.Text;

namespace Fuse.CodeAnalysis.Syntax
{
    public sealed class SyntaxToken : SyntaxNode
    {
        public SyntaxToken(SyntaxKind kind, int position, string text, object value)
        {
            Kind = kind;
            Position = position;
            Text = text;
            Value = value;
        }

        public override SyntaxKind Kind { get; }
        public int Position { get; }
        public string Text { get; }
        public object Value { get; }
        public bool IsMissing => Text == null;

        public override TextSpan Span => new(Position, Text?.Length ?? 0);
    }
}
