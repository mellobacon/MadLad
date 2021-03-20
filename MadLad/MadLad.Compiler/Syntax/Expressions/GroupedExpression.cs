using System.Collections.Generic;

namespace MadLad.MadLad.Compiler.Syntax.Expressions
{
    // Represents a grouped expression ie stuff surrounded in parentheses
    public class GroupedExpression : ExpressionSyntax
    {
        public SyntaxToken Openparentoken;
        public ExpressionSyntax Expression;
        public SyntaxToken Closedparentoken;

        public GroupedExpression(SyntaxToken openparentoken, ExpressionSyntax expression, SyntaxToken closedparentoken)
        {
            Openparentoken = openparentoken;
            Expression = expression;
            Closedparentoken = closedparentoken;
        }

        public override SyntaxKind Kind => SyntaxKind.GroupedExpression;
        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Openparentoken;
            yield return Expression;
            yield return Closedparentoken;
        }
    }
}