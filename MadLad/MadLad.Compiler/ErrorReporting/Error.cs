using MadLad.MadLad.Compiler.Syntax.Text;

namespace MadLad.MadLad.Compiler.ErrorReporting
{
    public class Error
    {
        public readonly TextSpan Span;
        public readonly string Details;

        // Defines what makes up an error message
        public Error(TextSpan span, string details)
        {
            Span = span;
            Details = details;
        }

        public override string ToString()
        {
            return Details;
        }
    }
}