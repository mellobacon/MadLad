namespace MadLad.Compiler.CodeAnalysis.Syntax.Text
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
        
        public static TextSpan FromBounds(int start, int end)
        {
            var length = end - start;
            return new TextSpan(start, length);
        }
    }
}