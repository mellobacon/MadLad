using System;
using System.Collections.Immutable;

namespace MadLad.Compiler.CodeAnalysis.Syntax.Symbols
{
    public class FunctionSymbol : Symbol
    {
        public readonly ImmutableArray<ParameterSymbol> Parameter;
        public readonly TypeSymbol Type;

        internal FunctionSymbol(string name, ImmutableArray<ParameterSymbol> parameter, TypeSymbol type) : base(name)
        {
            Parameter = parameter;
            Type = type;
        }

        protected override SymbolKind Kind => SymbolKind.Function;
    }
}