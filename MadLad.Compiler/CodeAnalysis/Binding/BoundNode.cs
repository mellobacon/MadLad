namespace MadLad.Compiler.CodeAnalysis.Binding
{
    public abstract class BoundNode
    {
        public abstract BoundKind Kind { get; }
    }
}