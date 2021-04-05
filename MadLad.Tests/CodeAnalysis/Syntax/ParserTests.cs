using System.Collections.Generic;
using MadLad.Compiler.CodeAnalysis.Syntax;
using Xunit;

namespace MadLad.Tests.CodeAnalysis.Syntax
{
    public class ParserTests
    {
        [Theory]
        [MemberData(nameof(GetBinaryOperatorPairsData))]
        public void Parse_Parser_BinaryExpression_HonorsPrecedence(SyntaxKind op1, SyntaxKind op2)
        {
            var op1Precendence = op1.GetBinaryOperatorPrecedence();
            var op2Precendence = op2.GetBinaryOperatorPrecedence();
            var op1Text = SyntaxPrecedences.GetText(op1);
            var op2Text = SyntaxPrecedences.GetText(op2);
            var text = $"a {op1Text} b {op2Text} c";
            var expression = SyntaxTree.Parse(text).Root;
            if (op1Precendence >= op2Precendence)
            {
                using var e = new AssertingEnumerator(expression);
                e.AssertNode(SyntaxKind.BinaryExpression);
                e.AssertNode(SyntaxKind.BinaryExpression);
                e.AssertNode(SyntaxKind.NameExpression);
                e.AssertToken(SyntaxKind.VariableToken, "a");
                e.AssertToken(op1, op1Text);
                e.AssertNode(SyntaxKind.NameExpression);
                e.AssertToken(SyntaxKind.VariableToken, "b");
                e.AssertToken(op2, op2Text);
                e.AssertNode(SyntaxKind.NameExpression);
                e.AssertToken(SyntaxKind.VariableToken, "c");
            }
            else
            {
                using var e = new AssertingEnumerator(expression);
                e.AssertNode(SyntaxKind.BinaryExpression);
                e.AssertNode(SyntaxKind.NameExpression);
                e.AssertToken(SyntaxKind.VariableToken, "a");
                e.AssertToken(op1, op1Text);
                e.AssertNode(SyntaxKind.BinaryExpression);
                e.AssertNode(SyntaxKind.NameExpression);
                e.AssertToken(SyntaxKind.VariableToken, "b");
                e.AssertToken(op2, op2Text);
                e.AssertNode(SyntaxKind.NameExpression);
                e.AssertToken(SyntaxKind.VariableToken, "c");
            }
        }

        [Theory]
        [MemberData(nameof(GetUnaryOperatorPairsData))]
        public void Parse_Parser_UnaryExpression_HonorsPrecedence(SyntaxKind unarykind, SyntaxKind binarykind)
        {
            var unaryPrecendence = unarykind.GetUnaryOperatorPrecedence();
            var binaryPrecendence = binarykind.GetUnaryOperatorPrecedence();
            var unaryText = SyntaxPrecedences.GetText(unarykind);
            var binaryText = SyntaxPrecedences.GetText(binarykind);
            var text = $"{unaryText} a {binaryText} b";
            var expression = SyntaxTree.Parse(text).Root;
            if (unaryPrecendence >= binaryPrecendence)
            {
                using var e = new AssertingEnumerator(expression);
                e.AssertNode(SyntaxKind.BinaryExpression);
                e.AssertNode(SyntaxKind.UnaryExpression);
                e.AssertToken(unarykind, unaryText);
                e.AssertNode(SyntaxKind.NameExpression);
                e.AssertToken(SyntaxKind.VariableToken, "a");
                e.AssertToken(binarykind, binaryText);
                e.AssertNode(SyntaxKind.NameExpression);
                e.AssertToken(SyntaxKind.VariableToken, "b");
            }
            else
            {
                using var e = new AssertingEnumerator(expression);
                e.AssertNode(SyntaxKind.UnaryExpression);
                e.AssertToken(unarykind, unaryText);
                e.AssertNode(SyntaxKind.BinaryExpression);
                e.AssertNode(SyntaxKind.NameExpression);
                e.AssertToken(SyntaxKind.VariableToken, "a");
                e.AssertToken(binarykind, binaryText);
                e.AssertNode(SyntaxKind.NameExpression);
                e.AssertToken(SyntaxKind.VariableToken, "b");
            }
        }

        public static IEnumerable<object[]> GetBinaryOperatorPairsData()
        {
            foreach (var op1 in SyntaxPrecedences.GetBinaryOperatorKinds())
            {
                foreach (var op2 in SyntaxPrecedences.GetBinaryOperatorKinds())
                {
                    yield return new object[] {op1, op2};
                }
            }
        }

        public static IEnumerable<object[]> GetUnaryOperatorPairsData()
        {
            foreach (var unary in SyntaxPrecedences.GetUnaryOperatorKinds())
            {
                foreach (var binary in SyntaxPrecedences.GetBinaryOperatorKinds())
                {
                    yield return new object[] {unary, binary};
                }
            }
        }
    }
}
