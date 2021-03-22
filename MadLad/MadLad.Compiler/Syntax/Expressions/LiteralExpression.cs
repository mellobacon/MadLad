using System.Collections.Generic;

namespace MadLad.MadLad.Compiler.Syntax.Expressions
{
    // Represents a number
    public class LiteralExpression : ExpressionSyntax
    {
        public readonly object Value;
        public readonly SyntaxToken Token;
        
        public LiteralExpression(SyntaxToken token) : this(token, token.Value) {}

        public LiteralExpression(SyntaxToken token, object value)
        {
            Value = value;
            Token = token;
        }
        
        public override SyntaxKind Kind => SyntaxKind.LiteralExpression;
        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Token;
        }
    }
}