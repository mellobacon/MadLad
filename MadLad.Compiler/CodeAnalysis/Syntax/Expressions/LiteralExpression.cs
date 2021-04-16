using System.Collections.Generic;

namespace MadLad.Compiler.CodeAnalysis.Syntax.Expressions
{
    // Represents a number
    public sealed class LiteralExpression : ExpressionSyntax
    {
        public readonly object Value;
        private readonly SyntaxToken Token;
        
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