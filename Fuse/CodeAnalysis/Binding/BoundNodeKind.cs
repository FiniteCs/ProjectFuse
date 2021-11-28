namespace Fuse.CodeAnalysis.Binding
{
    internal enum BoundNodeKind
    {
        // Statements
        BlockStatement,
        ExpressionStatement,

        // Expressions
        LiteralExpression,
        VariableExpression,
        AssignmnetExpression,
        UnaryExpression,
        BinaryExpression,
    }
}
