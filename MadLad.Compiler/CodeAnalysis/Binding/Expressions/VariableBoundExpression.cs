using System;
using MadLad.Compiler.CodeAnalysis.Syntax;
using MadLad.Compiler.CodeAnalysis.Syntax.Symbols;

namespace MadLad.Compiler.CodeAnalysis.Binding.Expressions
{
    internal sealed class VariableBoundExpression : BoundExpression
    {
        public readonly VariableSymbol Variable;

        public VariableBoundExpression(VariableSymbol variable)
        {
            Variable = variable;
        }

        public override BoundKind Kind => BoundKind.VariableExpression;
        public override TypeSymbol Type => Variable.Type;
    }
}