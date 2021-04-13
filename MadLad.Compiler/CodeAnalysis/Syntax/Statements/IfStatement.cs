using System.Collections.Generic;
using MadLad.Compiler.CodeAnalysis.Syntax.Expressions;

namespace MadLad.Compiler.CodeAnalysis.Syntax.Statements
{
    public class IfStatement : StatementSyntax
    {
        readonly SyntaxToken Ifkeyword;
        readonly SyntaxToken Openparen;
        public readonly ExpressionSyntax Condition;
        readonly SyntaxToken Closeparen;
        public readonly StatementSyntax Thenstatement;
        public readonly ElseStatement Elsestatement;
        
        public IfStatement(SyntaxToken ifkeyword, SyntaxToken openparen, ExpressionSyntax condition, SyntaxToken closeparen, 
            StatementSyntax thenstatement, ElseStatement elsestatement)
        {
            Ifkeyword = ifkeyword;
            Openparen = openparen;
            Condition = condition;
            Closeparen = closeparen;
            Thenstatement = thenstatement;
            Elsestatement = elsestatement;
        }
        
        public override SyntaxKind Kind => SyntaxKind.IfStatement;
        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Ifkeyword;
            yield return Openparen;
            yield return Condition;
            yield return Closeparen;
            yield return Thenstatement;
            yield return Elsestatement;
        }
    }
}