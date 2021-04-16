using System;

namespace MadLad.Compiler.CodeAnalysis.Binding.Expressions
{
    // Returns the value of the literal expression
    internal sealed class LiteralBoundExpression : BoundExpression
    {
        public readonly object Value;

        public LiteralBoundExpression(object value)
        {
            Value = value;
        }

        public override BoundKind Kind => BoundKind.LiteralExpression;
        public override Type Type => Value.GetType();
    }
}