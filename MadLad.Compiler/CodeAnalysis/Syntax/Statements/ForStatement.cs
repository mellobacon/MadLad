using System.Collections.Generic;
using MadLad.Compiler.CodeAnalysis.Syntax.Expressions;

namespace MadLad.Compiler.CodeAnalysis.Syntax.Statements
{
    public sealed class ForStatement : StatementSyntax
    {
        private readonly SyntaxToken Forkeyword;
        private readonly SyntaxToken Openparen;
        public readonly StatementSyntax Initializer;
        public readonly ExpressionSyntax Condition;
        private readonly SyntaxToken Semicolon;
        public readonly ExpressionSyntax Iterator;
        private readonly SyntaxToken Closedparen;
        public readonly StatementSyntax Dostatement;
        
        public ForStatement(SyntaxToken forkeyword, SyntaxToken openparen, StatementSyntax initializer, 
            ExpressionSyntax condition, SyntaxToken semicolon, ExpressionSyntax iterator, SyntaxToken closedparen, 
            StatementSyntax dostatement)
        {
            Forkeyword = forkeyword;
            Openparen = openparen;
            Initializer = initializer;
            Condition = condition;
            Semicolon = semicolon;
            Iterator = iterator;
            Closedparen = closedparen;
            Dostatement = dostatement;
        }

        public override SyntaxKind Kind => SyntaxKind.ForStatement;
        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Forkeyword;
            yield return Openparen;
            yield return Initializer;
            yield return Condition;
            yield return Semicolon;
            yield return Iterator;
            yield return Closedparen;
            yield return Dostatement;
        }
    }
}