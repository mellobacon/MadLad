using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using MadLad.Compiler.CodeAnalysis.ErrorReporting;
using MadLad.Compiler.CodeAnalysis.Syntax.Expressions;
using MadLad.Compiler.CodeAnalysis.Syntax.Text;

namespace MadLad.Compiler.CodeAnalysis.Syntax
{
    public class SyntaxTree
    {
        public readonly ImmutableArray<Error> Errors;
        public readonly SourceText Text;
        public readonly CompilationUnit Root;

        private SyntaxTree(SourceText text)
        {
            var parser = new Parser.Parser(text);
            Root = parser.ParseCompilationUnit();
            Text = text;
            Errors = parser.Errors.ToImmutableArray();
        }

        public static SyntaxTree Parse(string text)
        {
            // Convert the text into sourcetext
            var sourcetext = SourceText.From(text);
            // Lex the input
            //var parser = new Parser.Parser(sourcetext);
            // Parse the input
            return new SyntaxTree(sourcetext);
        }

        public static IEnumerable<SyntaxToken> ParseTokens(string text)
        {
            var lexer = new Lexer.Lexer(SourceText.From(text));
            while (true)
            {
                var token = lexer.Lex();
                if (token.Kind == SyntaxKind.EOFToken)
                {
                    break;
                }

                yield return token;
            }
        }
    }
}