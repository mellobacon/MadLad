using MadLad.Compiler.CodeAnalysis.ErrorReporting;
using MadLad.Compiler.CodeAnalysis.Syntax.Text;

namespace MadLad.Compiler.CodeAnalysis.Syntax.Lexer
{
    public class Lexer
    {
        private readonly SourceText Text;
        private int Start;
        private int Position;
        private SyntaxKind Kind;
        private object Value;
        private char Current => Peek(0); // Sets the current character to whatever
        private char NextToken => Peek(1); // Sets the next token after the current one to whatever

        private readonly ErrorList ErrorList = new();
        public ErrorList Errors => ErrorList;

        public Lexer(SourceText text)
        {
            Text = text;
        }

        /// <summary>
        /// Turns each character into a token and returns it
        /// </summary>
        /// <returns></returns>
        public SyntaxToken Lex()
        {
            Start = Position; // keep track of where thing starts
            Kind = SyntaxKind.BadToken;
            Value = null;
            
            // Sets syntax token parameters depending on what char it is
            switch (Current)
            {
                case '\0':
                    Kind = SyntaxKind.EOFToken;
                    break;
                case '=':
                    if (NextToken == '=')
                    {
                        Kind = SyntaxKind.EqualsEqualsToken;
                        Advance(2);
                        break;
                    }
                    else
                    {
                        Kind = SyntaxKind.EqualsToken;
                        Advance(1);
                        break;
                    }
                case '!':
                    if (NextToken == '=')
                    {
                        Kind = SyntaxKind.NotEqualsToken;
                        Advance(2);
                        break;
                    }
                    else
                    {
                        Kind = SyntaxKind.BangToken;
                        Advance(1);
                        break;
                    }
                case '+':
                    Kind = SyntaxKind.PlusToken;
                    Advance(1);
                    break;
                case '-':
                    Kind = SyntaxKind.MinusToken;
                    Advance(1);
                    break;
                case '*':
                    Kind = SyntaxKind.StarToken;
                    Advance(1);
                    break;
                case '/':
                    Kind = SyntaxKind.SlashToken;
                    Advance(1);
                    break;
                case '&':
                    if (NextToken == '&')
                    {
                        Kind = SyntaxKind.AndAmpersandToken;
                        Advance(2);
                        break;
                    }
                    Advance(1);
                    break;
                case '|':
                    if (NextToken == '|')
                    {
                        Kind = SyntaxKind.OrPipeToken;
                        Advance(2);
                        break;
                    }
                    Advance(1);
                    break;
                case '(':
                    Kind = SyntaxKind.OpenParenToken;
                    Advance(1);
                    break;
                case ')':
                    Kind = SyntaxKind.CloseParenToken;
                    Advance(1);
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
                    if (char.IsLetter(Current) || Current == '_')
                    {
                        ReadLetterToken();
                    }
                    else
                    {
                        ErrorList.ReportBadCharacter(Position, Current);
                        Advance(1);
                    }
                    break;
            }

            var text = SyntaxPrecedences.GetText(Kind);
            var length = Position - Start;
            if (text == null)
            {
                text = Text.ToString(Start, length);
            }
            return new SyntaxToken(text, Kind, Value, Start);
        }
        
        #region Operations for lexing

        private void Advance(int i)
        {
            Position += i;
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
            // continues getting the number until there isnt another one to read
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
                Advance(1);
            }

            var length = Position - Start;
            var text = Text.ToString(Start, length);
            Kind = SyntaxKind.NumberToken;

            // if there is a decimal in the number its a float else its an int
            if (Text.Contains('.') && dotcount == 1)
            {
                if (!float.TryParse(text, out var value))
                {
                    ErrorList.ReportInvalidNumber(new TextSpan(Start, length), text, typeof(float));
                }
                Value = value;   
            }
            else
            {
                if (!int.TryParse(text, out var value))
                {
                    ErrorList.ReportInvalidNumber(new TextSpan(Start, length), text, typeof(int));
                }
                Value = value;   
            }
        }

        private void ReadWhiteSpaceToken()
        {
            while (char.IsWhiteSpace(Current))
            {
                Advance(1);
            }
            Kind = SyntaxKind.WhitespaceToken;
        }

        private void ReadLetterToken()
        {
            while (char.IsLetter(Current) || Current == '_')
            {
                Advance(1);
            }

            var length = Position - Start;
            var text = Text.ToString(Start, length);
            Kind = SyntaxPrecedences.GetKeywordKind(text);
            if (Kind == SyntaxKind.FalseKeyword)
            {
                Value = false;
            }

            if (Kind == SyntaxKind.TrueKeyword)
            {
                Value = true;
            }
        }
        #endregion
    }
}
