using System.Collections.Generic;

namespace MadLad.MadLad.Compiler.Syntax.Expressions
{
    public class UnaryExpression : ExpressionSyntax
    {
        public readonly SyntaxToken OpToken;
        public readonly ExpressionSyntax Operand;
        
        public UnaryExpression(SyntaxToken optoken, ExpressionSyntax operand)
        {
            OpToken = optoken;
            Operand = operand;
        }

        public override SyntaxKind Kind => SyntaxKind.UnaryExpression;
        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return OpToken;
            yield return Operand;
        }
    }
}