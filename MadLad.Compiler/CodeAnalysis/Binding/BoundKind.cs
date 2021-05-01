namespace MadLad.Compiler.CodeAnalysis.Binding
{
    internal enum BoundKind
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
        VariableExpression,
        ErrorExpression
    }
}