namespace MadLad.Compiler.CodeAnalysis.Syntax.Text
{
    public class TextLine
    {
        private readonly SourceText Text;
        public readonly int Start;
        private readonly int Length;
        private readonly int LengthWithBreak;
        private int End => Start + Length;
        public TextSpan Span => new TextSpan(Start, Length);
        public TextSpan SpanWithBreak => new TextSpan(Start, LengthWithBreak);

        public TextLine(SourceText text, int start, int length, int lengthwithbreak)
        {
            Text = text;
            Start = start;
            Length = length;
            LengthWithBreak = lengthwithbreak;
        }
        
        public override string ToString() => Text.ToString(Start, LengthWithBreak);
        public string ToString(int start, int length) => Text.ToString(Span);
        public string ToString(TextSpan span) => ToString(span.Start, span.Length);
    }
}