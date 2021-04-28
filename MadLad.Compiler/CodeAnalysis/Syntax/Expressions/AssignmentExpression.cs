using System.Collections.Generic;

namespace MadLad.Compiler.CodeAnalysis.Syntax.Expressions
{
    public sealed class AssignmentExpression : ExpressionSyntax
    {
        public readonly SyntaxToken Compoundoperator;
        public readonly bool Iscompound;
        public readonly SyntaxToken VariableToken;
        private readonly SyntaxToken EqualsToken;
        public readonly ExpressionSyntax Expression;

        public AssignmentExpression(SyntaxToken variableToken, SyntaxToken compoundtoken, ExpressionSyntax expression,
            SyntaxToken compoundoperator, bool iscompound)
            : this(variableToken, compoundtoken, expression)
        {
            Compoundoperator = compoundoperator;
            Iscompound = iscompound;
        }

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