using System;
using MadLad.MadLad.Compiler.Syntax;

namespace MadLad.MadLad.Compiler.Binding.Expressions
{
    public class VariableBoundExpression : BoundExpression
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