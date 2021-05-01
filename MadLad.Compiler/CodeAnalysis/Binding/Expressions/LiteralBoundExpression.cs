using System;
using MadLad.Compiler.CodeAnalysis.Syntax.Symbols;

namespace MadLad.Compiler.CodeAnalysis.Binding.Expressions
{
    // Returns the value of the literal expression
    internal sealed class LiteralBoundExpression : BoundExpression
    {
        public readonly object Value;

        public LiteralBoundExpression(object value)
        {
            Value = value;
            Type = value switch
            {
                bool => TypeSymbol.Bool,
                int => TypeSymbol.Int,
                float => TypeSymbol.Float,
                string => TypeSymbol.String,
                _ => throw new Exception($"Unexpected literal '{Value}' of type {Value.GetType()}")
            };
        }

        public override BoundKind Kind => BoundKind.LiteralExpression;
        public override TypeSymbol Type { get; }
    }
}