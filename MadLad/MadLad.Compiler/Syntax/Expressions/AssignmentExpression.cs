using System.Collections.Generic;

namespace MadLad.MadLad.Compiler.Syntax.Expressions
{
    public class AssignmentExpression : ExpressionSyntax
    {
        public readonly SyntaxToken VariableToken;
        private readonly SyntaxToken EqualsToken;
        public readonly ExpressionSyntax Expression;

        public AssignmentExpression(SyntaxToken variabletoken, SyntaxToken equalstoken, ExpressionSyntax expression)
        {
            VariableToken = variabletoken;
            EqualsToken = equalstoken;
            Expression = expression;
        }

        public override SyntaxKind Kind => SyntaxKind.AssignmentExpression;
        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return VariableToken;
            yield return EqualsToken;
            yield return Expression;
        }
    }
}