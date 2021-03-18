namespace MadLad.MadLad.Compiler.Syntax
{
    /// <summary>
    /// Represents what kind of syntax token the token can be
    /// </summary>
    public enum SyntaxKind
    {
        // Tokens
        NumberToken,
        
        WhitespaceToken,
        PlusToken,
        MinusToken,
        StarToken,
        SlashToken,
        OpenParenToken,
        CloseParenToken,
        
        // Expressions
        LiteralExpression,
        BinaryExpression,
        GroupedExpression,
        UnaryExpression,

        // Empty
        BadToken,
        EOFToken,
    }
}