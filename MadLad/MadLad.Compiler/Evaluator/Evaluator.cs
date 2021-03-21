using System;
using MadLad.MadLad.Compiler.Syntax;
using MadLad.MadLad.Compiler.Syntax.Expressions;

namespace MadLad.MadLad.Compiler.Evaluator
{
    // Evaluates the expressions
    public class Evaluator
    {
        private readonly ExpressionSyntax Root;
        public Evaluator(ExpressionSyntax root)
        {
            Root = root;
        }

        public object Evaluate()
        {
            return EvaluateExpression(Root);
        }

        private object EvaluateExpression(ExpressionSyntax root)
        {
            // Evaluate number node
            if (root is NumberNode n)
            {
                if (n.Token.Text.Contains('.'))
                {
                    return (float)n.Token.Value;
                }
                return (int)n.Token.Value;
            }

            // Evaluate grouped expression
            if (root is GroupedExpression g)
            {
                var expression = EvaluateExpression(g.Expression);
                return expression;
            }
            
            // Evaluate binary expression
            if (root is BinaryExpression b)
            {
                var left = EvaluateExpression(b.Left);
                var right = EvaluateExpression(b.Right);
                var op = b.Op;
                // if one of the numbers is an float convert both numbers to floats
                if (left is float || right is float)
                {
                    switch (op.Kind)
                    {
                        case SyntaxKind.PlusToken:
                            return Convert.ToSingle(left) + Convert.ToSingle(right);
                        case SyntaxKind.MinusToken:
                            return Convert.ToSingle(left) - Convert.ToSingle(right);
                        case SyntaxKind.StarToken:
                            return Convert.ToSingle(left) * Convert.ToSingle(right);
                        case SyntaxKind.SlashToken:
                            return Convert.ToSingle(left) / Convert.ToSingle(right);
                        default:
                            throw new Exception($"Unexpected binary operator {b.Op}");
                    }
                }
                // if one of the numbers is an int convert both numbers to ints
                switch (op.Kind)
                {
                    case SyntaxKind.PlusToken:
                        return (int)left + (int)right;
                    case SyntaxKind.MinusToken:
                        return (int)left - (int)right;
                    case SyntaxKind.StarToken:
                        return (int)left * (int)right;
                    case SyntaxKind.SlashToken:
                        return Convert.ToSingle(left) / Convert.ToSingle(right); //except this one. this stays floaty
                    default:
                        throw new Exception($"Unexpected binary operator {b.Op}");
                }
            }
            // Evaluate unary expression
            if (root is UnaryExpression u)
            {
                var op = u.OpToken;
                var operand = EvaluateExpression(u.Operand);
                if (op.Kind == SyntaxKind.MinusToken)
                {
                    if (operand is float)
                    {
                        return -(float)operand;
                    }
                    return -(int)operand;
                }
                throw new Exception($"Unexpected unnary operator {op}");
            }
            throw new Exception($"Unexpected node {root.Kind}");
        }
    }
}
