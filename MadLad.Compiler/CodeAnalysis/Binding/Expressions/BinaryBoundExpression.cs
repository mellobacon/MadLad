using System;

namespace MadLad.Compiler.CodeAnalysis.Binding.Expressions
{
    internal sealed class BinaryBoundExpression : BoundExpression
    {
        public readonly BoundExpression Left;
        public readonly BinaryBoundOperator Op;
        public readonly BoundExpression Right;

        public BinaryBoundExpression(BoundExpression left, BinaryBoundOperator op, BoundExpression right)
        {
            Left = left;
            Op = op;
            Right = right;
        }

        public override BoundKind Kind => BoundKind.BinaryExpression;
        public override Type Type => Op.ResultType;
    }
}