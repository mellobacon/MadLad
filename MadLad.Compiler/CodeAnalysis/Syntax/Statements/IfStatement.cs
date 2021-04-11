using System.Collections.Generic;
using MadLad.Compiler.CodeAnalysis.Syntax.Expressions;

namespace MadLad.Compiler.CodeAnalysis.Syntax.Statements
{
    public class IfStatement : StatementSyntax
    {
        readonly SyntaxToken Ifkeyword;
        public readonly ExpressionSyntax Condition;
        public readonly StatementSyntax Thenstatement;
        public readonly ElseStatement Elsestatement;

        // if ()
        // {
        //
        // }
        public IfStatement(SyntaxToken ifkeyword, ExpressionSyntax condition, StatementSyntax thenstatement,
            ElseStatement elsestatement)
        {
            Ifkeyword = ifkeyword;
            Condition = condition;
            Thenstatement = thenstatement;
            Elsestatement = elsestatement;
        }
        
        public override SyntaxKind Kind => SyntaxKind.IfStatement;
        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Ifkeyword;
            yield return Condition;
            yield return Thenstatement;
            yield return Elsestatement;
        }
    }
}