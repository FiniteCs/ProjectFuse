namespace Fuse.CodeAnalysis.Binding
{
    internal enum BoundNodeKind
    {
        // Statements
        BlockStatement,
        ExpressionStatement,
        VariableDeclaration,
        IfStatement,
        WhileStatement,
        ForStatement,

        // Expressions
        LiteralExpression,
        VariableExpression,
        AssignmnetExpression,
        UnaryExpression,
        BinaryExpression,
    }
}
