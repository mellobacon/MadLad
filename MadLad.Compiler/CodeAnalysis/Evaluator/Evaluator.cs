using System;
using System.Collections.Generic;
using MadLad.Compiler.CodeAnalysis.Binding;
using MadLad.Compiler.CodeAnalysis.Binding.Expressions;
using MadLad.Compiler.CodeAnalysis.Binding.Statements;
using MadLad.Compiler.CodeAnalysis.Syntax;

namespace MadLad.Compiler.CodeAnalysis.Evaluator
{
    // Evaluates the expressions
    public class Evaluator
    {
        private readonly BoundStatement Root;
        private readonly Dictionary<Variable, object> Variables;
        object LastValue;
        public Evaluator(BoundStatement root, Dictionary<Variable, object> variables)
        {
            Root = root;
            Variables = variables;
        }

        public object Evaluate()
        {
            EvaluateStatement(Root);
            return LastValue;
        }

        private void EvaluateStatement(BoundStatement root)
        {
            switch (root)
            {
                case BlockBoundStatement b:
                    foreach (var statement in b.Statements)
                    {
                        EvaluateStatement(statement);
                    }

                    break;
                case ExpressionBoundStatement e:
                    LastValue = EvaluateExpression(e.Expression);
                    break;
                case BoundVariableDeclaration v:
                    var value = EvaluateExpression(v.Expression);
                    Variables[v.Variable] = value;
                    LastValue = value;
                    break;
                case IfBoundStatement f:
                    var condition = (bool)EvaluateExpression(f.Conditiion);
                    if (condition)
                    {
                        EvaluateStatement(f.Statement);
                    }
                    else if (f.Elsestatement != null)
                    {
                        EvaluateStatement(f.Elsestatement);
                    }
                    
                    break;
                default:
                    throw new Exception($"Unexpected node {root.Kind}");
            }
        }

        private object EvaluateExpression(BoundExpression root)
        {
            switch (root)
            {
                // Evaluate node
                case LiteralBoundExpression n:
                    return n.Value;
                // Evaluate binary expression
                case BinaryBoundExpression b:
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
                            BinaryBoundOperatorKind.LessThan => Convert.ToSingle(left) < Convert.ToSingle(right),
                            BinaryBoundOperatorKind.GreaterThan => Convert.ToSingle(left) > Convert.ToSingle(right),
                            BinaryBoundOperatorKind.LessOrEqual => Convert.ToSingle(left) <= Convert.ToSingle(right),
                            BinaryBoundOperatorKind.GreaterOrEqual => Convert.ToSingle(left) >= Convert.ToSingle(right),
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
                        BinaryBoundOperatorKind.LogicalAnd => (bool)left && (bool)right,
                        BinaryBoundOperatorKind.LogicalOr => (bool)left || (bool)right,
                        BinaryBoundOperatorKind.LessThan => (int) left < (int) right,
                        BinaryBoundOperatorKind.GreaterThan => (int) left > (int) right,
                        BinaryBoundOperatorKind.LessOrEqual => (int) left <= (int) right,
                        BinaryBoundOperatorKind.GreaterOrEqual => (int) left >= (int) right,
                        _ => throw new Exception($"Unexpected binary operator {b.Op}")
                    };
                }
                // Evaluate unary expression
                case UnaryBoundExpression u:
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
                case AssignmentBoundExpression a:
                {
                    var value = EvaluateExpression(a.Expression);
                    Variables[a.Variable] = value;
                    return value;
                }
                case VariableBoundExpression v:
                {
                    var value = Variables[v.Variable];
                    return value;
                }
                default:
                    throw new Exception($"Unexpected node {root.Kind}");
            }
        }
    }
}
