using System.Collections.Generic;

namespace MadLad.Compiler.CodeAnalysis.Syntax.Expressions
{
    // Represents a grouped expression ie stuff surrounded in parentheses
    public sealed class GroupedExpression : ExpressionSyntax
    {
        private readonly SyntaxToken Openparentoken;
        public readonly ExpressionSyntax Expression;
        private readonly SyntaxToken Closedparentoken;

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