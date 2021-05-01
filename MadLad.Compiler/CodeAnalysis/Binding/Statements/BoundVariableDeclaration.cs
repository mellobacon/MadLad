using MadLad.Compiler.CodeAnalysis.Binding.Expressions;
using MadLad.Compiler.CodeAnalysis.Syntax;
using MadLad.Compiler.CodeAnalysis.Syntax.Symbols;

namespace MadLad.Compiler.CodeAnalysis.Binding.Statements
{
    internal sealed class BoundVariableDeclaration : BoundStatement
    {
        public readonly VariableSymbol Variable;
        public readonly BoundExpression Expression;

        public BoundVariableDeclaration(VariableSymbol variable, BoundExpression expression)
        {
            Variable = variable;
            Expression = expression;
        }

        public override BoundKind Kind => BoundKind.VariableDeclaration;
    }
}