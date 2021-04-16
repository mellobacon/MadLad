using System.Collections.Generic;

namespace MadLad.Compiler.CodeAnalysis.Syntax.Expressions
{
    // Represents a binary expression ie 2 + 2 or like...5 / -7 or something
    public sealed class BinaryExpression : ExpressionSyntax
    {
        public readonly ExpressionSyntax Left;
        public readonly SyntaxToken Op;
        public readonly ExpressionSyntax Right;

        public BinaryExpression(ExpressionSyntax left, SyntaxToken op, ExpressionSyntax right)
        {
            Left = left;
            Op = op;
            Right = right;
        }
        
        public override SyntaxKind Kind => SyntaxKind.BinaryExpression;
        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Left;
            yield return Op;
            yield return Right;
        }
    }
}
