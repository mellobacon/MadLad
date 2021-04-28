using System.Collections.Generic;
using System.Collections.Immutable;
using MadLad.Compiler.CodeAnalysis.ErrorReporting;
using MadLad.Compiler.CodeAnalysis.Syntax.Expressions;
using MadLad.Compiler.CodeAnalysis.Syntax.Statements;
using MadLad.Compiler.CodeAnalysis.Syntax.Text;

namespace MadLad.Compiler.CodeAnalysis.Syntax.Parser
{
    internal sealed class Parser
    {
        private readonly SourceText Text;
        private readonly ImmutableArray<SyntaxToken> Tokens;
        private readonly ErrorList ErrorList = new();
        public ErrorList Errors => ErrorList;

        private int Position;
        private SyntaxToken Current => Peek(0); // Gets the current character

        // Lex the text for the parser
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
                    tokens.Add(token); // Add each token in the input to a list of tokens
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
            return Current.Kind switch
            {
                SyntaxKind.OpenBracketToken => ParseBlockStatement(),
                SyntaxKind.VarKeyword => ParseVariableDeclaration(),
                SyntaxKind.IfKeyword => ParseIfStatement(),
                SyntaxKind.WhileKeyword => ParseWhileStatement(),
                SyntaxKind.ForKeyword => ParseForStatement(),
                _ => ParseExpressionStatement()
            };
        }

        private IfStatement ParseIfStatement()
        {
            var iftoken = MatchToken(SyntaxKind.IfKeyword);
            var openparen = MatchToken(SyntaxKind.OpenParenToken);
            var condition = ParseBinaryExpression();
            var closedparen = MatchToken(SyntaxKind.CloseParenToken);
            var statement = ParseStatement();
            var elsestatement = ParseElseStatement();
            return new IfStatement(iftoken, openparen, condition, closedparen, statement, elsestatement);
        }

        private ElseStatement ParseElseStatement()
        {
            if (Current.Kind != SyntaxKind.ElseKeyword)
            {
                return null;
            }

            var elsekeyword = NextToken();
            var statement = ParseStatement();
            return new ElseStatement(elsekeyword, statement);
        }

        private ForStatement ParseForStatement()
        {
            var forkeyword = MatchToken(SyntaxKind.ForKeyword);
            var openparen = MatchToken(SyntaxKind.OpenParenToken);
            var init = ParseStatement();
            var condition = ParseAssignmentExpression();
            var semicolon = MatchToken(SyntaxKind.SemicolonToken);
            var iterator = ParseAssignmentExpression();
            var closeparen = MatchToken(SyntaxKind.CloseParenToken);
            var statement = ParseStatement();
            return new ForStatement(forkeyword, openparen, init, condition, semicolon, iterator, closeparen, statement);
        }

        private WhileStatement ParseWhileStatement()
        {
            var whilekeyword = MatchToken(SyntaxKind.WhileKeyword);
            var openparen = MatchToken(SyntaxKind.OpenParenToken);
            var condition = ParseBinaryExpression();
            var closedparen = MatchToken(SyntaxKind.CloseParenToken);
            var dostatement = ParseStatement();
            return new WhileStatement(whilekeyword, openparen, condition, closedparen, dostatement);
        }

        private BlockStatement ParseBlockStatement()
        {
            var statements = ImmutableArray.CreateBuilder<StatementSyntax>();
            var openbracket = MatchToken(SyntaxKind.OpenBracketToken);

            while (Current.Kind != SyntaxKind.EOFToken && Current.Kind != SyntaxKind.ClosedBracketToken)
            {
                var starttoken = Current;
                
                var statement = ParseStatement();
                statements.Add(statement);

                if (Current == starttoken)
                {
                    NextToken();
                }
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

        private StatementSyntax ParseVariableDeclaration()
        {
            var keyword = MatchToken(SyntaxKind.VarKeyword);
            var variable = MatchToken(SyntaxKind.VariableToken);
            var equals = MatchToken(SyntaxKind.EqualsToken);
            var expression = ParseAssignmentExpression();
            var semicolon = MatchToken(SyntaxKind.SemicolonToken);
            return new VariableDeclaration(keyword, variable, equals, expression, semicolon);
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

            if (Current.Kind == SyntaxKind.VariableToken && SyntaxPrecedences.GetCompoundOperator(Peek(1)))
            {
                // parse as compound operator assignment
                var identifierToken = NextToken();
                var compoundOperatorToken = NextToken();
                var right = ParseBinaryExpression();
                return new AssignmentExpression(identifierToken, SyntaxPrecedences.GetSoloOperator(Peek(1)), right, compoundOperatorToken, true);
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
            // check if token is a keyword or number or variable or start of a grouped expression or string
            // and return it
            switch (Current.Kind)
            {
                // grouped expression
                case SyntaxKind.OpenParenToken:
                    var left = NextToken();
                    var expression = ParseAssignmentExpression();
                    var right = MatchToken(SyntaxKind.CloseParenToken);
                    return new GroupedExpression(left, expression, right);
                // bools
                case SyntaxKind.TrueKeyword:
                case SyntaxKind.FalseKeyword:
                    var keywordToken = NextToken();
                    var value = keywordToken.Kind == SyntaxKind.TrueKeyword;
                    return new LiteralExpression(keywordToken, value);
                // variables
                case SyntaxKind.VariableToken:
                    var variable = NextToken();
                    return new NameExpression(variable);
                // strings
                case SyntaxKind.StringToken:
                    var stringtoken = MatchToken(SyntaxKind.StringToken);
                    return new LiteralExpression(stringtoken);
                // default to reading a number
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
