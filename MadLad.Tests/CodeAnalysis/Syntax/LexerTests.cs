using System.Collections.Generic;
using System.Linq;
using MadLad.Compiler.CodeAnalysis.Syntax;
using Xunit;

namespace MadLad.Tests.CodeAnalysis.Syntax
{
    public class LexerTests
    {
        [Theory]
        [MemberData(nameof(GetTokensData))]
        public void Lexer_Lexes_Tokens(SyntaxKind kind, string text)
        {
            var tokens = SyntaxTree.ParseTokens(text);

            var token = Assert.Single(tokens);
            
            if (token != null)
            {
                Assert.Equal(kind, token.Kind);
                Assert.Equal(text, token.Text);
            }
        }
        
        [Theory]
        [MemberData(nameof(GetTokensPairsData))]
        public void Lexer_Lexes_TokenPairs(SyntaxKind kind1, string text1,
                                            SyntaxKind kind2, string text2)
        {
            var text = text1 + text2;
            var tokens = SyntaxTree.ParseTokens(text).ToArray();
            
            Assert.Equal(2 , tokens.Length);
            Assert.Equal(tokens[0].Kind , kind1);
            Assert.Equal(tokens[0].Text , text1);
            Assert.Equal(tokens[1].Kind , kind2);
            Assert.Equal(tokens[1].Text , text2);
        }
        
        [Theory]
        [MemberData(nameof(GetTokensPairsWithSeparatorData))]
        public void Lexer_Lexes_TokenPairs_With_Separators(SyntaxKind kind1, string text1, SyntaxKind separatorkind, 
            string separatortext, SyntaxKind kind2, string text2)
        {
            var text = text1 + separatortext + text2;
            var tokens = SyntaxTree.ParseTokens(text).ToArray();
            
            Assert.Equal(3 , tokens.Length);
            Assert.Equal(tokens[0].Kind , kind1);
            Assert.Equal(tokens[0].Text , text1);
            Assert.Equal(tokens[1].Text, separatortext);
            Assert.Equal(tokens[1].Kind, separatorkind);
            Assert.Equal(tokens[2].Kind , kind2);
            Assert.Equal(tokens[2].Text , text2);
        }

        public static IEnumerable<object[]> GetTokensData()
        {
            foreach (var t in GetTokens().Concat(GetSeparators()))
            {
                yield return new object[] {t.kind, t.text};
            }
        }
        
        public static IEnumerable<object[]> GetTokensPairsData()
        {
            foreach (var t in GetTokenPairs())
            {
                yield return new object[] {t.kind1, t.text1, t.kind2, t.text2};
            }
        }
        
        public static IEnumerable<object[]> GetTokensPairsWithSeparatorData()
        {
            foreach (var t in GetTokenPairsWithSeparator())
            {
                yield return new object[] {t.kind1, t.text1, t.separatorkind, t.separatortext, t.kind2, t.text2};
            }
        }
        
        private static IEnumerable<(SyntaxKind kind, string text)> GetSeparators()
        {
            return new[]
            {
                (SyntaxKind.WhitespaceToken, " "),
                (SyntaxKind.WhitespaceToken, "  "),
                (SyntaxKind.WhitespaceToken, "\r"),
                (SyntaxKind.WhitespaceToken, "\n"),
                (SyntaxKind.WhitespaceToken, "\r\n")
            };
        }

        // get every valid token besides whitespace and eof. and bad
        private static IEnumerable<(SyntaxKind kind, string text)> GetTokens()
        {
            return new[]
            {
                (SyntaxKind.PlusToken, "+"),
                (SyntaxKind.MinusToken, "-"),
                (SyntaxKind.StarToken, "*"),
                (SyntaxKind.SlashToken, "/"),
                (SyntaxKind.ModuloToken, "%"),
                (SyntaxKind.StarStarToken, "**"),

                (SyntaxKind.OpenParenToken, "("),
                (SyntaxKind.CloseParenToken, ")"),

                (SyntaxKind.BangToken, "!"),
                (SyntaxKind.EqualsToken, "="),
                
                (SyntaxKind.AndAmpersandToken, "&&"),
                (SyntaxKind.OrPipeToken, "||"),
                (SyntaxKind.EqualsEqualsToken, "=="),
                (SyntaxKind.NotEqualsToken, "!="),
                
                (SyntaxKind.PlusEqualsToken, "+="),
                (SyntaxKind.MinusEqualsToken, "-="),
                (SyntaxKind.StarEqualsToken, "*="),
                (SyntaxKind.SlashEqualsToken, "/="),
                (SyntaxKind.ModuloEqualsToken, "%="),

                (SyntaxKind.FalseKeyword, "NO"),
                (SyntaxKind.TrueKeyword, "FINE"),
                
                (SyntaxKind.NumberToken, "1"),
                (SyntaxKind.NumberToken, "123"),
                (SyntaxKind.NumberToken, "5.32"),
                
                (SyntaxKind.VariableToken, "a"),
                (SyntaxKind.VariableToken, "abc"),
                (SyntaxKind.VariableToken, "a_b_3"),
                
                (SyntaxKind.StringToken, "\"something\""),
                (SyntaxKind.StringToken, "\"some thing\""),
            };
        }

        // checks if there is a token that needs a space inbetween (like ==false)
        private static bool RequiresSeparator(SyntaxKind kind1, SyntaxKind kind2)
        {
            var keyword1 = kind1.ToString().EndsWith("Keyword");
            var keyword2 = kind2.ToString().EndsWith("Keyword");
            
            if (kind1 == SyntaxKind.VariableToken && kind2 == SyntaxKind.VariableToken)
            {
                return true;
            }

            if (keyword1 && keyword2)
            {
                return true;
            }
            
            if (keyword1 && kind2 == SyntaxKind.VariableToken)
            {
                return true;
            }
            
            if (kind1 == SyntaxKind.VariableToken && keyword2)
            {
                return true;
            }
            
            if (kind1 == SyntaxKind.NumberToken && kind2 == SyntaxKind.NumberToken)
            {
                return true;
            }
            
            if (keyword1 && kind2 == SyntaxKind.NumberToken)
            {
                return true;
            }
            
            if (kind1 == SyntaxKind.BangToken && kind2 == SyntaxKind.EqualsToken)
            {
                return true;
            }
            
            if (kind1 == SyntaxKind.BangToken && kind2 == SyntaxKind.EqualsEqualsToken)
            {
                return true;
            }
            
            if (kind1 == SyntaxKind.EqualsToken && kind2 == SyntaxKind.EqualsToken)
            {
                return true;
            }
            
            if (kind1 == SyntaxKind.EqualsToken && kind2 == SyntaxKind.EqualsEqualsToken)
            {
                return true;
            }
            
            if (kind1 == SyntaxKind.StarToken && kind2 == SyntaxKind.StarToken)
            {
                return true;
            }
            
            if (kind1 == SyntaxKind.StarStarToken && kind2 == SyntaxKind.EqualsToken)
            {
                //return true;
            }
            
            if (kind1 == SyntaxKind.StarToken && kind2 == SyntaxKind.StarStarToken)
            {
                return true;
            }

            if (kind1 == SyntaxKind.VariableToken && kind2 == SyntaxKind.NumberToken)
            {
                return true;
            }

            if (kind1 == SyntaxKind.MinusToken && kind2 == SyntaxKind.EqualsEqualsToken)
            {
                return true;
            }
            
            if (kind1 == SyntaxKind.PlusToken && kind2 == SyntaxKind.EqualsEqualsToken)
            {
                return true;
            }
            
            if (kind1 == SyntaxKind.SlashToken && kind2 == SyntaxKind.EqualsEqualsToken)
            {
                return true;
            }
            
            if (kind1 == SyntaxKind.StarToken && kind2 == SyntaxKind.EqualsEqualsToken)
            {
                return true;
            }
            
            if (kind1 == SyntaxKind.ModuloToken && kind2 == SyntaxKind.EqualsEqualsToken)
            {
                return true;
            }
            
            //
            
            if (kind1 == SyntaxKind.MinusToken && kind2 == SyntaxKind.EqualsToken)
            {
                return true;
            }
            
            if (kind1 == SyntaxKind.PlusToken && kind2 == SyntaxKind.EqualsToken)
            {
                return true;
            }
            
            if (kind1 == SyntaxKind.SlashToken && kind2 == SyntaxKind.EqualsToken)
            {
                return true;
            }
            
            if (kind1 == SyntaxKind.StarToken && kind2 == SyntaxKind.EqualsToken)
            {
                return true;
            }
            
            if (kind1 == SyntaxKind.ModuloToken && kind2 == SyntaxKind.EqualsToken)
            {
                return true;
            }

            if (kind1 == SyntaxKind.StarToken && kind2 == SyntaxKind.StarEqualsToken)
            {
                return true;
            }

            return false;
        }

        static IEnumerable<(SyntaxKind kind1, string text1, SyntaxKind kind2, string text2)> GetTokenPairs()
        {
            foreach (var t1 in GetTokens())
            {
                foreach (var t2 in GetTokens())
                {
                    if (!RequiresSeparator(t1.kind, t2.kind))
                    {
                        yield return (t1.kind, t1.text, t2.kind, t2.text);   
                    }
                }
            }
        }
        
        static IEnumerable<(SyntaxKind kind1, string text1, SyntaxKind separatorkind, 
            string separatortext, SyntaxKind kind2, string text2)> GetTokenPairsWithSeparator()
        {
            foreach (var t1 in GetTokens())
            {
                foreach (var t2 in GetTokens())
                {
                    if (!RequiresSeparator(t1.kind, t2.kind))
                    {
                        foreach (var s in GetSeparators())
                        {
                            yield return (t1.kind, t1.text, s.kind, s.text, t2.kind, t2.text);      
                        }
                    }
                }
            }
        }
    }
}