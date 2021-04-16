using System.Collections.Generic;
using System.Collections.Immutable;

namespace MadLad.Compiler.CodeAnalysis.Syntax.Statements
{
    public sealed class BlockStatement : StatementSyntax
    {
        readonly SyntaxToken Openbracket;
        public readonly ImmutableArray<StatementSyntax> Statements;
        readonly SyntaxToken Closedbracket;

        public BlockStatement(SyntaxToken openbracket, ImmutableArray<StatementSyntax> statements , SyntaxToken closedbracket)
        {
            Openbracket = openbracket;
            Statements = statements;
            Closedbracket = closedbracket;
        }

        public override SyntaxKind Kind => SyntaxKind.BlockStatement;
        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Openbracket;
            foreach (var statement in Statements)
            {
                yield return statement;
            }
            yield return Closedbracket;
        }
    }
}
