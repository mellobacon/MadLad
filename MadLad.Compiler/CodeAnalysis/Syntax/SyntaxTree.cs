using System.Collections.Generic;
using System.Linq;
using MadLad.Compiler.CodeAnalysis.ErrorReporting;
using MadLad.Compiler.CodeAnalysis.Syntax.Expressions;
using MadLad.Compiler.CodeAnalysis.Syntax.Text;

namespace MadLad.Compiler.CodeAnalysis.Syntax
{
    public class SyntaxTree
    {
        public readonly IEnumerable<Error> Errors;
        public readonly SourceText Text;
        public readonly ExpressionSyntax Root;
        private readonly SyntaxToken Eoftoken;

        public SyntaxTree(SourceText text, IEnumerable<Error> errors, ExpressionSyntax root, SyntaxToken eoftoken)
        {
            Errors = errors.ToArray();
            Text = text;
            Root = root;
            Eoftoken = eoftoken;
        }

        public static SyntaxTree Parse(string text)
        {
            // Convert the text into sourcetext
            var sourcetext = SourceText.From(text);
            // Lex the input
            var parser = new Parser.Parser(sourcetext);
            // Parse the input
            return parser.Parse();
        }
    }
}