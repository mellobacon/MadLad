using System.Collections.Generic;
using System.Collections.Immutable;
using MadLad.Compiler.CodeAnalysis.ErrorReporting;
using MadLad.Compiler.CodeAnalysis.Syntax.Expressions;
using MadLad.Compiler.CodeAnalysis.Syntax.Statements;
using MadLad.Compiler.CodeAnalysis.Syntax.Text;

namespace MadLad.Compiler.CodeAnalysis.Syntax.Parser
{
    public class Parser
    {
        private readonly SourceText Text;
        private readonly ImmutableArray<SyntaxToken> Tokens;
        private readonly ErrorList ErrorList = new();
        public ErrorList Errors => ErrorList;

        private int Position;
        private SyntaxToken Current => Peek(0);
        
        // Add each token in the input to a list of tokens
        public Parser(SourceText text)
        {
            Text = text;
            var lexer = new Lexer.Lexer(text);
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
            Tokens = tokens.ToImmutableArray();
            ErrorList.AddRange(lexer.Errors); // add any errors while lexing to the error list
        }

        // Analyze the tokens
        public CompilationUnit ParseCompilationUnit()
        {
            // parse the statement
            var statement = ParseStatement();
            var eoftoken = MatchToken(SyntaxKind.EOFToken);
            return new CompilationUnit(statement, eoftoken);
        }
        
        private StatementSyntax ParseStatement()
        {
            if (Current.Kind == SyntaxKind.OpenBracketToken)
            {
                return ParseBlockStatement();
            }
            return ParseExpressionStatement();
        }

        private BlockStatement ParseBlockStatement()
        {
            var statements = ImmutableArray.CreateBuilder<StatementSyntax>();
            var openbracket = MatchToken(SyntaxKind.OpenBracketToken);

            while (Current.Kind != SyntaxKind.EOFToken && Current.Kind != SyntaxKind.ClosedBracketToken)
            {
                var statement = ParseStatement();
                statements.Add(statement);
            }
            
            var closedbracket = MatchToken(SyntaxKind.ClosedBracketToken);
            
            return new BlockStatement(openbracket, statements.ToImmutable(), closedbracket);
        }
        
        private ExpressionStatement ParseExpressionStatement()
        {
            var expression = ParseAssignmentExpression();
            var semicolon = MatchToken(SyntaxKind.SemicolonToken);
            return new ExpressionStatement(expression, semicolon);
        }

        private ExpressionSyntax ParseAssignmentExpression()
        {
            // if something is being assigned to a variable parse that
            // else parse binary expression
            if (Current.Kind == SyntaxKind.VariableToken && Peek(1).Kind == SyntaxKind.EqualsToken)
            {
                var identifierToken = NextToken();
                var operatorToken = NextToken();
                var right = ParseAssignmentExpression();
                return new AssignmentExpression(identifierToken, operatorToken, right);
            }
            return ParseBinaryExpression();
        }

        private ExpressionSyntax ParseBinaryExpression(int parentprecedence = 0)
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

        private ExpressionSyntax ParsePrimaryExpression()
        {
            // check if token is a keyword or number or variable or start of a grouped expression
            // and return it
            switch (Current.Kind)
            {
                case SyntaxKind.OpenParenToken:
                    var left = NextToken();
                    var expression = ParseAssignmentExpression();
                    var right = MatchToken(SyntaxKind.CloseParenToken);
                    return new GroupedExpression(left, expression, right);
                case SyntaxKind.TrueKeyword:
                case SyntaxKind.FalseKeyword:
                    var keywordToken = NextToken();
                    var value = keywordToken.Kind == SyntaxKind.TrueKeyword;
                    return new LiteralExpression(keywordToken, value);
                case SyntaxKind.VariableToken:
                    var variable = NextToken();
                    return new NameExpression(variable);
                default:
                    var numbertoken = MatchToken(SyntaxKind.NumberToken);
                    return new LiteralExpression(numbertoken);
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
