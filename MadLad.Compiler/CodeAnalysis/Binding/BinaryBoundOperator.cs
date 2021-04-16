using System;
using MadLad.Compiler.CodeAnalysis.Syntax;

namespace MadLad.Compiler.CodeAnalysis.Binding
{
    internal sealed class BinaryBoundOperator
    {
        private readonly SyntaxKind SyntaxKind;
        public readonly BinaryBoundOperatorKind BoundKind;
        private readonly Type LeftType;
        private readonly Type RightType;
        public readonly Type ResultType;

        private BinaryBoundOperator(SyntaxKind syntaxKind, BinaryBoundOperatorKind kind, Type type) : this(syntaxKind, kind, type, type, type) { }

        private BinaryBoundOperator(SyntaxKind syntaxKind, BinaryBoundOperatorKind kind, Type operandType, Type resultType) 
            : this(syntaxKind, kind, operandType, operandType, resultType) { }
        private BinaryBoundOperator(SyntaxKind syntaxkind, BinaryBoundOperatorKind boundkind, Type lefttype, Type righttype, Type resulttype)
        {
            SyntaxKind = syntaxkind;
            BoundKind = boundkind;
            LeftType = lefttype;
            RightType = righttype;
            ResultType = resulttype;
        }

        private static readonly BinaryBoundOperator[] operations =
        {
            new(SyntaxKind.PlusToken, BinaryBoundOperatorKind.Addition, typeof(int)),
            new(SyntaxKind.PlusToken, BinaryBoundOperatorKind.Addition, typeof(float)),

            new(SyntaxKind.MinusToken, BinaryBoundOperatorKind.Subtraction, typeof(int)),
            new(SyntaxKind.MinusToken, BinaryBoundOperatorKind.Subtraction, typeof(float)),

            new(SyntaxKind.SlashToken, BinaryBoundOperatorKind.Division, typeof(int)),
            new(SyntaxKind.SlashToken, BinaryBoundOperatorKind.Division, typeof(float)),

            new(SyntaxKind.StarToken, BinaryBoundOperatorKind.Multiplication, typeof(int)),
            new(SyntaxKind.StarToken, BinaryBoundOperatorKind.Multiplication, typeof(float)),

            new(SyntaxKind.EqualsEqualsToken, BinaryBoundOperatorKind.Equals, typeof(int)),
            new(SyntaxKind.EqualsEqualsToken, BinaryBoundOperatorKind.Equals, typeof(float)),
            new(SyntaxKind.EqualsEqualsToken, BinaryBoundOperatorKind.Equals, typeof(bool)),

            new(SyntaxKind.EqualsEqualsToken, BinaryBoundOperatorKind.Equals, typeof(int), typeof(bool)),
            new(SyntaxKind.EqualsEqualsToken, BinaryBoundOperatorKind.Equals, typeof(float), typeof(bool)),

            new(SyntaxKind.NotEqualsToken, BinaryBoundOperatorKind.NotEquals, typeof(bool)),
            new(SyntaxKind.NotEqualsToken, BinaryBoundOperatorKind.NotEquals, typeof(int), typeof(bool)),
            new(SyntaxKind.NotEqualsToken, BinaryBoundOperatorKind.NotEquals, typeof(float), typeof(bool)),
            
            new(SyntaxKind.LessThanToken, BinaryBoundOperatorKind.LessThan, typeof(int), typeof(bool)),
            new(SyntaxKind.GreaterThanToken, BinaryBoundOperatorKind.GreaterThan, typeof(int), typeof(bool)),
            new(SyntaxKind.LessThanToken, BinaryBoundOperatorKind.LessThan, typeof(float), typeof(bool)),
            new(SyntaxKind.GreaterThanToken, BinaryBoundOperatorKind.GreaterThan, typeof(float), typeof(bool)),
            
            new(SyntaxKind.LessEqualsToken, BinaryBoundOperatorKind.LessOrEqual, typeof(int), typeof(bool)),
            new(SyntaxKind.GreatEqualsToken, BinaryBoundOperatorKind.GreaterOrEqual, typeof(int), typeof(bool)),
            new(SyntaxKind.LessEqualsToken, BinaryBoundOperatorKind.LessOrEqual, typeof(float), typeof(bool)),
            new(SyntaxKind.GreatEqualsToken, BinaryBoundOperatorKind.GreaterOrEqual, typeof(float), typeof(bool)),

            new(SyntaxKind.AndAmpersandToken, BinaryBoundOperatorKind.LogicalAnd, typeof(bool)),
            new(SyntaxKind.OrPipeToken, BinaryBoundOperatorKind.LogicalOr, typeof(bool)),
        };

        public static BinaryBoundOperator Bind(Type leftType, SyntaxKind kind, Type rightType)
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