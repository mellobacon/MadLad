using System;

namespace MadLad.MadLad.Compiler.Binding.Expressions
{
    public abstract class BoundExpression : BoundNode
    {
        public abstract Type Type { get; }
    }
}