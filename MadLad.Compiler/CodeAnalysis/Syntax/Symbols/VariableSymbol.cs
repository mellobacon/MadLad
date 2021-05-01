using System;

namespace MadLad.Compiler.CodeAnalysis.Syntax.Symbols
{
    public class VariableSymbol : TypeSymbol
    {
        public readonly TypeSymbol Type;

        public VariableSymbol(string name, TypeSymbol type) : base(name)
        {
            Type = type;
        }

        public override string ToString() => Name;
    }
}