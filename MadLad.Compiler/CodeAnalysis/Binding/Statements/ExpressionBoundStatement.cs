using MadLad.Compiler.CodeAnalysis.Binding.Expressions;

namespace MadLad.Compiler.CodeAnalysis.Binding.Statements
{
    internal sealed class ExpressionBoundStatement : BoundStatement
    {
        public readonly BoundExpression Expression;

        public ExpressionBoundStatement(BoundExpression expression)
        {
            Expression = expression;
        }

        public override BoundKind Kind => BoundKind.ExpressionStatement;
    }
}