using MadLad.Compiler.CodeAnalysis.Syntax.Text;
using Xunit;

namespace MadLad.Tests.CodeAnalysis.Syntax.Text
{
    public class SourceTextTests
    {
        [Theory]
        [InlineData(".", 1)]
        [InlineData(".\r\n", 2)]
        [InlineData(".\r\n\r\n", 4)]
        public void SourceTextWithLines(string text, int linecount)
        {
            var sourcetext = SourceText.From(text);
            Assert.Equal(linecount, sourcetext.Lines.Length);
        }
    }
}