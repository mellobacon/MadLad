namespace MadLad.Compiler.CodeAnalysis.Binding
{
    public enum BoundKind
    {
        // Statements
        BlockStatement,
        ExpressionStatement,
        
        // Expressions
        LiteralExpression,
        BinaryExpression,
        UnaryExpression,
        AssignmentExpression,
        VariableExpression
    }
}