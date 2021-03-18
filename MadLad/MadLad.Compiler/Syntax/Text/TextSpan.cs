namespace MadLad.MadLad.Compiler.Syntax.Text
{
    public class TextSpan
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