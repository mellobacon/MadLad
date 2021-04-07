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
                SyntaxKind.EqualsEqualsToken => 3,
                SyntaxKind.NotEqualsToken => 3,
                SyntaxKind.AndAmpersandToken => 2,
                SyntaxKind.OrPipeToken => 1,
                _ => 0
            };
        }

        public static SyntaxKind GetKeywordKind(string text)
        {
            return text switch
            {
                "true" => SyntaxKind.TrueKeyword,
                "false" => SyntaxKind.FalseKeyword,
                "var" => SyntaxKind.VarKeyword,
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
                SyntaxKind.NotEqualsToken => "!=",
                SyntaxKind.EqualsEqualsToken => "==",
                SyntaxKind.OrPipeToken => "||",
                SyntaxKind.AndAmpersandToken => "&&",
                SyntaxKind.TrueKeyword => "true",
                SyntaxKind.FalseKeyword => "false",
                _ => null
            };
        }
    }
}
