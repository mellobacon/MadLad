using System;
using System.Collections.Generic;
using MadLad.Compiler.CodeAnalysis.Syntax;
using Xunit;

namespace MadLad.Tests.CodeAnalysis.Syntax
{
    public class SyntaxPrecedencesTests
    {
        [Theory]
        [MemberData(nameof(GetSyntaxKindData))]
        public void SyntaxFact_GetText_RoundTrips(SyntaxKind kind)
        {
            var text = SyntaxPrecedences.GetText(kind);

            if (text == null)
            {
                return;
            }

            var tokens = SyntaxTree.ParseTokens(text);
            var token = Assert.Single(tokens);

            if (token != null)
            {
                Assert.Equal(kind, token.Kind);
                Assert.Equal(text, token.Text);
            }
        }

        public static IEnumerable<object[]> GetSyntaxKindData()
        {
            var kinds = (SyntaxKind[]) Enum.GetValues(typeof(SyntaxKind));
            foreach (var kind in kinds)
            {
                yield return new object[] {kind};
            }
        }
    }
}