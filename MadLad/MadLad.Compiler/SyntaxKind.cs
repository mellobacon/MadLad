namespace MadLad.MadLad.Compiler
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
        
        // Empty
        BadToken,
        EOFToken,
    }
}