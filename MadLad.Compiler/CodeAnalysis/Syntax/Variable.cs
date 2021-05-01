using System;
using MadLad.Compiler.CodeAnalysis.Syntax.Symbols;

namespace MadLad.Compiler.CodeAnalysis.Syntax
{
    public class Variable
    {
        private readonly string Name;
        private readonly TypeSymbol Type;
        
        internal Variable(string name, TypeSymbol type)
        {
            Name = name;
            Type = type;
        }
    }
}