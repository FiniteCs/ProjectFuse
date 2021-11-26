﻿namespace Fuse.CodeAnalysis.Syntax
{
    internal class Lexer
    {
        private readonly string _text;
        private int _position;
        private readonly DiagnosticBag _diagnostics = new();
        public Lexer(string text)
        {
            _text = text;
        }

        public DiagnosticBag Diagnostics => _diagnostics;

        private char Peek(int offset)
        {
            int index = _position + offset;
            if (index >= _text.Length)
                return '\0';

            return _text[index];
        }

        private char Current => Peek(0);

        private char Lookahead => Peek(1);

        private void Next()
        {
            _position++;
        }

        public SyntaxToken Lex()
        {
            if (_position >= _text.Length)
                return new SyntaxToken(SyntaxKind.EndOfFileToken, _position, "\0", null);

            int start = _position;

            if (char.IsDigit(Current))
            {
                while (char.IsDigit(Current))
                    Next();

                int length = _position - start;
                string text = _text.Substring(start, length);
                if (!int.TryParse(text, out int value))
                    _diagnostics.ReportInvalidNumber(new TextSpan(start, length), text, typeof(int));

                return new SyntaxToken(SyntaxKind.NumberToken, start, text, value);
            }

            if (char.IsWhiteSpace(Current))
            {
                while (char.IsWhiteSpace(Current))
                    Next();

                int length = _position - start;
                string text = _text.Substring(start, length);
                return new SyntaxToken(SyntaxKind.WhitespaceToken, start, text, null);
            }

            if (char.IsLetter(Current))
            {
                while (char.IsLetter(Current))
                    Next();

                int length = _position - start;
                string text = _text.Substring(start, length);
                SyntaxKind kind = SyntaxFacts.GetKeywordKind(text);
                return new SyntaxToken(kind, start, text, null);
            }

            switch (Current)
            {
                case '+':
                    return new SyntaxToken(SyntaxKind.PlusToken, _position++, "+", null);
                case '-':
                    return new SyntaxToken(SyntaxKind.MinusToken, _position++, "-", null);
                case '*':
                    return new SyntaxToken(SyntaxKind.StarToken, _position++, "*", null);
                case '/':
                    return new SyntaxToken(SyntaxKind.SlashToken, _position++, "/", null);
                case '(':
                    return new SyntaxToken(SyntaxKind.OpenParenthesisToken, _position++, "(", null);
                case ')':
                    return new SyntaxToken(SyntaxKind.CloseParenthesisToken, _position++, ")", null);
                case '&':
                    if (Lookahead == '&')
                    {
                        _position += 2;
                        return new SyntaxToken(SyntaxKind.AmpersandAmpersandToken, start, "&&", null);
                    }
                    break;
                case '|':
                    if (Lookahead == '|')
                    {
                        _position += 2;
                        return new SyntaxToken(SyntaxKind.PipePipeToken, start, "||", null);
                    }
                    break;
                case '=':
                    if (Lookahead == '=')
                    {
                        _position += 2;
                        return new SyntaxToken(SyntaxKind.EqualsEqualsToken, start, "==", null);
                    }
                    else
                    {
                        _position += 1;
                        return new SyntaxToken(SyntaxKind.EqualsToken, start, "=", null);
                    }
                case '!':
                    if (Lookahead == '=')
                    {
                        _position += 2;
                        return new SyntaxToken(SyntaxKind.BangEqualsToken, start, "!=", null);
                    }
                    else
                    {
                        _position += 1;
                        return new SyntaxToken(SyntaxKind.BangToken, start, "!", null);
                    }
            }

            _diagnostics.ReportBadCharacter(_position, Current);
            return new SyntaxToken(SyntaxKind.BadToken, _position++, _text.Substring(_position - 1, 1), null);
        }
    }
}
