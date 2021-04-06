using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using MadLad.Compiler.CodeAnalysis.Binding.Expressions;
using MadLad.Compiler.CodeAnalysis.ErrorReporting;
using MadLad.Compiler.CodeAnalysis.Syntax;
using MadLad.Compiler.CodeAnalysis.Syntax.Expressions;

namespace MadLad.Compiler.CodeAnalysis.Binding
{
    public class Binder
    {
        private BoundScope Scope;
        private readonly ErrorList ErrorList = new();
        public ErrorList Errors => ErrorList;

        public Binder(BoundScope parent)
        {
            Scope = new BoundScope(parent);
        }

        public static BoundGlobalScope BindGlobalScope(BoundGlobalScope previous, CompilationUnit syntax)
        {
            var parentscope = CreateParentScopes(previous);
            var binder = new Binder(parentscope);
            var expression = binder.BindExpression(syntax.Expression);
            var variables = binder.Scope.GetDeclaredVariables();
            var errors = binder.Errors.ToImmutableArray();
            return new BoundGlobalScope(previous, errors, variables, expression);
        }

        private static BoundScope CreateParentScopes(BoundGlobalScope previous)
        {
            var stack = new Stack<BoundGlobalScope>();
            while (previous != null)
            {
                stack.Push(previous);
                previous = previous.Previous;
            }

            BoundScope current = null;
            while (stack.Count > 0)
            {
                previous = stack.Pop();
                var scope = new BoundScope(current);
                foreach (var v in previous.Variables)
                {
                    scope.TryDeclare(v);
                }

                current = scope;
            }

            return current;
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

            if (!Scope.TryDeclare(variable))
            {
                ErrorList.ReportVariableAlreadyExists(syntax.VariableToken.Span, name);
            }
            
            return new AssignmentBoundExpression(variable, expression);
        }

        private BoundExpression BindNameExpression(NameExpression syntax)
        {
            var name = syntax.IdentifierToken.Text;
            if (!Scope.TryLookup(name, out var variable))
            {
                ErrorList.ReportUndefinedName(syntax.IdentifierToken.Span, name);
                return new LiteralBoundExpression(0);
            }
            
            return new VariableBoundExpression(variable);
        }
    }
}