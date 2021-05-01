using System;
using MadLad.Compiler.CodeAnalysis.Syntax;
using MadLad.Compiler.CodeAnalysis.Syntax.Symbols;

namespace MadLad.Compiler.CodeAnalysis.Binding.Expressions
{
    internal sealed class AssignmentBoundExpression : BoundExpression
    {
        public readonly bool Iscompoundoperator;
        public readonly VariableSymbol Variable;
        public readonly BoundExpression Expression;
        public readonly SyntaxToken Compoundoperator;

        public AssignmentBoundExpression(VariableSymbol variable, BoundExpression expression, SyntaxToken compoundoperator, bool iscompoundoperator)
        {
            Variable = variable;
            Expression = expression;
            Compoundoperator = compoundoperator;
            Iscompoundoperator = iscompoundoperator;
        }

        public override BoundKind Kind => BoundKind.AssignmentExpression;
        public override TypeSymbol Type => Expression.Type;
    }
}