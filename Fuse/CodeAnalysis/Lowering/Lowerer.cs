using Fuse.CodeAnalysis.Binding;
using Fuse.CodeAnalysis.Symbols;
using Fuse.CodeAnalysis.Syntax;

namespace Fuse.CodeAnalysis.Lowering
{
    internal sealed class Lowerer : BoundTreeRewriter
    {
        private int _labelCount;
        private Lowerer()
        {

        }

        private BoundLabel GenerateLabel()
        {
            string name = $"Label{++_labelCount}";
            return new BoundLabel(name);
        }

        public static BoundBlockStatement Lower(BoundStatement statement)
        {
            Lowerer lowerer = new();
            BoundStatement result = lowerer.RewriteStatement(statement);
            return Flatten(result);
        }

        private static BoundBlockStatement Flatten(BoundStatement statement)
        {
            ImmutableArray<BoundStatement>.Builder builder = ImmutableArray.CreateBuilder<BoundStatement>();
            Stack<BoundStatement> stack = new();
            stack.Push(statement);

            while (stack.Count > 0)
            {
                BoundStatement current = stack.Pop();

                if (current is BoundBlockStatement block)
                {
                    foreach (BoundStatement s in block.Statements.Reverse())
                        stack.Push(s);
                }
                else
                {
                    builder.Add(current);
                }
            }

            return new BoundBlockStatement(builder.ToImmutable());
        }

        protected override BoundStatement RewriteIfStatement(BoundIfStatement node)
        {
            if (node.ElseStatement == null)
            {
                BoundLabel endLabel = GenerateLabel();
                BoundConditionalGotoStatement gotoFalse = new(endLabel, node.Condition, false);
                BoundLabelStatement endLabelStatement = new(endLabel);
                BoundBlockStatement result = new(ImmutableArray.Create(gotoFalse, node.ThenStatement, endLabelStatement));
                return RewriteStatement(result);
            }
            else
            {
                BoundLabel elseLabel = GenerateLabel();
                BoundLabel endLabel = GenerateLabel();

                BoundConditionalGotoStatement gotoFalse = new(elseLabel, node.Condition, false);
                BoundGotoStatement gotoEndStatement = new(endLabel);
                BoundLabelStatement elseLabelStatement = new(elseLabel);
                BoundLabelStatement endLabelStatement = new(endLabel);
                BoundBlockStatement result = new
                (
                    ImmutableArray.Create
                    (
                        gotoFalse, 
                        node.ThenStatement, 
                        gotoEndStatement,
                        elseLabelStatement,
                        node.ElseStatement,
                        endLabelStatement
                    )
                );

                return RewriteStatement(result);
            }
        }

        protected override BoundStatement RewriteWhileStatement(BoundWhileStatement node)
        {
            BoundLabel continueLabel = GenerateLabel();
            BoundLabel checkLabel = GenerateLabel();
            BoundLabel endLabel = GenerateLabel();

            BoundGotoStatement gotoCheck = new(checkLabel);
            BoundLabelStatement continueLabelStatement = new(continueLabel);
            BoundLabelStatement checkLabelStatement = new(checkLabel);
            BoundConditionalGotoStatement gotoTrue = new(continueLabel, node.Condition);
            BoundLabelStatement endLabelStatement = new(endLabel);
            BoundBlockStatement result = new
            (
                ImmutableArray.Create
                (
                    gotoCheck,
                    continueLabelStatement,
                    node.Body,
                    checkLabelStatement,
                    gotoTrue,
                    endLabelStatement
                )
            );

            return RewriteStatement(result);
        }

        protected override BoundStatement RewriteForStatement(BoundForStatement node)
        {
            BoundVariableDeclaration variableDeclaration = new(node.Variable, node.LowerBound);
            BoundVariableExpression variableExpression = new(node.Variable);
            var upperBoundSymbol = new VariableSymbol("upperBound", true, typeof(int));
            var upperBoundDeclaration = new BoundVariableDeclaration(upperBoundSymbol, node.UpperBound);
            BoundBinaryExpression condition = new
            (
                variableExpression,
                BoundBinaryOperator.Bind(SyntaxKind.LessOrEqualsToken, typeof(int), typeof(int)), 
                new BoundVariableExpression(upperBoundSymbol)
            );

            BoundExpressionStatement increment = new
            (
                new BoundAssignmentExpression
                (
                    node.Variable,
                    new BoundBinaryExpression
                    (
                        variableExpression,
                        BoundBinaryOperator.Bind(SyntaxKind.PlusToken, typeof(int), typeof(int)),
                        new BoundLiteralExpression(1)
                    )
                )
            );

            BoundBlockStatement whileBody = new(ImmutableArray.Create(node.Body, increment));
            BoundWhileStatement whileStatement = new(condition, whileBody);
            BoundBlockStatement result = new(ImmutableArray.Create<BoundStatement>(variableDeclaration,
                                                                                   upperBoundDeclaration,
                                                                                   whileStatement));

            return RewriteStatement(result);
        }
    }
}
