using MadLad.Compiler.CodeAnalysis.Syntax.Text;

namespace MadLad.Compiler.CodeAnalysis.ErrorReporting
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