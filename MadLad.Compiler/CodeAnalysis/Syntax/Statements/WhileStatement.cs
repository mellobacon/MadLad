using System.Collections.Generic;
using MadLad.Compiler.CodeAnalysis.Syntax.Expressions;

namespace MadLad.Compiler.CodeAnalysis.Syntax.Statements
{
    public sealed class WhileStatement : StatementSyntax
    {
        readonly SyntaxToken Whilekeyword;
        readonly SyntaxToken Openparen;
        public readonly ExpressionSyntax Condition;
        readonly SyntaxToken Closeparen;
        public readonly StatementSyntax Dostatement;

        public WhileStatement(SyntaxToken whilekeyword, SyntaxToken openparen, ExpressionSyntax condition, SyntaxToken closeparen, 
            StatementSyntax dostatement)
        {
            Whilekeyword = whilekeyword;
            Openparen = openparen;
            Condition = condition;
            Closeparen = closeparen;
            Dostatement = dostatement;
        }

        public override SyntaxKind Kind => SyntaxKind.WhileStatement;
        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Whilekeyword;
            yield return Openparen;
            yield return Condition;
            yield return Closeparen;
            yield return Dostatement;
        }
    }
}