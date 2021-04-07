namespace MadLad.Compiler.CodeAnalysis.Binding
{
    public enum BoundKind
    {
        // Statements
        BlockStatement,
        ExpressionStatement,
        VariableDeclaration,
        
        // Expressions
        LiteralExpression,
        BinaryExpression,
        UnaryExpression,
        AssignmentExpression,
        VariableExpression
    }
}