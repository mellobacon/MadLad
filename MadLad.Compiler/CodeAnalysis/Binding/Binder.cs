using System;
using System.Collections.Generic;
using System.Linq;
using MadLad.Compiler.CodeAnalysis.Binding.Expressions;
using MadLad.Compiler.CodeAnalysis.ErrorReporting;
using MadLad.Compiler.CodeAnalysis.Syntax;
using MadLad.Compiler.CodeAnalysis.Syntax.Expressions;

namespace MadLad.Compiler.CodeAnalysis.Binding
{
    public class Binder
    {
        private readonly Dictionary<Variable, object> Variables;
        private readonly ErrorList ErrorList = new();
        public ErrorList Errors => ErrorList;

        public Binder(Dictionary<Variable, object> variables)
        {
            Variables = variables;
        }
        
        public BoundExpression BindExpression(ExpressionSyntax syntax)
        {
            return syntax.Kind switch
            {
                SyntaxKind.LiteralExpression => BindLiteralExpression((LiteralExpression) syntax),
                SyntaxKind.BinaryExpression => BindBinaryExpression((BinaryExpression) syntax),
                SyntaxKind.UnaryExpression => BindUnaryExpression((UnaryExpression) syntax),
                SyntaxKind.GroupedExpression => BindGroupedExpression((GroupedExpression)syntax),
                SyntaxKind.AssignmentExpression => BindAssignmentExpression((AssignmentExpression)syntax),
                SyntaxKind.NameExpression => BindNameExpression((NameExpression)syntax),
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

        private BoundExpression BindGroupedExpression(GroupedExpression syntax)
        {
            return BindExpression(syntax.Expression);
        }

        private BoundExpression BindAssignmentExpression(AssignmentExpression syntax)
        {
            var name = syntax.VariableToken.Text;
            var expression = BindExpression(syntax.Expression);

            var variable = new Variable(name, expression.Type);
            
            var existingvariable = Variables.Keys.FirstOrDefault(v => v.Name == name);
            if (existingvariable != null)
            {
                Variables.Remove(existingvariable);
            }
            
            Variables[variable] = null;
            
            return new AssignmentBoundExpression(variable, expression);
        }

        private BoundExpression BindNameExpression(NameExpression syntax)
        {
            var name = syntax.IdentifierToken.Text;
            var variable = Variables.Keys.FirstOrDefault(v => v.Name == name);
            if (variable == null)
            {
                ErrorList.ReportUndefinedName(syntax.IdentifierToken.Span, name);
                return new LiteralBoundExpression(0);
            }
            
            return new VariableBoundExpression(variable);
        }
    }
}