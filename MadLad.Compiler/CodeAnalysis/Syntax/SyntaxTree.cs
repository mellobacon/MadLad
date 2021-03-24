using System.Collections.Generic;
using System.Linq;
using MadLad.Compiler.CodeAnalysis.ErrorReporting;
using MadLad.Compiler.CodeAnalysis.Syntax.Expressions;

namespace MadLad.Compiler.CodeAnalysis.Syntax
{
    public class SyntaxTree
    {
        public readonly IEnumerable<Error> Errors;
        public readonly ExpressionSyntax Root;
        private readonly SyntaxToken Eoftoken;

        public SyntaxTree(IEnumerable<Error> errors, ExpressionSyntax root, SyntaxToken eoftoken)
        {
            Errors = errors.ToArray();
            Root = root;
            Eoftoken = eoftoken;
        }

        public static SyntaxTree Parse(string text)
        {
            // lex the input
            var parser = new Parser.Parser(text);
            // parse the input
            return parser.Parse();
        }
    }
}