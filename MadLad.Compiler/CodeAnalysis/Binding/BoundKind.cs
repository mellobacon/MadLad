namespace MadLad.Compiler.CodeAnalysis.Binding
{
    public enum BoundKind
    {
        // Statements
        BlockStatement,
        ExpressionStatement,
        VariableDeclaration,
        IfStatement,
        WhileStatement,

        // Expressions
        LiteralExpression,
        BinaryExpression,
        UnaryExpression,
        AssignmentExpression,
        VariableExpression
    }
}