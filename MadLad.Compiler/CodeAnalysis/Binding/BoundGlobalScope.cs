using System.Collections.Immutable;
using MadLad.Compiler.CodeAnalysis.Binding.Statements;
using MadLad.Compiler.CodeAnalysis.ErrorReporting;
using MadLad.Compiler.CodeAnalysis.Syntax;
using MadLad.Compiler.CodeAnalysis.Syntax.Symbols;

namespace MadLad.Compiler.CodeAnalysis.Binding
{
    internal sealed class BoundGlobalScope
    {
        public readonly BoundGlobalScope Previous;
        public readonly ImmutableArray<Error> Errors;
        public readonly ImmutableArray<VariableSymbol> Variables;
        public readonly BoundStatement Statement;

        public BoundGlobalScope(BoundGlobalScope previous, ImmutableArray<Error> errors, 
            ImmutableArray<VariableSymbol> variables, BoundStatement statement)
        {
            Previous = previous;
            Errors = errors;
            Variables = variables;
            Statement = statement;
        }
    }
}