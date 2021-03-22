using System;

namespace MadLad.MadLad.Compiler.Binding.Expressions
{
    public class UnaryBoundExpression : BoundExpression
    {
        private readonly UnaryBoundOperator Op;
        private readonly BoundExpression Operand;

        public UnaryBoundExpression(UnaryBoundOperator op, BoundExpression operand)
        {
            Op = op;
            Operand = operand;
        }

        public override BoundKind Kind => BoundKind.UnaryExpression;
        public override Type Type => Op.ResultType;
    }
}