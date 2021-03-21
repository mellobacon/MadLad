namespace MadLad.MadLad.Compiler.Syntax
{
    // Gets the precedence of operators and keywords. Goes from most important to least important.
    public static class SyntaxPrecedences
    {
        public static int GetUnaryOperatorPrecedence(this SyntaxKind kind)
        {
            return kind switch
            {
                SyntaxKind.MinusToken => 6,
                _ => 0
            };
        }
        
        public static int GetBinaryOperatorPrecedence(this SyntaxKind kind)
        {
            // return binary operator precedence
            return kind switch
            {
                SyntaxKind.StarToken => 5,
                SyntaxKind.SlashToken => 4,
                SyntaxKind.PlusToken => 3,
                SyntaxKind.MinusToken => 2,
                SyntaxKind.EqualsEqualsToken => 1,
                _ => 0
            };
        }

        public static SyntaxKind GetKeywordKind(string text)
        {
            return text switch
            {
                "true" => SyntaxKind.TrueKeyword,
                "false" => SyntaxKind.FalseKeyword,
                _ => SyntaxKind.VariableToken
            };
        }
        
        public static string GetText(SyntaxKind kind)
        {
            return kind switch
            {
                SyntaxKind.PlusToken => "+",
                SyntaxKind.MinusToken => "-",
                SyntaxKind.StarToken => "*",
                SyntaxKind.SlashToken => "/",
                SyntaxKind.OpenParenToken => "(",
                SyntaxKind.CloseParenToken => ")",
                SyntaxKind.TrueKeyword => "true",
                SyntaxKind.FalseKeyword => "false",
                _ => null
            };
        }
    }
}
