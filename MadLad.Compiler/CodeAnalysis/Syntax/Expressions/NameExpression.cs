using System.Collections.Generic;

namespace MadLad.Compiler.CodeAnalysis.Syntax.Expressions
{
    public class NameExpression : ExpressionSyntax
    {
        public readonly SyntaxToken IdentifierToken;
        public NameExpression(SyntaxToken identifierToken)
        {
            IdentifierToken = identifierToken;
        }

        public override SyntaxKind Kind => SyntaxKind.NameExpression;
        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return IdentifierToken;
        }
    }
}