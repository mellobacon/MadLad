using System;
using System.Collections.Generic;
using MadLad.Compiler.CodeAnalysis.Binding;
using MadLad.Compiler.CodeAnalysis.Binding.Expressions;
using MadLad.Compiler.CodeAnalysis.Binding.Statements;
using MadLad.Compiler.CodeAnalysis.Syntax;
using MadLad.Compiler.CodeAnalysis.Syntax.Symbols;

namespace MadLad.Compiler.CodeAnalysis.Evaluator
{
    // Evaluates the expressions
    internal sealed class Evaluator
    {
        private readonly BoundStatement Root;
        private readonly Dictionary<VariableSymbol, object> Variables;
        object LastValue;
        public Evaluator(BoundStatement root, Dictionary<VariableSymbol, object> variables)
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

        // get each statement and evaluate
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
                    // get the value and assign the variable to it
                    var value = EvaluateExpression(v.Expression);
                    Variables[v.Variable] = value;
                    
                    LastValue = value;
                    
                    break;
                case IfBoundStatement f:
                    try
                    {
                        var _ = (bool)EvaluateExpression(f.Conditiion);
                    }
                    catch (Exception )
                    {
                        break;
                    }
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
                    // we make sure the condition can be evaluated as a bool. if not we just dont
                    // return the statement
                    try
                    {
                        var _ = (bool)EvaluateExpression(w.Condition);
                    }
                    catch (Exception )
                    {
                        break;
                    }
                    
                    while ((bool)EvaluateExpression(w.Condition))
                    {
                        EvaluateStatement(w.Statement);
                    }

                    break;
                case ForBoundStatement f:
                    EvaluateStatement(f.Initialization);
                    
                    // we make sure the condition can be evaluated as a bool. if not we just dont
                    // return the statement
                    try
                    {
                        var _ = (bool)EvaluateExpression(f.Condition);
                    }
                    catch (Exception)
                    {
                        break;
                    }
                    
                    while ((bool)EvaluateExpression(f.Condition))
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
                            BinaryBoundOperatorKind.Addition => (float) left + (float) right,
                            BinaryBoundOperatorKind.Subtraction => (float) left - (float) right,
                            BinaryBoundOperatorKind.Multiplication => (float) left * (float) right,
                            BinaryBoundOperatorKind.Division => (float)left / (float)right,
                            BinaryBoundOperatorKind.Modulo => (float)left % (float)right,
                            BinaryBoundOperatorKind.Pow => Math.Pow((float)left, (float)right),
                            BinaryBoundOperatorKind.Equals => Equals(left, right),
                            BinaryBoundOperatorKind.NotEquals => !Equals(left, right),
                            BinaryBoundOperatorKind.LessThan => (float) left < (float) right,
                            BinaryBoundOperatorKind.GreaterThan => (float) left > (float) right,
                            BinaryBoundOperatorKind.LessOrEqual => (float) left <= (float) right,
                            BinaryBoundOperatorKind.GreaterOrEqual => (float)left >= (float)right,
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
                            Convert.ToSingle(left) / Convert.ToSingle(right) // except this one. this stays floaty
                        ,
                        BinaryBoundOperatorKind.Modulo => (int)left % (int)right,
                        BinaryBoundOperatorKind.Pow => Math.Pow((int)left, (int)right),
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

                    // if the expression is a compound operator (x += 10)
                    if (a.Iscompoundoperator)
                    {
                        return a.Compoundoperator.Kind switch
                        {
                            SyntaxKind.PlusEqualsToken => Variables[a.Variable] = (int) Variables[a.Variable] + (int)value,
                            SyntaxKind.MinusEqualsToken => Variables[a.Variable] = (int) Variables[a.Variable] - (int)value,
                            SyntaxKind.SlashEqualsToken => Variables[a.Variable] = (int) Variables[a.Variable] / (int)value,
                            SyntaxKind.StarEqualsToken => Variables[a.Variable] = (int) Variables[a.Variable] * (int)value,
                            SyntaxKind.ModuloEqualsToken => Variables[a.Variable] = (int) Variables[a.Variable] % (int)value,
                        };
                    }
                    
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

                case ErrorBoundExpression e:
                {
                    return e;
                }
                
                default:
                    throw new Exception($"Unexpected node {root.Kind}");
            }
        }
    }
}
