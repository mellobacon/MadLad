﻿using System.Collections.Generic;
using MadLad.Compiler.CodeAnalysis.Syntax.Expressions;

namespace MadLad.Compiler.CodeAnalysis.Syntax.Statements
{
    public class VariableDeclaration : StatementSyntax
    {
        public readonly SyntaxToken Declarationkeyword;
        public readonly SyntaxToken Variable;
        public new readonly SyntaxToken Equals;
        public readonly ExpressionSyntax Expression;
        readonly SyntaxToken Semicolon;

        // var x = 10;
        public VariableDeclaration(SyntaxToken declarationkeyword, SyntaxToken variable, SyntaxToken equals,
            ExpressionSyntax expression, SyntaxToken semicolon)
        {
            Declarationkeyword = declarationkeyword;
            Variable = variable;
            Equals = equals;
            Expression = expression;
            Semicolon = semicolon;
        }

        public override SyntaxKind Kind => SyntaxKind.VariableDeclaration;
        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Declarationkeyword;
            yield return Variable;
            yield return Equals;
            yield return Expression;
            yield return Semicolon;
        }
    }
}