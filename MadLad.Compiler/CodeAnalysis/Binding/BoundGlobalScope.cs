using System.Collections.Immutable;
using MadLad.Compiler.CodeAnalysis.Binding.Expressions;
using MadLad.Compiler.CodeAnalysis.ErrorReporting;
using MadLad.Compiler.CodeAnalysis.Syntax;

namespace MadLad.Compiler.CodeAnalysis.Binding
{
    public sealed class BoundGlobalScope
    {
        public readonly BoundGlobalScope Previous;
        public readonly ImmutableArray<Error> Errors;
        public readonly ImmutableArray<Variable> Variables;
        public readonly BoundExpression Expression;

        public BoundGlobalScope(BoundGlobalScope previous, ImmutableArray<Error> errors, 
            ImmutableArray<Variable> variables, BoundExpression expression)
        {
            Previous = previous;
            Errors = errors;
            Variables = variables;
            Expression = expression;
        }
    }
}