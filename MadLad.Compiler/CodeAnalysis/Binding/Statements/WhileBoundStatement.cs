using MadLad.Compiler.CodeAnalysis.Binding.Expressions;

namespace MadLad.Compiler.CodeAnalysis.Binding.Statements
{
    public class WhileBoundStatement : BoundStatement
    {
        public readonly BoundExpression Condition;
        public readonly BoundStatement Statement;

        public WhileBoundStatement(BoundExpression condition, BoundStatement statement )
        {
            Condition = condition;
            Statement = statement;
        }
        public override BoundKind Kind => BoundKind.WhileStatement;
    }
}