﻿namespace MadLad.Compiler.CodeAnalysis.Syntax
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
        LessThanToken,
        GreaterThanToken,
        LessEqualsToken,
        GreatEqualsToken,
        OpenBracketToken,
        ClosedBracketToken,
        SemicolonToken,
        VariableToken,
        
        // Keywords
        TrueKeyword,
        FalseKeyword,
        VarKeyword,
        IfKeyword,
        ElseKeyword,
        WhileKeyword,
        ForKeyword,
        
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
        VariableDeclaration,
        IfStatement,
        ElseStatement,
        WhileStatement,
        ForStatement,

        // Empty
        BadToken,
        EOFToken,
    }
}