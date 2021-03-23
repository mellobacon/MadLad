using System;

namespace MadLad.MadLad.Compiler.Syntax
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