using System;
using MadLad.Compiler.CodeAnalysis.Syntax;

namespace MadLad.Compiler.CodeAnalysis.Binding
{
    public class UnaryBoundOperator
    {
        private readonly SyntaxKind SyntaxKind;
        public readonly UnaryBoundOperatorKind Kind;
        private readonly Type OperandType;
        public readonly Type ResultType;

        private UnaryBoundOperator(SyntaxKind syntaxKind, UnaryBoundOperatorKind kind, Type operandType) : this(syntaxKind, kind, operandType, operandType) { }

        private UnaryBoundOperator(SyntaxKind syntaxkind, UnaryBoundOperatorKind kind, Type operandtype, Type resulttype)
        {
            SyntaxKind = syntaxkind;
            Kind = kind;
            OperandType = operandtype;
            ResultType = resulttype;
        }

        private static readonly UnaryBoundOperator[] operations =
        {
            new(SyntaxKind.MinusToken, UnaryBoundOperatorKind.Negation, typeof(int)),
            new(SyntaxKind.BangToken, UnaryBoundOperatorKind.LogicalNegation, typeof(bool)),
        };

        public static UnaryBoundOperator Bind(SyntaxKind kind, Type operandtype)
        {
            foreach (var op in operations)
            {
                if (op.SyntaxKind == kind && op.OperandType == operandtype)
                {
                    return op;
                }
            }
            return null;
        }
    }
}