using Fuse.CodeAnalysis.Symbols;

namespace Fuse.CodeAnalysis.Binding
{
    internal abstract class BoundExpression : BoundNode
    {
        public abstract TypeSymbol Type { get; }
    }
}
