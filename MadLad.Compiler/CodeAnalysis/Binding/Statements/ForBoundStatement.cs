using MadLad.Compiler.CodeAnalysis.Binding.Expressions;

namespace MadLad.Compiler.CodeAnalysis.Binding.Statements
{
    internal sealed class ForBoundStatement : BoundStatement
    {
        public readonly BoundStatement Initialization;
        public readonly BoundExpression Condition;
        public readonly BoundExpression Iteration;
        public readonly BoundStatement Statement;

        /**
         * if (var x = 0; x < 1; x++) {}
         */
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