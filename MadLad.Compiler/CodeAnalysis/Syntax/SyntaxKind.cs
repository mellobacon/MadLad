namespace MadLad.Compiler.CodeAnalysis.Syntax
{
    /// <summary>
    /// Represents what kind of syntax token the token can be
    /// </summary>
    public enum SyntaxKind
    {
        // Tokens
        WhitespaceToken,
        
        NumberToken,
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
        OrPipeToken,
        AndAmpersandToken,
        OpenBracketToken,
        ClosedBracketToken,
        SemicolonToken,

        VariableToken,
        TrueKeyword,
        FalseKeyword,
        
        CompilationUnit,
        
        // Expressions
        LiteralExpression,
        BinaryExpression,
        GroupedExpression,
        UnaryExpression,
        AssignmentExpression,
        NameExpression,
        
        // Statements
        BlockStatement,
        ExpressionStatement,

        // Empty
        BadToken,
        EOFToken,
    }
}