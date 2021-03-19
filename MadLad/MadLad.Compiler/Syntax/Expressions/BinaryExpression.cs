using System.Collections.Generic;

namespace MadLad.MadLad.Compiler.Syntax.Expressions
{
    public class BinaryExpression : ExpressionSyntax
    {
        readonly ExpressionSyntax Left;
        readonly SyntaxToken Op;
        readonly ExpressionSyntax Right;

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
