using System;

namespace MadLad.Compiler.CodeAnalysis.Binding.Expressions
{
    public abstract class BoundExpression : BoundNode
    {
        public abstract Type Type { get; }
    }
}