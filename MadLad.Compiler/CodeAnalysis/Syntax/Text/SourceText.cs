using System.Collections.Immutable;

namespace MadLad.Compiler.CodeAnalysis.Syntax.Text
{
    public sealed class SourceText
    {
        private readonly string Text;
        public ImmutableArray<TextLine> Lines;
        
        private SourceText(string text)
        {
            Text = text;
            Lines = ParseLines(this, text);
        }
        
        private static ImmutableArray<TextLine> ParseLines(SourceText sourcetext, string text)
        {
            var result = ImmutableArray.CreateBuilder<TextLine>();
            var position = 0;
            var start = 0;
            while (position < text.Length)
            {
                var linebreakwidth = GetLineBreakWidth(text, position);
                if (linebreakwidth == 0)
                {
                    position++;
                }
                else
                {
                    AddLine(sourcetext, position, start, linebreakwidth, result);
                    position += linebreakwidth;
                    start = position;
                }
            }

            if (position >= start)
            {
                AddLine(sourcetext, position, start, 0, result);
            }

            return result.ToImmutable();
        }
        
        private static int GetLineBreakWidth(string text, int i)
        {
            var character = text[i];
            var length = i + 1 >= text.Length ? '\0' : text[i + 1];
            if (character == '\r' && length == '\n')
            {
                return 2;
            }

            if (character == '\r' || character == '\n')
            {
                return 1;
            }
            return 0;
        }

        private static void AddLine(SourceText text, int position, int start, int linebreakwidth, ImmutableArray<TextLine>.Builder result)
        {
            var linelength = position - start;
            var linelengthwithbreak = linelength + linebreakwidth;
            var line = new TextLine(text, start, linelength, linelengthwithbreak);
            result.Add(line);
        }

        public int GetLineIndex(int position)
        {
            int lower = 0;
            int upper = Lines.Length - 1;
            while (lower <= upper)
            {
                var index = lower + (upper - lower) / 2;
                var start = Lines[index].Start;
                if (position == start)
                {
                    return index;
                }

                if (start > position)
                {
                    upper = index - 1;
                }
                else
                {
                    lower = index + 1;
                }
            }
            return lower - 1;
        }

        /// <summary>
        /// Convert the text into sourcetext. sourcetext allows line number computation
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static SourceText From(string text)
        {
            return new(text);
        }

        public char this[int index] => Text[index];
        public int Length => Text.Length;
        public bool Contains(char i) => Text.Contains(i); 

        public override string ToString() => Text;
        public string ToString(int start, int length) => Text.Substring(start, length);
        public string ToString(TextSpan span) => ToString(span.Start, span.Length);
    }
}
