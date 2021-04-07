﻿using System.Collections.Immutable;

namespace MadLad.Compiler.CodeAnalysis.Binding.Statements
{
    public class BlockBoundStatement : BoundStatement
    {
        public readonly ImmutableArray<BoundStatement> Statements;

        public BlockBoundStatement(ImmutableArray<BoundStatement> statements)
        {
            Statements = statements;
        }

        public override BoundKind Kind => BoundKind.BlockStatement;
    }
}