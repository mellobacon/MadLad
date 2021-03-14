namespace MadLad.Lexer
{
    public class SyntaxToken
    {
        public readonly string Text;
        public readonly SyntaxKind Kind;
        public readonly object Value;

        /// <summary>
        /// Defines what a syntax token is ie what components make up a syntax token.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="kind"></param>
        /// <param name="value"></param>
        public SyntaxToken(string text, SyntaxKind kind, object value)
        {
            Text = text;
            Kind = kind;
            Value = value;
        }
    }
}