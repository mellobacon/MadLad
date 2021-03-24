namespace MadLad.Compiler.CodeAnalysis.Syntax
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
        EqualsToken,
        EqualsEqualsToken,
        BangToken,
        NotEqualsToken,

        VariableToken,
        TrueKeyword,
        FalseKeyword,
        
        // Expressions
        LiteralExpression,
        BinaryExpression,
        GroupedExpression,
        UnaryExpression,
        AssignmentExpression,
        NameExpression,

        // Empty
        BadToken,
        EOFToken,
    }
}