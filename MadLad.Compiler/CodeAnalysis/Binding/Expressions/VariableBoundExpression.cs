using System;
using MadLad.Compiler.CodeAnalysis.Syntax;

namespace MadLad.Compiler.CodeAnalysis.Binding.Expressions
{
    internal sealed class VariableBoundExpression : BoundExpression
    {
        public readonly Variable Variable;
        public override Type Type => Variable.Type;

        public VariableBoundExpression(Variable variable)
        {
            Variable = variable;
        }

        public override BoundKind Kind => BoundKind.VariableExpression;
    }
}