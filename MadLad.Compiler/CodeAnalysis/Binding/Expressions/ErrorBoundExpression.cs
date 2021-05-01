using MadLad.Compiler.CodeAnalysis.Syntax.Symbols;

namespace MadLad.Compiler.CodeAnalysis.Binding.Expressions
{
    internal class ErrorBoundExpression : BoundExpression
    {
        public override BoundKind Kind => BoundKind.ErrorExpression;
        public override TypeSymbol Type => TypeSymbol.Error;
    }
}