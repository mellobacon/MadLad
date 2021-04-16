using System.Collections.Generic;
using System.Collections.Immutable;
using MadLad.Compiler.CodeAnalysis.Syntax;

namespace MadLad.Compiler.CodeAnalysis.Binding
{
    internal sealed class BoundScope
    {
        public readonly BoundScope Parent;
        private readonly Dictionary<string, Variable> variables = new();

        public BoundScope(BoundScope parent)
        {
            Parent = parent;
        }

        public bool TryDeclare(Variable variable)
        {
            // checks if the variable is declared. if it is, dont redeclare it
            if (variables.ContainsKey(variable.Name))
            {
                return false;
            }
            variables.Add(variable.Name, variable);
            return true;
        }

        public bool TryLookup(string name, out Variable variable)
        {
            // checks if the declared variable has a value
            if (variables.TryGetValue(name, out variable))
            {
                return true;
            }

            if (Parent == null)
            {
                return false;
            }

            return Parent.TryLookup(name, out variable);
        }

        public ImmutableArray<Variable> GetDeclaredVariables()
        {
            return variables.Values.ToImmutableArray();
        }
    }
}