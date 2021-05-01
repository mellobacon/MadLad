using System;
using MadLad.Compiler.CodeAnalysis.Syntax;
using MadLad.Compiler.CodeAnalysis.Syntax.Symbols;

namespace MadLad.Compiler.CodeAnalysis.Binding
{
    internal sealed class UnaryBoundOperator
    {
        private readonly SyntaxKind SyntaxKind;
        public readonly UnaryBoundOperatorKind Kind;
        private readonly TypeSymbol OperandType;
        public readonly TypeSymbol ResultType;

        private UnaryBoundOperator(SyntaxKind syntaxKind, UnaryBoundOperatorKind kind, TypeSymbol operandType) : this(syntaxKind, kind, operandType, operandType) { }

        private UnaryBoundOperator(SyntaxKind syntaxkind, UnaryBoundOperatorKind kind, TypeSymbol operandtype, TypeSymbol resulttype)
        {
            SyntaxKind = syntaxkind;
            Kind = kind;
            OperandType = operandtype;
            ResultType = resulttype;
        }

        private static readonly UnaryBoundOperator[] operations =
        {
            new(SyntaxKind.MinusToken, UnaryBoundOperatorKind.Negation, TypeSymbol.Int),
            new(SyntaxKind.BangToken, UnaryBoundOperatorKind.LogicalNegation, TypeSymbol.Bool),
        };

        public static UnaryBoundOperator Bind(SyntaxKind kind, TypeSymbol operandtype)
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