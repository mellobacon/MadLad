using System;
using System.Collections.Generic;
using MadLad.Compiler.CodeAnalysis.Binding;
using MadLad.Compiler.CodeAnalysis.Binding.Expressions;
using MadLad.Compiler.CodeAnalysis.Syntax;

namespace MadLad.Compiler.CodeAnalysis.Evaluator
{
    // Evaluates the expressions
    public class Evaluator
    {
        private readonly BoundExpression Root;
        private readonly Dictionary<Variable, object> Variables;
        public Evaluator(BoundExpression root, Dictionary<Variable, object> variables)
        {
            Root = root;
            Variables = variables;
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
                        BinaryBoundOperatorKind.Equals => Equals(left, right),
                        BinaryBoundOperatorKind.NotEquals => !Equals(left, right),
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
                    BinaryBoundOperatorKind.Equals => Equals(left, right),
                    BinaryBoundOperatorKind.NotEquals => !Equals(left, right),
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

            if (root is AssignmentBoundExpression a)
            {
                var value = EvaluateExpression(a.Expression);
                Variables[a.Variable] = value;
                return value;
            }

            if (root is VariableBoundExpression v)
            {
                var value = Variables[v.Variable];
                return value;
            }
            
            throw new Exception($"Unexpected node {root.Kind}");
        }
    }
}
