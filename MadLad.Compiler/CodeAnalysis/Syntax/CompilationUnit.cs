using System.Collections.Generic;
using MadLad.Compiler.CodeAnalysis.Syntax.Expressions;
using MadLad.Compiler.CodeAnalysis.Syntax.Statements;

namespace MadLad.Compiler.CodeAnalysis.Syntax
{
    // used to make more than one syntax tree apparently
    public sealed class CompilationUnit : SyntaxNode
    {
        public readonly StatementSyntax Expression;
        readonly SyntaxToken Eoftoken;

        public CompilationUnit(StatementSyntax expression, SyntaxToken eoftoken)
        {
            Expression = expression;
            Eoftoken = eoftoken;
        }

        public override SyntaxKind Kind => SyntaxKind.CompilationUnit;
        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Expression;
            yield return Eoftoken;
        }
    }
}