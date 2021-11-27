﻿using Fuse.CodeAnalysis.Syntax;
using Xunit;

namespace Fuse.Tests.CodeAnalysis.Syntax
{
    public class ParserTest
    {
        [Theory]
        [MemberData(nameof(GetBinaryOperatorPairsData))]
        public void Parser_BinaryExpression_HonorsPrecedences(SyntaxKind op1, SyntaxKind op2)
        {
            var op1Precedence = SyntaxFacts.GetBinaryOperatorPrecedence(op1);
            var op2Precedence = SyntaxFacts.GetBinaryOperatorPrecedence(op2);
            var op1Text = SyntaxFacts.GetText(op1);
            var op2Text = SyntaxFacts.GetText(op2);
            var text = $"a {op1Text} b {op2Text} c";
            var expression = SyntaxTree.Parse(text).Root;

            if (op1Precedence >= op2Precedence)
            {
                using var e = new AssertingEnumerator(expression);
                e.AssetNode(SyntaxKind.BinaryExpression);

                e.AssetNode(SyntaxKind.BinaryExpression);
                e.AssetNode(SyntaxKind.NameExpression);
                e.AssetToken(SyntaxKind.IdentifierToken, "a");
                e.AssetToken(op1, op1Text);
                e.AssetNode(SyntaxKind.NameExpression);
                e.AssetToken(SyntaxKind.IdentifierToken, "b");
                e.AssetToken(op2, op2Text);
                e.AssetNode(SyntaxKind.NameExpression);
                e.AssetToken(SyntaxKind.IdentifierToken, "c");
            }
            else
            {
                using var e = new AssertingEnumerator(expression);
                e.AssetNode(SyntaxKind.BinaryExpression);
                e.AssetNode(SyntaxKind.NameExpression);
                e.AssetToken(SyntaxKind.IdentifierToken, "a");
                e.AssetToken(op1, op1Text);
                e.AssetNode(SyntaxKind.BinaryExpression);
                e.AssetNode(SyntaxKind.NameExpression);
                e.AssetToken(SyntaxKind.IdentifierToken, "b");
                e.AssetToken(op2, op2Text);
                e.AssetNode(SyntaxKind.NameExpression);
                e.AssetToken(SyntaxKind.IdentifierToken, "c");
            }
        }

        [Theory]
        [MemberData(nameof(GetUnaryOperatorPairsData))]
        public void Parser_UnaryExpression_HonorsPrecedences(SyntaxKind unary, SyntaxKind binary)
        {
            var unaryPrecedence = SyntaxFacts.GetUnaryOperatorPrecedence(unary);
            var binaryPrecedence = SyntaxFacts.GetBinaryOperatorPrecedence(binary);
            var unaryText = SyntaxFacts.GetText(unary);
            var binaryText = SyntaxFacts.GetText(binary);
            var text = $"{unaryText} a {binaryText} b";
            var expression = SyntaxTree.Parse(text).Root;

            if (unaryPrecedence >= binaryPrecedence)
            {
                using var e = new AssertingEnumerator(expression);
                e.AssetNode(SyntaxKind.BinaryExpression);
                e.AssetNode(SyntaxKind.UnaryExpression);
                e.AssetToken(unary, unaryText);
                e.AssetNode(SyntaxKind.NameExpression);
                e.AssetToken(SyntaxKind.IdentifierToken, "a");
                e.AssetToken(binary, binaryText);
                e.AssetNode(SyntaxKind.NameExpression);
                e.AssetToken(SyntaxKind.IdentifierToken, "b");
            }
            else
            {
                using var e = new AssertingEnumerator(expression);
                e.AssetNode(SyntaxKind.UnaryExpression);
                e.AssetToken(unary, unaryText);
                e.AssetNode(SyntaxKind.BinaryExpression);
                e.AssetNode(SyntaxKind.NameExpression);
                e.AssetToken(SyntaxKind.IdentifierToken, "a");
                e.AssetToken(binary, binaryText);
                e.AssetNode(SyntaxKind.NameExpression);
                e.AssetToken(SyntaxKind.IdentifierToken, "b");
            }
        }

        public static IEnumerable<object[]> GetBinaryOperatorPairsData()
        {
            foreach (var op1 in SyntaxFacts.GetBinaryOperatorsKinds())
            {
                foreach (var op2 in SyntaxFacts.GetBinaryOperatorsKinds())
                {
                    yield return new object[] { op1, op2 };
                }
            }
        }

        public static IEnumerable<object[]> GetUnaryOperatorPairsData()
        {
            foreach (var unary in SyntaxFacts.GetUnaryOperatorsKinds())
            {
                foreach (var binary in SyntaxFacts.GetBinaryOperatorsKinds())
                {
                    yield return new object[] { unary, binary };
                }
            }
        }
    }
}