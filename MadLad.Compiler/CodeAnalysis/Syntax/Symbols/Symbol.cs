namespace MadLad.Compiler.CodeAnalysis.Syntax.Symbols
{
    public abstract class Symbol
    {
        public readonly string Name;

        private protected Symbol(string name)
        {
            Name = name;
        }

        protected abstract SymbolKind Kind { get; }
        
        public override string ToString() => Name;
    }
}