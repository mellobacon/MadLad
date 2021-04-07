﻿using System.Collections.Generic;
using MadLad.Compiler.CodeAnalysis.Evaluator;
using MadLad.Compiler.CodeAnalysis.Syntax;
using Xunit;

namespace MadLad.Tests.CodeAnalysis
{
    public class EvaluationTests
    {
        [Theory]
        [InlineData("5", 5)]
        [InlineData("-12", -12)]
        [InlineData("14 + 12", 26)]
        [InlineData("12 - 3", 9)]
        [InlineData("4 * 2", 8)]
        [InlineData("9 / 3", 3)]
        [InlineData("(10)", 10)]
        [InlineData("1.0 + 1.0", 2.0)]
        [InlineData("1 / 2", 0.5)]
        [InlineData("12 == 3", false)]
        [InlineData("3 == 3", true)]
        [InlineData("12 != 3", true)]
        [InlineData("!true", false)]
        [InlineData("false == false", true)]
        [InlineData("true == false", false)]
        [InlineData("a = 10", 10)]
        [InlineData("(a = 10)", 10)]
        [InlineData("1 + 2 * 3", 7)]
        [InlineData("(1 + 2) * 3", 9)]
        [InlineData("true", true)]
        [InlineData("false", false)]
        [InlineData("---1", -1)]
        [InlineData("true && true", true)]
        [InlineData("false || true", true)]
        [InlineData("true && false", false)]
        public void SyntaxFact_GetText_RoundTrips(string text, object expectedValue)
        {
            var syntaxTree = SyntaxTree.Parse(text);
            var compilation = new Compilation(syntaxTree);
            var variables = new Dictionary<Variable, object>();
            var result = compilation.Evaluate(variables);
            
            Assert.Empty(result.Errors);
            Assert.Equal(expectedValue, result.Value);
        }
    }
}