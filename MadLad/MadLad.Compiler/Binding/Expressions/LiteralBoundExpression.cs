using System;

namespace MadLad.MadLad.Compiler.Binding.Expressions
{
    // Returns the value of the literal expression
    public class LiteralBoundExpression : BoundExpression
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