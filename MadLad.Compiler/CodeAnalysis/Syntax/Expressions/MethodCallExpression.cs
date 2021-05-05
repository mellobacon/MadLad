using System.Collections.Generic;
using System.Collections.Immutable;
using MadLad.Compiler.CodeAnalysis.Syntax.Symbols;

namespace MadLad.Compiler.CodeAnalysis.Syntax.Expressions
{
    public class MethodCallExpression : ExpressionSyntax
    {
        public readonly SyntaxToken Identifier;
        private readonly SyntaxToken Openparen;
        public readonly SeperatedSyntaxList<ExpressionSyntax> Args;
        private readonly SyntaxToken Closedparen;

        // print("hello");
        // print(thing);
        // print(1, 2);
        // add(5, 6);
        public MethodCallExpression(SyntaxToken identifier, SyntaxToken openparen, 
            SeperatedSyntaxList<ExpressionSyntax> args, SyntaxToken closedparen)
        {
            Identifier = identifier;
            Openparen = openparen;
            Args = args;
            Closedparen = closedparen;
        }

        public override SyntaxKind Kind => SyntaxKind.MethodExpression;
        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Identifier;
            yield return Openparen;
            foreach (var arg in Args)
            {
                yield return arg;
            }

            yield return Closedparen;
        }
    }
}