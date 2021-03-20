using System.Collections.Generic;
using MadLad.MadLad.Compiler.ErrorReporting;
using MadLad.MadLad.Compiler.Syntax.Expressions;

namespace MadLad.MadLad.Compiler.Syntax.Parser
{
    public class Parser
    {
        readonly SyntaxToken[] Tokens;
        readonly ErrorList ErrorList = new();
        ErrorList Errors => ErrorList;

        int Position;
        private SyntaxToken Current => Peek(0);
        
        // Add each token in the input to a list of tokens
        public Parser(string text)
        {
            var lexer = new Syntax.Lexer.Lexer(text);
            var tokens = new List<SyntaxToken>();
            SyntaxToken token;
            
            // iterate through the tokens
            do
            {
                // Lex the input
                token = lexer.Lex();
                // If the token is valid, add it to the token list
                if (token.Kind != SyntaxKind.WhitespaceToken && token.Kind != SyntaxKind.BadToken)
                {
                    tokens.Add(token);
                }
            } while (token.Kind != SyntaxKind.EOFToken);
            Tokens = tokens.ToArray();
            ErrorList.AddRange(lexer.Errors); // add any errors while lexing to the error list
        }
        
        // Analyze the tokens
        public SyntaxTree Parse()
        {
            // parse the expression
            var primaryexpression = ParseBinaryExpression();
            var eoftoken = MatchToken(SyntaxKind.EOFToken);
            
            // if something is being assigned to a variable parse that
            // else parse binary expression
            
            return new SyntaxTree(ErrorList, primaryexpression, eoftoken);
        }
        
        ExpressionSyntax ParseBinaryExpression(int parentprecedence = 0)
        {
            ExpressionSyntax left;
            // if there is a unary expression parse that
            var unaryprecedence = Current.Kind.GetUnaryOperatorPrecedence();
            if (unaryprecedence != 0 && unaryprecedence >= parentprecedence)
            {
                var op = NextToken();
                var operand = ParseBinaryExpression(unaryprecedence);
                left = new UnaryExpression(op, operand);
            }
            else
            {
               left = ParsePrimaryExpression();
            }
            
            // else actually parse the binary expression
            while (true)
            {
                var precedence = Current.Kind.GetBinaryOperatorPrecedence();
                
                if (precedence.Equals(0) || precedence <= parentprecedence)
                {
                    break;
                }

                var op = NextToken();
                var right = ParseBinaryExpression(precedence);
                left = new BinaryExpression(left, op, right);
            }

            return left;
        }

        ExpressionSyntax ParsePrimaryExpression()
        {
            // check if token is a number and
            // return that value...i think
            switch (Current.Kind)
            {
                case SyntaxKind.OpenParenToken:
                    var left = NextToken();
                    var expression = ParseBinaryExpression();
                    var right = MatchToken(SyntaxKind.CloseParenToken);
                    return new GroupedExpression(left, expression, right);
                default:
                    var numbertoken = MatchToken(SyntaxKind.NumberToken);
                    return new NumberNode(numbertoken);
            }
        }

        #region Things to help the parser parse
        private SyntaxToken Peek(int offset)
        {
            var index = Position + offset;
            return index >= Tokens.Length ? Tokens[^1] : Tokens[index];
        }
        
        // Get the current token and then move to the next position in the input
        private SyntaxToken NextToken()
        {
            var current = Current;
            Position++;
            return current;
        }

        // if the token matches to the one passed in return the token else token will be null
        private SyntaxToken MatchToken(SyntaxKind kind)
        {
            if (Current.Kind == kind)
            {
                return NextToken();   
            }
            Errors.ReportUnexpectedToken(Current.Span, Current.Kind, kind);
            return new SyntaxToken(null, kind, null, Current.Position);
        }
        #endregion
    }
}
