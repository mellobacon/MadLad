using System;
using MadLad.MadLad.Compiler.Binding;
using MadLad.MadLad.Compiler.Binding.Expressions;

namespace MadLad.MadLad.Compiler.Evaluator
{
    // Evaluates the expressions
    public class Evaluator
    {
        private readonly BoundExpression Root;
        public Evaluator(BoundExpression root)
        {
            Root = root;
        }

        public object Evaluate()
        {
            return EvaluateExpression(Root);
        }

        private object EvaluateExpression(BoundExpression root)
        {
            // Evaluate node
            if (root is LiteralBoundExpression n)
            {
                return n.Value;
            }

            // Evaluate binary expression
            if (root is BinaryBoundExpression b)
            {
                var left = EvaluateExpression(b.Left);
                var right = EvaluateExpression(b.Right);
                
                // if one of the numbers is an float convert both numbers to floats
                if (left is float || right is float)
                {
                    return b.Op.BoundKind switch
                    {
                        BinaryBoundOperatorKind.Addition => Convert.ToSingle(left) + Convert.ToSingle(right),
                        BinaryBoundOperatorKind.Subtraction => Convert.ToSingle(left) - Convert.ToSingle(right),
                        BinaryBoundOperatorKind.Multiplication => Convert.ToSingle(left) * Convert.ToSingle(right),
                        BinaryBoundOperatorKind.Division => Convert.ToSingle(left) / Convert.ToSingle(right),
                        _ => throw new Exception($"Unexpected binary operator {b.Op}")
                    };
                }

                return b.Op.BoundKind switch
                {
                    BinaryBoundOperatorKind.Addition => (int) left + (int) right,
                    BinaryBoundOperatorKind.Subtraction => (int) left - (int) right,
                    BinaryBoundOperatorKind.Multiplication => (int) left * (int) right,
                    BinaryBoundOperatorKind.Division =>
                        Convert.ToSingle(left) / Convert.ToSingle(right) //except this one. this stays floaty
                    ,
                    _ => throw new Exception($"Unexpected binary operator {b.Op}")
                };
            }
            // Evaluate unary expression
            if (root is UnaryBoundExpression u)
            {
                var operand = EvaluateExpression(u.Operand);
                switch (u.Op.Kind)
                {
                    case UnaryBoundOperatorKind.Negation:
                        if (operand is float)
                        {
                            return -(float)operand;
                        }
                        return -(int)operand;
                    case UnaryBoundOperatorKind.LogicalNegation:
                        return !(bool)operand;
                }
                throw new Exception($"Unexpected unnary operator {u.Op}");
            }
            throw new Exception($"Unexpected node {root.Kind}");
        }
    }
}
