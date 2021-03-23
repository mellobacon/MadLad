using System;
using MadLad.MadLad.Compiler.Binding.Expressions;
using MadLad.MadLad.Compiler.ErrorReporting;
using MadLad.MadLad.Compiler.Syntax;
using MadLad.MadLad.Compiler.Syntax.Expressions;

namespace MadLad.MadLad.Compiler.Binding
{
    public class Binder
    {
        private readonly ErrorList ErrorList = new();
        public ErrorList Errors => ErrorList;
        
        public BoundExpression BindExpression(ExpressionSyntax syntax)
        {
            return syntax.Kind switch
            {
                SyntaxKind.LiteralExpression => BindLiteralExpression((LiteralExpression) syntax),
                SyntaxKind.BinaryExpression => BindBinaryExpression((BinaryExpression) syntax),
                SyntaxKind.UnaryExpression => BindUnaryExpression((UnaryExpression) syntax),
                _ => throw new Exception($"Unexpected syntax {syntax.Kind}")
            };
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
                ErrorList.ReportUndefinedBinaryOperator(syntax.Op.Span, syntax.Op.Text, boundleft.Type, boundright.Type);
                return boundleft;
            }
            return new BinaryBoundExpression(boundleft, boundoperator, boundright);
        }

        private BoundExpression BindUnaryExpression(UnaryExpression syntax)
        {
            var boundoperand = BindExpression(syntax.Operand);
            var boundoperator = UnaryBoundOperator.Bind(syntax.OpToken.Kind, boundoperand.Type);
            if (boundoperator == null)
            {
                ErrorList.ReportUndefinedUnaryOperator(syntax.OpToken.Span, syntax.OpToken.Text, boundoperand.Type);
                return boundoperand;
            }
            return new UnaryBoundExpression(boundoperator, boundoperand);
        }
    }
}