﻿using Fuse.CodeAnalysis.Syntax;

namespace Fuse.CodeAnalysis.Binding
{
    internal sealed class BoundUnaryOperator
    {
        private BoundUnaryOperator(SyntaxKind synatxKind, BoundUnaryOperatorKind kind, Type operandType)
            : this(synatxKind, kind, operandType, operandType)
        {
        }

        private BoundUnaryOperator(SyntaxKind synatxKind, BoundUnaryOperatorKind kind, Type operandType, Type resultType)
        {
            SynatxKind = synatxKind;
            Kind = kind;
            OperandType = operandType;
            Type = resultType;
        }

        public SyntaxKind SynatxKind { get; }
        public BoundUnaryOperatorKind Kind { get; }
        public Type OperandType { get; }
        public Type Type { get; }

        private static readonly BoundUnaryOperator[] _operators =
        {
            new BoundUnaryOperator(SyntaxKind.BangToken, BoundUnaryOperatorKind.LogicalNegation, typeof(bool)),

            new BoundUnaryOperator(SyntaxKind.PlusToken, BoundUnaryOperatorKind.Identity, typeof(int)),
            new BoundUnaryOperator(SyntaxKind.MinusToken, BoundUnaryOperatorKind.Negation, typeof(int)),
            new BoundUnaryOperator(SyntaxKind.TildeToken, BoundUnaryOperatorKind.OnesComplement, typeof(int)),
        };

        public static BoundUnaryOperator Bind(SyntaxKind syntaxKind, Type operandType)
        {
            foreach (BoundUnaryOperator op in _operators)
            {
                if (op.SynatxKind == syntaxKind && op.OperandType == operandType)
                    return op;
            }

            return null;
        }
    }
}
