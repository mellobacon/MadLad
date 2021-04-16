using System;

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
        public override Type Type => Op.ResultType;
    }
}