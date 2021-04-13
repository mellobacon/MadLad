using MadLad.Compiler.CodeAnalysis.Binding.Expressions;

namespace MadLad.Compiler.CodeAnalysis.Binding.Statements
{
    public class ForBoundStatement : BoundStatement
    {
        public readonly BoundStatement Initialization;
        public readonly BoundExpression Condition;
        public readonly BoundExpression Iteration;
        public readonly BoundStatement Statement;

        public ForBoundStatement(BoundStatement initialization, BoundExpression condition, BoundExpression iteration, BoundStatement statement)
        {
            Initialization = initialization;
            Condition = condition;
            Iteration = iteration;
            Statement = statement;
        }

        public override BoundKind Kind => BoundKind.ForStatement;
    }
}