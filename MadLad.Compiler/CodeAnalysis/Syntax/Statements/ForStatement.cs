using System.Collections.Generic;
using MadLad.Compiler.CodeAnalysis.Syntax.Expressions;

namespace MadLad.Compiler.CodeAnalysis.Syntax.Statements
{
    public sealed class ForStatement : StatementSyntax
    {
        public SyntaxToken Forkeyword;
        public SyntaxToken Openparen;
        public StatementSyntax Initializer;
        public ExpressionSyntax Condition;
        public SyntaxToken Semicolon;
        public ExpressionSyntax Iterator;
        public SyntaxToken Closedparen;
        public StatementSyntax Dostatement;


        /**
         * if (var x = 0; x < 1; x++) {}
         */
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