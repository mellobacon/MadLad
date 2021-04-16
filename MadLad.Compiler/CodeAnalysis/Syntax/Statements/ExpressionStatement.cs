using System.Collections.Generic;
using MadLad.Compiler.CodeAnalysis.Syntax.Expressions;

namespace MadLad.Compiler.CodeAnalysis.Syntax.Statements
{
    public sealed class ExpressionStatement : StatementSyntax
    {
        public readonly ExpressionSyntax Expression;
        readonly SyntaxToken Semicolon;

        public ExpressionStatement(ExpressionSyntax expression, SyntaxToken semicolon)
        {
            Expression = expression;
            Semicolon = semicolon;
        }

        public override SyntaxKind Kind => SyntaxKind.ExpressionStatement;
        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Expression;
            yield return Semicolon;
        }
    }
}