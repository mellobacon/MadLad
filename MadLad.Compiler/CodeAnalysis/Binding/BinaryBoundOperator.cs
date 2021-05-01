using MadLad.Compiler.CodeAnalysis.Syntax;
using MadLad.Compiler.CodeAnalysis.Syntax.Symbols;

namespace MadLad.Compiler.CodeAnalysis.Binding
{
    internal sealed class BinaryBoundOperator
    {
        private readonly SyntaxKind SyntaxKind;
        public readonly BinaryBoundOperatorKind BoundKind;
        private readonly TypeSymbol LeftType;
        private readonly TypeSymbol RightType;
        public readonly TypeSymbol ResultType;

        private BinaryBoundOperator(SyntaxKind syntaxKind, BinaryBoundOperatorKind kind, TypeSymbol TypeSymbol) : this(syntaxKind, kind, TypeSymbol, TypeSymbol, TypeSymbol) { }

        private BinaryBoundOperator(SyntaxKind syntaxKind, BinaryBoundOperatorKind kind, TypeSymbol operandType, TypeSymbol resultType) 
            : this(syntaxKind, kind, operandType, operandType, resultType) { }
        private BinaryBoundOperator(SyntaxKind syntaxkind, BinaryBoundOperatorKind boundkind, TypeSymbol lefttype, TypeSymbol righttype, TypeSymbol resulttype)
        {
            SyntaxKind = syntaxkind;
            BoundKind = boundkind;
            LeftType = lefttype;
            RightType = righttype;
            ResultType = resulttype;
        }

        private static readonly BinaryBoundOperator[] operations =
        {
            new(SyntaxKind.PlusToken, BinaryBoundOperatorKind.Addition, TypeSymbol.Int),
            new(SyntaxKind.PlusToken, BinaryBoundOperatorKind.Addition, TypeSymbol.Float),

            new(SyntaxKind.MinusToken, BinaryBoundOperatorKind.Subtraction, TypeSymbol.Int),
            new(SyntaxKind.MinusToken, BinaryBoundOperatorKind.Subtraction, TypeSymbol.Float),

            new(SyntaxKind.SlashToken, BinaryBoundOperatorKind.Division, TypeSymbol.Int),
            new(SyntaxKind.SlashToken, BinaryBoundOperatorKind.Division, TypeSymbol.Float),

            new(SyntaxKind.StarToken, BinaryBoundOperatorKind.Multiplication, TypeSymbol.Int),
            new(SyntaxKind.StarToken, BinaryBoundOperatorKind.Multiplication, TypeSymbol.Float),
            
            new(SyntaxKind.ModuloToken, BinaryBoundOperatorKind.Modulo, TypeSymbol.Int),
            new(SyntaxKind.ModuloToken, BinaryBoundOperatorKind.Modulo, TypeSymbol.Float),
            
            new(SyntaxKind.StarStarToken, BinaryBoundOperatorKind.Pow, TypeSymbol.Int),
            new(SyntaxKind.StarStarToken, BinaryBoundOperatorKind.Pow, TypeSymbol.Float),

            new(SyntaxKind.EqualsEqualsToken, BinaryBoundOperatorKind.Equals, TypeSymbol.Int),
            new(SyntaxKind.EqualsEqualsToken, BinaryBoundOperatorKind.Equals, TypeSymbol.Float),
            new(SyntaxKind.EqualsEqualsToken, BinaryBoundOperatorKind.Equals, TypeSymbol.Bool),

            new(SyntaxKind.EqualsEqualsToken, BinaryBoundOperatorKind.Equals, TypeSymbol.Int, TypeSymbol.Bool),
            new(SyntaxKind.EqualsEqualsToken, BinaryBoundOperatorKind.Equals, TypeSymbol.Float, TypeSymbol.Bool),

            new(SyntaxKind.NotEqualsToken, BinaryBoundOperatorKind.NotEquals, TypeSymbol.Bool),
            new(SyntaxKind.NotEqualsToken, BinaryBoundOperatorKind.NotEquals, TypeSymbol.Int, TypeSymbol.Bool),
            new(SyntaxKind.NotEqualsToken, BinaryBoundOperatorKind.NotEquals, TypeSymbol.Float, TypeSymbol.Bool),
            
            new(SyntaxKind.LessThanToken, BinaryBoundOperatorKind.LessThan, TypeSymbol.Int, TypeSymbol.Bool),
            new(SyntaxKind.GreaterThanToken, BinaryBoundOperatorKind.GreaterThan, TypeSymbol.Int, TypeSymbol.Bool),
            new(SyntaxKind.LessThanToken, BinaryBoundOperatorKind.LessThan, TypeSymbol.Float, TypeSymbol.Bool),
            new(SyntaxKind.GreaterThanToken, BinaryBoundOperatorKind.GreaterThan, TypeSymbol.Float, TypeSymbol.Bool),
            
            new(SyntaxKind.LessEqualsToken, BinaryBoundOperatorKind.LessOrEqual, TypeSymbol.Int, TypeSymbol.Bool),
            new(SyntaxKind.GreatEqualsToken, BinaryBoundOperatorKind.GreaterOrEqual, TypeSymbol.Int, TypeSymbol.Bool),
            new(SyntaxKind.LessEqualsToken, BinaryBoundOperatorKind.LessOrEqual, TypeSymbol.Float, TypeSymbol.Bool),
            new(SyntaxKind.GreatEqualsToken, BinaryBoundOperatorKind.GreaterOrEqual, TypeSymbol.Float, TypeSymbol.Bool),

            new(SyntaxKind.AndAmpersandToken, BinaryBoundOperatorKind.LogicalAnd, TypeSymbol.Bool),
            new(SyntaxKind.OrPipeToken, BinaryBoundOperatorKind.LogicalOr, TypeSymbol.Bool),
        };

        public static BinaryBoundOperator Bind(TypeSymbol leftType, SyntaxKind kind, TypeSymbol rightType)
        {
            foreach (var op in operations)
            {
                if (op.SyntaxKind == kind && op.LeftType == leftType && op.RightType == rightType)
                {
                    return op;
                }
            }
            return null;
        }
    }
}