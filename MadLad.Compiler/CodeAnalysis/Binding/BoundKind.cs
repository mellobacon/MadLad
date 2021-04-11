namespace MadLad.Compiler.CodeAnalysis.Binding
{
    public enum BoundKind
    {
        // Statements
        BlockStatement,
        ExpressionStatement,
        VariableDeclaration,
        IfStatement,

        // Expressions
        LiteralExpression,
        BinaryExpression,
        UnaryExpression,
        AssignmentExpression,
        VariableExpression
    }
}