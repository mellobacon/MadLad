using System;
using System.Collections.Generic;
using MadLad.Compiler.CodeAnalysis.Binding;
using MadLad.Compiler.CodeAnalysis.Binding.Expressions;
using MadLad.Compiler.CodeAnalysis.Binding.Statements;
using MadLad.Compiler.CodeAnalysis.Syntax;

namespace MadLad.Compiler.CodeAnalysis.Evaluator
{
    // Evaluates the expressions
    internal sealed class Evaluator
    {
        private readonly BoundStatement Root;
        private readonly Dictionary<Variable, object> Variables;
        object LastValue;
        public Evaluator(BoundStatement root, Dictionary<Variable, object> variables)
        {
            Root = root;
            Variables = variables;
        }

        // evaluate the statement and return the value
        public object Evaluate()
        {
            EvaluateStatement(Root);
            return LastValue;
        }

        // get each statement and evaluate ....this doesnt need a commment
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
                case WhileBoundStatement w:
                    var whilecondition = (bool)EvaluateExpression(w.Condition);
                    while (whilecondition)
                    {
                        EvaluateStatement(w.Statement);
                    }
                    
                    break;
                case ForBoundStatement f:
                    EvaluateStatement(f.Initialization);
                    var forcondition = (bool)EvaluateExpression(f.Condition);
                    while (forcondition)
                    {
                        EvaluateStatement(f.Statement);
                        EvaluateExpression(f.Iteration);
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
                // Evaluate single value
                case LiteralBoundExpression n:
                    return n.Value;
                
                // Evaluate binary expression
                case BinaryBoundExpression b:
                {
                    // get the left and right operands / expressions
                    var left = EvaluateExpression(b.Left);
                    var right = EvaluateExpression(b.Right);
                
                    // if one of the numbers is an float calculate both numbers as floats
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

                    // calculate as ints
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
                    // get the operand / expression
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
                
                // Evaluate the assignment expression (x = 10)
                case AssignmentBoundExpression a:
                {
                    // set the value
                    var value = EvaluateExpression(a.Expression);
                    
                    // set the variable equal to the value
                    Variables[a.Variable] = value;
                    return value;
                }
                
                // Evaluate the variable expression (x)
                case VariableBoundExpression v:
                {
                    // get the value of the variable
                    var value = Variables[v.Variable];
                    return value;
                }
                default:
                    throw new Exception($"Unexpected node {root.Kind}");
            }
        }
    }
}
