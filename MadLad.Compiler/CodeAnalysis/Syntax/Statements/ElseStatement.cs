using System.Collections.Generic;

namespace MadLad.Compiler.CodeAnalysis.Syntax.Statements
{
    public class ElseStatement : StatementSyntax
    {
        private readonly SyntaxToken Elsekeyword;
        public readonly StatementSyntax Elsestatement;

        public ElseStatement(SyntaxToken elsekeyword, StatementSyntax elsestatement)
        {
            Elsekeyword = elsekeyword;
            Elsestatement = elsestatement;
        }

        public override SyntaxKind Kind => SyntaxKind.ElseStatement;
        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Elsekeyword;
            yield return Elsestatement;
        }
    }
}