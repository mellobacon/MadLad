using System.Collections.Generic;
using MadLad.Compiler.CodeAnalysis.Syntax.Expressions;

namespace MadLad.Compiler.CodeAnalysis.Syntax.Statements
{
    public class WhileStatement : StatementSyntax
    {
        readonly SyntaxToken Whilekeyword;
        public readonly ExpressionSyntax Condition;
        public readonly StatementSyntax Dostatement;

        public WhileStatement(SyntaxToken whilekeyword, ExpressionSyntax condition, StatementSyntax dostatement)
        {
            Whilekeyword = whilekeyword;
            Condition = condition;
            Dostatement = dostatement;
        }

        public override SyntaxKind Kind => SyntaxKind.WhileStatement;
        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Whilekeyword;
            yield return Condition;
            yield return Dostatement;
        }
    }
}