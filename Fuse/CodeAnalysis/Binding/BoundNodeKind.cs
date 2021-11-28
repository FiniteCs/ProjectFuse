namespace Fuse.CodeAnalysis.Binding
{
    internal enum BoundNodeKind
    {
        // Statements
        BlockStatement,
        ExpressionStatement,
        VariableDeclaration,

        // Expressions
        LiteralExpression,
        VariableExpression,
        AssignmnetExpression,
        UnaryExpression,
        BinaryExpression,
    }
}
