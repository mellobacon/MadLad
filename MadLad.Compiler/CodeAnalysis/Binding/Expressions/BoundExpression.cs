using System;

namespace MadLad.Compiler.CodeAnalysis.Binding.Expressions
{
    internal abstract class BoundExpression : BoundNode
    {
        public abstract Type Type { get; }
    }
}