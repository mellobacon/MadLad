using System;
using MadLad.MadLad.Compiler.Syntax;

namespace MadLad.MadLad.Compiler.Binding.Expressions
{
    public class AssignmentBoundExpression : BoundExpression
    {
        public readonly Variable Variable;
        public readonly BoundExpression Expression;

        public AssignmentBoundExpression(Variable variable, BoundExpression expression)
        {
            Variable = variable;
            Expression = expression;
        }

        public override BoundKind Kind => BoundKind.AssignmentExpression;
        public override Type Type => Expression.Type;
    }
}