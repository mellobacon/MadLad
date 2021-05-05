using System.Collections.Immutable;
using MadLad.Compiler.CodeAnalysis.Syntax.Symbols;

namespace MadLad.Compiler.CodeAnalysis.Binding.Expressions
{
    internal class MethodBoundExpression : BoundExpression
    {
        public readonly FunctionSymbol Function;
        public readonly ImmutableArray<BoundExpression> Args;

        public MethodBoundExpression(FunctionSymbol function, ImmutableArray<BoundExpression> args)
        {
            Function = function;
            Args = args;
        }

        public override BoundKind Kind => BoundKind.MethodExpression;
        public override TypeSymbol Type => Function.Type;
    }
}