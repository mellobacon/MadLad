using System;
using MadLad.Compiler.CodeAnalysis.Syntax;

namespace MadLad.Compiler.CodeAnalysis.Binding.Expressions
{
    internal sealed class AssignmentBoundExpression : BoundExpression
    {
        public bool Iscompoundoperator;
        public readonly Variable Variable;
        public readonly BoundExpression Expression;
        public readonly SyntaxToken Compoundoperator;

        public AssignmentBoundExpression(Variable variable, BoundExpression expression, SyntaxToken compoundoperator, bool iscompoundoperator)
        {
            Variable = variable;
            Expression = expression;
            Compoundoperator = compoundoperator;
            Iscompoundoperator = iscompoundoperator;
        }

        public override BoundKind Kind => BoundKind.AssignmentExpression;
        public override Type Type => Expression.Type;
    }
}