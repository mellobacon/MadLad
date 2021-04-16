using System.Collections.Generic;
using MadLad.Compiler.CodeAnalysis.Syntax.Expressions;

namespace MadLad.Compiler.CodeAnalysis.Syntax.Statements
{
    public sealed class ForStatement : StatementSyntax
    {
        readonly SyntaxToken Forkeyword;
        readonly SyntaxToken Openparen;
        public readonly StatementSyntax Initialization;
        public readonly ExpressionStatement Condition;
        public readonly ExpressionSyntax Iteration;
        readonly SyntaxToken Closedparen;
        public readonly StatementSyntax Dostatement;
        
        public ForStatement(SyntaxToken forkeyword, SyntaxToken openparen, StatementSyntax initialization, 
            ExpressionStatement condition, ExpressionSyntax iteration, SyntaxToken closedparen, StatementSyntax dostatement)
        {
            Forkeyword = forkeyword;
            Openparen = openparen;
            Initialization = initialization;
            Condition = condition;
            Iteration = iteration;
            Closedparen = closedparen;
            Dostatement = dostatement;
        }

        public override SyntaxKind Kind => SyntaxKind.ForStatement;
        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Forkeyword;
            yield return Openparen;
            yield return Initialization;
            yield return Condition;
            yield return Iteration;
            yield return Closedparen;
            yield return Dostatement;
        }
    }
}