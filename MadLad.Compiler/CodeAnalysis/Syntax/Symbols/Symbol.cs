namespace MadLad.Compiler.CodeAnalysis.Syntax.Symbols
{
    public abstract class Symbol
    {
        public readonly string Name;

        protected abstract SymbolKind Kind { get; }

        private protected Symbol(string name)
        {
            Name = name;
        }

        public override string ToString() => Name;
    }
}