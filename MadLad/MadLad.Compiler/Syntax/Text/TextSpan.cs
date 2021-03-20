namespace MadLad.MadLad.Compiler.Syntax.Text
{
    // for finding the length of the text ig
    public readonly struct TextSpan
    {
        public readonly int Start;
        public readonly int Length;
        public int End => Start + Length;

        public TextSpan(int start, int length)
        {
            Start = start;
            Length = length;
        }
    }
}