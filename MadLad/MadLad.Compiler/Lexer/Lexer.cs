namespace MadLad.Lexer
{
    public class Lexer
    {
        readonly string Text;
        string _text;
        int Start;
        int Position;
        SyntaxKind Kind;
        object Value;
        char Current => Peek(0); // Sets the current character to whatever

        readonly ErrorList ErrorList = new();
        public ErrorList Errors => ErrorList;
        
        public Lexer(string text)
        {
            Text = text;
        }

        #region Operations for lexing

        private void Advance()
        {
            Position++;
        }
        
        // returns the char at whatever position depending on the offset
        private char Peek(int offset)
        {
            var index = Position + offset;
            return index >= Text.Length ? '\0' : Text[index];
        }

        private void ReadNumberToken()
        {
            var dotcount = 0;
            while (char.IsDigit(Current) || Current == '.')
            {
                if (Current == '.')
                {
                    if (dotcount == 1)
                    {
                        break;
                    }
                    dotcount++;
                }
                Advance();
            }

            var length = Position - Start;
            var text = Text.Substring(Start, length);
            _text = text;
            Kind = SyntaxKind.NumberToken;

            // if there is a decimal in the number its a float else its an int
            if (Text.Contains('.') && dotcount == 1)
            {
                float.TryParse(_text, out var value);
                Value = value;
            }
            else
            {
                int.TryParse(_text, out var value);
                Value = value;
            }
        }

        private void ReadWhiteSpaceToken()
        {
            while (char.IsWhiteSpace(Current))
            {
                Advance();
            }
            _text = " ";
            Kind = SyntaxKind.WhitespaceToken;
        }

        #endregion

        /// <summary>
        /// Returns each syntax token in the input
        /// </summary>
        /// <returns></returns>
        public SyntaxToken Lex()
        {
            Start = Position; // keep track of where thing starts
            Kind = SyntaxKind.BadToken;
            Value = null;
            
            // sets stuff depending on what char it is
            switch (Current)
            {
                case '\0':
                    Kind = SyntaxKind.EOFToken;
                    break;
                case '+':
                    Kind = SyntaxKind.PlusToken;
                    _text = "+";
                    Advance();
                    break;
                case '-':
                    Kind = SyntaxKind.MinusToken;
                    _text = "-";
                    Advance();
                    break;
                case '*':
                    Kind = SyntaxKind.StarToken;
                    _text = "*";
                    Advance();
                    break;
                case '/':
                    Kind = SyntaxKind.SlashToken;
                    _text = "/";
                    Advance();
                    break;
                case '(':
                    Kind = SyntaxKind.OpenParenToken;
                    _text = "(";
                    Advance();
                    break;
                case ')':
                    Kind = SyntaxKind.CloseParenToken;
                    _text = ")";
                    Advance();
                    break;
                case '0': case '1': case '2':
                case '3': case '4': case '5':
                case '6': case '7': case '8':
                case '9':   
                    ReadNumberToken();
                    break;
                case ' ':
                case '\n':
                case '\t':
                case '\r':
                    ReadWhiteSpaceToken();
                    break;
                default: 
                    if (char.IsWhiteSpace(Current))
                    {
                        ReadWhiteSpaceToken();
                    }
                    else
                    {
                        ErrorList.ReportBadCharacter(Position, Current);
                    }
                    break;
            }
            return new SyntaxToken(_text, Kind, Value);
        }
    }
}
