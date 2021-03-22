using System;
using MadLad.MadLad.Compiler.Syntax;

namespace MadLad.MadLad.Compiler.Binding
{
    public class UnaryBoundOperator
    {
        private readonly SyntaxKind SyntaxKind;
        private readonly UnaryBoundOperatorKind Kind;
        private static Type OperandType;
        public readonly Type ResultType;

        UnaryBoundOperator(SyntaxKind syntaxKind, UnaryBoundOperatorKind kind, Type operandType) : this(syntaxKind, kind, operandType, operandType) { }
        
        public UnaryBoundOperator(SyntaxKind syntaxkind, UnaryBoundOperatorKind kind, Type operandtype, Type resulttype)
        {
            SyntaxKind = syntaxkind;
            Kind = kind;
            OperandType = operandtype;
            ResultType = resulttype;
        }

        private static readonly UnaryBoundOperator[] operations =
        {
            new(SyntaxKind.MinusToken, UnaryBoundOperatorKind.Negation, typeof(bool)),
            new(SyntaxKind.BangToken, UnaryBoundOperatorKind.LogicalNegation, typeof(int)),
        };

        public static UnaryBoundOperator Bind(SyntaxKind kind, Type operandtype)
        {
            foreach (var op in operations)
            {
                if (op.SyntaxKind == kind && operandtype == OperandType)
                {
                    return op;
                }
            }
            return null;
        }
    }
}