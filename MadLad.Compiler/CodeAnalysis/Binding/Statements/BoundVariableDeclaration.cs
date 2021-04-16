using MadLad.Compiler.CodeAnalysis.Binding.Expressions;
using MadLad.Compiler.CodeAnalysis.Syntax;

namespace MadLad.Compiler.CodeAnalysis.Binding.Statements
{
    internal sealed class BoundVariableDeclaration : BoundStatement
    {
        public readonly Variable Variable;
        public readonly BoundExpression Expression;

        public BoundVariableDeclaration(Variable variable, BoundExpression expression)
        {
            Variable = variable;
            Expression = expression;
        }

        public override BoundKind Kind => BoundKind.VariableDeclaration;
    }
}