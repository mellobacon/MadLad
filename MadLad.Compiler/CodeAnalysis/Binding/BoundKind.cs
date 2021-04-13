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
        ForStatement,

        // Expressions
        LiteralExpression,
        BinaryExpression,
        UnaryExpression,
        AssignmentExpression,
        VariableExpression
    }
}