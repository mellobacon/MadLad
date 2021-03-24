using System;

namespace MadLad.Compiler.CodeAnalysis.Syntax
{
    public class Variable
    {
        public readonly string Name;
        public readonly Type Type;
        
        public Variable(string name, Type type)
        {
            Name = name;
            Type = type;
        }
    }
}