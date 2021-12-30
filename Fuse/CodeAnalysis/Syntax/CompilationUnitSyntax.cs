namespace Fuse.CodeAnalysis.Syntax
{
    public sealed class CompilationUnitSyntax : SyntaxNode
    {
        public CompilationUnitSyntax(ImmutableArray<MemberSyntax> members, SyntaxToken endOfFileToken)
        {
            Members = members;
            EndOfFileToken = endOfFileToken;
        }

        public override SyntaxKind Kind => SyntaxKind.CompilationUnit;
        public ImmutableArray<MemberSyntax> Members { get; }
        public SyntaxToken EndOfFileToken { get; }
    }

    public sealed class FunctionDeclarationSyntax : MemberSyntax
    {
        public FunctionDeclarationSyntax(SyntaxToken functionKeyword, SyntaxToken identifier, SyntaxToken openParenthesisToken, SeparatedSyntaxList<ParameterSyntax> parameters, SyntaxToken closeParenthesisToken, TypeClauseSyntax type, BlockStatementSyntax body)
        {
            FunctionKeyword = functionKeyword;
            Identifier = identifier;
            OpenParenthesisToken = openParenthesisToken;
            Parameters = parameters;
            CloseParenthesisToken = closeParenthesisToken;
            Type = type;
            Body = body;
        }

        public override SyntaxKind Kind => SyntaxKind.FunctionDeclaration;

        public SyntaxToken FunctionKeyword { get; }
        public SyntaxToken Identifier { get; }
        public SyntaxToken OpenParenthesisToken { get; }
        public SeparatedSyntaxList<ParameterSyntax> Parameters { get; }
        public SyntaxToken CloseParenthesisToken { get; }
        public TypeClauseSyntax Type { get; }
        public BlockStatementSyntax Body { get; }
    }

    public sealed class GlobalStatementSyntax : MemberSyntax
    {
        public GlobalStatementSyntax(StatementSyntax statement)
        {
            Statement = statement;
        }

        public override SyntaxKind Kind => SyntaxKind.GlobalStatement;
        public StatementSyntax Statement { get; }
    }

    public abstract class MemberSyntax : SyntaxNode
    {
    }

    public sealed class ParameterSyntax : SyntaxNode
    {
        public ParameterSyntax(SyntaxToken identifier, TypeClauseSyntax type)
        {
            Identifier = identifier;
            Type = type;
        }

        public override SyntaxKind Kind => SyntaxKind.Parameter;
        public SyntaxToken Identifier { get; }
        public TypeClauseSyntax Type { get; }
    }

    public sealed class TypeClauseSyntax : SyntaxNode
    {
        public TypeClauseSyntax(SyntaxToken colonToken, SyntaxToken identifier)
        {
            ColonToken = colonToken;
            Identifier = identifier;
        }

        public override SyntaxKind Kind => SyntaxKind.TypeClause;
        public SyntaxToken ColonToken { get; }
        public SyntaxToken Identifier { get; }
    }
}
