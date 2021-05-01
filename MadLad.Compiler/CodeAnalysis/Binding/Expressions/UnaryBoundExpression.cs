using System;
using MadLad.Compiler.CodeAnalysis.Syntax.Symbols;

namespace MadLad.Compiler.CodeAnalysis.Binding.Expressions
{
    internal sealed class UnaryBoundExpression : BoundExpression
    {
        public readonly UnaryBoundOperator Op;
        public readonly BoundExpression Operand;

        public UnaryBoundExpression(UnaryBoundOperator op, BoundExpression operand)
        {
            Op = op;
            Operand = operand;
        }

        public override BoundKind Kind => BoundKind.UnaryExpression;
        public override TypeSymbol Type => Op.ResultType;
    }
}