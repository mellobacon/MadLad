namespace MadLad.Compiler.CodeAnalysis.Binding
{
    internal abstract class BoundNode
    {
        public abstract BoundKind Kind { get; }
    }
}