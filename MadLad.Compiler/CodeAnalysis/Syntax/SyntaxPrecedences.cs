using System;
using System.Collections.Generic;

namespace MadLad.Compiler.CodeAnalysis.Syntax
{
    // Gets the precedence of operators and keywords. Goes from most important to least important.
    public static class SyntaxPrecedences
    {
        public static int GetUnaryOperatorPrecedence(this SyntaxKind kind)
        {
            return kind switch
            {
                SyntaxKind.BangToken => 6,
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
                SyntaxKind.SlashToken => 5,
                SyntaxKind.PlusToken => 4,
                SyntaxKind.MinusToken => 4,
                SyntaxKind.ModuloToken => 4,
                SyntaxKind.EqualsEqualsToken => 3,
                SyntaxKind.NotEqualsToken => 3,
                SyntaxKind.LessThanToken => 3,
                SyntaxKind.GreaterThanToken => 3,
                SyntaxKind.LessEqualsToken => 3,
                SyntaxKind.GreatEqualsToken => 3,
                SyntaxKind.AndAmpersandToken => 2,
                SyntaxKind.OrPipeToken => 1,
                _ => 0
            };
        }

        public static SyntaxKind GetKeywordKind(string text)
        {
            return text switch
            {
                "FINE" => SyntaxKind.TrueKeyword, // true
                "NO" => SyntaxKind.FalseKeyword, // false
                "WHATEVER" => SyntaxKind.VarKeyword, // var
                "BUTWHATFUCKINGIF" => SyntaxKind.IfKeyword, // if
                "WHATTHEFUCKELSE" => SyntaxKind.ElseKeyword, //else
                "DOTHETHING" => SyntaxKind.WhileKeyword, // while
                "GOAROUNDPLS" => SyntaxKind.ForKeyword, // for
                _ => SyntaxKind.VariableToken
            };
        }
        
        public static IEnumerable<SyntaxKind> GetUnaryOperatorKinds()
        {
            var kinds = (SyntaxKind[]) Enum.GetValues(typeof(SyntaxKind));
            foreach (var kind in kinds)
            {
                if (GetUnaryOperatorPrecedence(kind) > 0)
                {
                    yield return kind;
                }
            }
        }

        public static IEnumerable<SyntaxKind> GetBinaryOperatorKinds()
        {
            var kinds = (SyntaxKind[]) Enum.GetValues(typeof(SyntaxKind));
            foreach (var kind in kinds)
            {
                if (GetBinaryOperatorPrecedence(kind) > 0)
                {
                    yield return kind;
                }
            }
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
                SyntaxKind.OpenBracketToken => "{",
                SyntaxKind.ClosedBracketToken => "}",
                SyntaxKind.SemicolonToken => ";",
                SyntaxKind.BangToken => "!",
                SyntaxKind.EqualsToken => "=",
                SyntaxKind.LessThanToken => "<",
                SyntaxKind.GreaterThanToken => ">",
                SyntaxKind.NotEqualsToken => "!=",
                SyntaxKind.EqualsEqualsToken => "==",
                SyntaxKind.LessEqualsToken => "<=",
                SyntaxKind.GreatEqualsToken => ">=",
                SyntaxKind.OrPipeToken => "||",
                SyntaxKind.AndAmpersandToken => "&&",
                SyntaxKind.TrueKeyword => "FINE",
                SyntaxKind.FalseKeyword => "NO",
                SyntaxKind.VarKeyword => "WHATEVER",
                SyntaxKind.IfKeyword => "BUTWHATFUCKINGIF",
                SyntaxKind.ElseKeyword => "WHATTHEFUCKELSE",
                SyntaxKind.WhileKeyword => "DOTHETHING",
                SyntaxKind.ForKeyword => "GOAROUNDPLS",
                SyntaxKind.ModuloToken => "%",
                SyntaxKind.StarStarToken => "**",
                SyntaxKind.PlusEqualsToken => "+=",
                SyntaxKind.MinusEqualsToken => "-=",
                SyntaxKind.SlashEqualsToken => "/=",
                SyntaxKind.StarEqualsToken => "*=",
                SyntaxKind.ModuloEqualsToken => "%=",
                _ => null
            };
        }

        public static bool GetCompoundOperator(SyntaxToken token)
        {
            return token.Kind switch
            {
                SyntaxKind.PlusEqualsToken or
                SyntaxKind.MinusToken or
                SyntaxKind.SlashEqualsToken or
                SyntaxKind.StarEqualsToken or
                SyntaxKind.ModuloEqualsToken
                => true,
                _ => false
            };
        }

        public static SyntaxToken GetSoloOperator(SyntaxToken token)
        {
            return token.Kind switch
            {
                SyntaxKind.PlusEqualsToken => new SyntaxToken(token.Text, SyntaxKind.PlusToken, token.Value, token.Position),
                SyntaxKind.MinusEqualsToken => new SyntaxToken(token.Text, SyntaxKind.MinusToken, token.Value, token.Position),
                SyntaxKind.SlashEqualsToken => new SyntaxToken(token.Text, SyntaxKind.SlashToken, token.Value, token.Position),
                SyntaxKind.StarEqualsToken => new SyntaxToken(token.Text, SyntaxKind.StarToken, token.Value, token.Position),
                SyntaxKind.ModuloEqualsToken => new SyntaxToken(token.Text, SyntaxKind.ModuloToken, token.Value, token.Position),
                _ => token
            };
        }
    }
}
