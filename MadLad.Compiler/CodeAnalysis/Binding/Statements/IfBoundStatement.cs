using MadLad.Compiler.CodeAnalysis.Binding.Expressions;

namespace MadLad.Compiler.CodeAnalysis.Binding.Statements
{
    internal sealed class IfBoundStatement : BoundStatement
    {
        public readonly BoundExpression Conditiion;
        public readonly BoundStatement Statement;
        public readonly BoundStatement Elsestatement;

        public IfBoundStatement(BoundExpression condition, BoundStatement statement, BoundStatement elsestatement)
        {
            Conditiion = condition;
            Statement = statement;
            Elsestatement = elsestatement;
        }
        public override BoundKind Kind => BoundKind.IfStatement;
    }
}