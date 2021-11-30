using Fuse.CodeAnalysis.Text;

namespace Fuse.CodeAnalysis.Syntax
{
    public sealed class SyntaxTree
    {
        public SyntaxTree(SourceText text)
        {
            Parser parser = new(text);
            CompilationUnitSyntax root = parser.ParseCompilationUnit();

            Text = text;
            Diagnostics = parser.Diagnostics.ToImmutableArray();
            Root = root;
        }

        public SourceText Text { get; }
        public ImmutableArray<Diagnostic> Diagnostics { get; }
        public CompilationUnitSyntax Root { get; }

        public static SyntaxTree Parse(string text)
        {
            SourceText sourceText = SourceText.From(text);
            return Parse(sourceText);
        }

        public static SyntaxTree Parse(SourceText text)
        {
            return new SyntaxTree(text);
        }

        public static IEnumerable<SyntaxToken> ParseTokens(string text)
        {
            SourceText sourceText = SourceText.From(text);
            return ParseTokens(sourceText);
        }

        public static IEnumerable<SyntaxToken> ParseTokens(SourceText text)
        {
            Lexer lexer = new(text);
            while (true)
            {
                SyntaxToken token = lexer.Lex();
                if (token.Kind == SyntaxKind.EndOfFileToken)
                    break;

                yield return token;
            }
        }
    }
}
