using System;

namespace MadLad.Compiler.CodeAnalysis.Syntax.Symbols
{
    public class VariableSymbol : Symbol
    {
        public readonly TypeSymbol Type;

        public VariableSymbol(string name, TypeSymbol type) : base(name)
        {
            Type = type;
        }

        protected override SymbolKind Kind => SymbolKind.Variable;
    }
}