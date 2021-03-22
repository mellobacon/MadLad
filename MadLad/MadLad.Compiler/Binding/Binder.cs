using System;
using MadLad.MadLad.Compiler.Binding.Expressions;
using MadLad.MadLad.Compiler.Syntax;
using MadLad.MadLad.Compiler.Syntax.Expressions;

namespace MadLad.MadLad.Compiler.Binding
{
    public class Binder
    {
        public BoundExpression BindExpression(ExpressionSyntax syntax)
        {
            switch (syntax.Kind)
            {
                case SyntaxKind.LiteralExpression:
                    return BindLiteralExpression((LiteralExpression)syntax);
                case SyntaxKind.BinaryExpression:
                    return BindBinaryExpression((BinaryExpression)syntax);
                case SyntaxKind.UnaryExpression:
                    return BindUnaryExpression((UnaryExpression)syntax);
                default:
                    throw new Exception($"Unexpected syntax {syntax.Kind}");
            }
        }

        private static BoundExpression BindLiteralExpression(LiteralExpression syntax)
        {
            var value = syntax.Value ?? 0;
            return new LiteralBoundExpression(value);
        }

        private BoundExpression BindBinaryExpression(BinaryExpression syntax)
        {
            var boundleft = BindExpression(syntax.Left);
            var boundright = BindExpression(syntax.Right);
            var boundoperator = BinaryBoundOperator.Bind(boundleft.Type, syntax.Op.Kind, boundright.Type);
            if (boundoperator == null)
            {
                // return error
            }
            return new BinaryBoundExpression(boundleft, boundoperator, boundright);
        }

        private BoundExpression BindUnaryExpression(UnaryExpression syntax)
        {
            var boundoperand = BindExpression(syntax.Operand);
            var boundoperator = UnaryBoundOperator.Bind(syntax.Operand.Kind, boundoperand.Type);
            if (boundoperator == null)
            {
                // return error
            }
            return new UnaryBoundExpression(boundoperator, boundoperand);
        }
    }
}