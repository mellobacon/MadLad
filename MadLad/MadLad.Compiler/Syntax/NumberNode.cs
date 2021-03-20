using System.Collections.Generic;
using MadLad.MadLad.Compiler.Syntax.Expressions;

namespace MadLad.MadLad.Compiler.Syntax
{
    // Represents a number
    public class NumberNode : ExpressionSyntax
    {
        SyntaxToken Token { get; }
        public override SyntaxKind Kind => SyntaxKind.LiteralExpression;
        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Token;
        }

        public NumberNode(SyntaxToken token)
        {
            Token = token;
        }
    }
}