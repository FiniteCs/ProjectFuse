﻿namespace Fuse.CodeAnalysis.Syntax
{
    public enum SyntaxKind
    {
        // Tokens
        BadToken,
        EndOfFileToken,
        WhitespaceToken,
        NumberToken,
        PlusToken,
        MinusToken,
        StarToken,
        SlashToken,
        BangToken,
        EqualsToken,
        AmpersandAmpersandToken,
        PipePipeToken,
        BangEqualsToken,
        LessToken,
        LessOrEqualsToken,
        GreaterToken,
        GreaterOrEqualsToken,
        EqualsEqualsToken,
        OpenParenthesisToken,
        CloseParenthesisToken,
        OpenBraceToken,
        CloseBraceToken,
        IdentifierToken,

        // Keywords
        ElseKeyword,
        FalseKeyword,
        IfKeyword,
        LetKeyword,
        TrueKeyword,
        VarKeyword,

        // Nodes
        CompilationUnit,
        ElseClause,

        // Statements
        BlockStatement,
        VariableDeclaration,
        ExpressionStatement,
        IfStatement,

        // Expressions
        LiteralExpression,
        NameExpression,
        UnaryExpression,
        BinaryExpression,
        ParenthesizedExpression,
        AssignmentExpression,
    }
}
