namespace MadLad.Compiler.CodeAnalysis.Syntax.Symbols
{
    public class ParameterSymbol : Symbol
    {
        public readonly TypeSymbol Type;

        internal ParameterSymbol(string name, TypeSymbol type) : base(name)
        {
            Type = type;
        }

        protected override SymbolKind Kind => SymbolKind.Parameter;
    }
}