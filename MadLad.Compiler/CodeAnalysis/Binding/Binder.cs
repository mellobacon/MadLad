using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using MadLad.Compiler.CodeAnalysis.Binding.Expressions;
using MadLad.Compiler.CodeAnalysis.Binding.Statements;
using MadLad.Compiler.CodeAnalysis.ErrorReporting;
using MadLad.Compiler.CodeAnalysis.Syntax;
using MadLad.Compiler.CodeAnalysis.Syntax.Expressions;
using MadLad.Compiler.CodeAnalysis.Syntax.Statements;

namespace MadLad.Compiler.CodeAnalysis.Binding
{
    internal sealed class Binder
    {
        private BoundScope Scope;
        private readonly ErrorList ErrorList = new();
        private ErrorList Errors => ErrorList;

        private Binder(BoundScope parent)
        {
            Scope = new BoundScope(parent);
        }
        
        // get stuff from each scope and bind them
        public static BoundGlobalScope BindGlobalScope(BoundGlobalScope previous, CompilationUnit syntax)
        {
            // get and bind the variables in the scope
            var parentscope = CreateParentScopes(previous); 
            var binder = new Binder(parentscope);
            var variables = binder.Scope.GetDeclaredVariables();
            
            // bind the statements / expressions
            var statement = binder.BindStatement(syntax.Expression);

            var errors = binder.Errors.ToImmutableArray();
            return new BoundGlobalScope(previous, errors, variables, statement);
        }

        private BoundStatement BindStatement(StatementSyntax statement)
        {
            return statement.Kind switch
            {
                SyntaxKind.BlockStatement => BindBlockStatement((BlockStatement)statement),
                SyntaxKind.ExpressionStatement => BindExpressionStatement((ExpressionStatement)statement),
                SyntaxKind.VariableDeclaration => BindVariableDeclaration((VariableDeclaration)statement),
                SyntaxKind.WhileStatement => BindWhileStatement((WhileStatement)statement),
                SyntaxKind.IfStatement => BindIfStatement((IfStatement)statement),
                SyntaxKind.ForStatement => BindForStatement((ForStatement)statement),
                _ => throw new Exception($"Unexpected syntax {statement.Kind}")
            };
        }

        private BoundStatement BindForStatement(ForStatement syntax)
        {
            var init = BindStatement(syntax.Initializer);
            var condition = BindExpression(syntax.Condition);
            var iterator = BindExpression(syntax.Iterator);
            var statement = BindStatement(syntax.Dostatement);
            return new ForBoundStatement(init, condition, iterator, statement);
        }

        private BoundStatement BindWhileStatement(WhileStatement syntax)
        {
            var condition = BindExpression(syntax.Condition, typeof(bool));
            var statement = BindStatement(syntax.Dostatement);
            return new WhileBoundStatement(condition, statement);
        }

        private BoundStatement BindIfStatement(IfStatement syntax)
        {
            var condition = BindExpression(syntax.Condition, typeof(bool));
            var statement = BindStatement(syntax.Thenstatement);
            var elsestatement = syntax.Elsestatement == null ? null : BindStatement(syntax.Elsestatement
                .Elsestatement);
            return new IfBoundStatement(condition, statement, elsestatement);
        }

        private BoundStatement BindBlockStatement(BlockStatement syntax)
        {
            var statements = ImmutableArray.CreateBuilder<BoundStatement>();
            Scope = new BoundScope(Scope);
            foreach (var _statement in syntax.Statements)
            {
                var statement = BindStatement(_statement);
                statements.Add(statement);
            }
            Scope = Scope.Parent;
            
            return new BlockBoundStatement(statements.ToImmutable());
        }
        
        private BoundStatement BindExpressionStatement(ExpressionStatement syntax)
        {
            var expression = BindExpression(syntax.Expression);
            return new ExpressionBoundStatement(expression);
        }

        private BoundStatement BindVariableDeclaration(VariableDeclaration syntax)
        {
            var name = syntax.Variable.Text;
            var expression = BindExpression(syntax.Expression);
            var variable = new Variable(name, expression.Type);

            if (!Scope.TryDeclare(variable))
            {
                ErrorList.ReportVariableAlreadyExists(syntax.Variable.Span, name);
            }

            return new BoundVariableDeclaration(variable, expression);
        }

        private BoundExpression BindExpression(ExpressionSyntax syntax, Type target)
        {
            var result = BindExpression(syntax);
            if (result.Type != target)
            {
                // return conversion error
            }

            return result;
        }

        private BoundExpression BindExpression(ExpressionSyntax syntax)
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
            // get variable name
            var name = syntax.VariableToken.Text;
            // get the expression the variable is equal to
            var expression = BindExpression(syntax.Expression);

            // check to make sure the variable doesnt exist
            if (!Scope.TryLookup(name, out var variable))
            {
                ErrorList.ReportUndefinedName(syntax.VariableToken.Span, name);
                return expression;
            }

            // check to make sure the variable cant be reassigned as another type
            if (expression.Type != variable.Type)
            {
                ErrorList.ReportCannotConvertType(expression.Type, variable.Type); // temp fix
                return expression;
            }

            return new AssignmentBoundExpression(variable, expression, syntax.Compoundoperator, syntax.Iscompound);
        }

        private BoundExpression BindNameExpression(NameExpression syntax)
        {
            var name = syntax.IdentifierToken.Text;
            if (string.IsNullOrEmpty(name))
            {
                return new LiteralBoundExpression(0);
            }
            if (!Scope.TryLookup(name, out var variable))
            {
                ErrorList.ReportUndefinedName(syntax.IdentifierToken.Span, name);
                return new LiteralBoundExpression(0);
            }
            
            return new VariableBoundExpression(variable);
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
    }
}