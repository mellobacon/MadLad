namespace MadLad.MadLad.Compiler.Syntax
{
    public static class SyntaxPrecedences
    {

        public static int GetUnaryOperatorPrecedence(this SyntaxKind kind)
        {
            return kind switch
            {
                SyntaxKind.MinusToken => 5,
                _ => 0
            };
        }
        
        public static int GetBinaryOperatorPrecedence(this SyntaxKind kind)
        {
            // return binary operator precedence
            return kind switch
            {
                SyntaxKind.StarToken => 4,
                SyntaxKind.SlashToken => 3,
                SyntaxKind.PlusToken => 2,
                SyntaxKind.MinusToken => 1,
                _ => 0
            };
        }
    }
}
