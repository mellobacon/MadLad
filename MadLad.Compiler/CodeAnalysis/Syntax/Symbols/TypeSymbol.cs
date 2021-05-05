namespace MadLad.Compiler.CodeAnalysis.Syntax.Symbols
{
    public class TypeSymbol : Symbol
    {
        public static readonly TypeSymbol String = new("string");
        public static readonly TypeSymbol Int = new("int");
        public static readonly TypeSymbol Bool = new("bool");
        public static readonly TypeSymbol Float = new("float");
        public static TypeSymbol Error = new("undefined");
        public static TypeSymbol Void = new("void");

        private TypeSymbol(string name) : base(name) { }

        protected override SymbolKind Kind => SymbolKind.Type;
    }
}