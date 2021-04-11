using MadLad.Compiler.CodeAnalysis.Binding.Expressions;

namespace MadLad.Compiler.CodeAnalysis.Binding.Statements
{
    public class IfBoundStatement : BoundStatement
    {
        public readonly BoundExpression Conditiion;
        public readonly BoundStatement Statement;
        public readonly BoundStatement Elsestatement;

        public IfBoundStatement(BoundExpression conditiion, BoundStatement statement, BoundStatement elsestatement)
        {
            Conditiion = conditiion;
            Statement = statement;
            Elsestatement = elsestatement;
        }
        public override BoundKind Kind => BoundKind.IfStatement;
    }
}