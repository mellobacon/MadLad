using System.Collections;
using System.Collections.Generic;
using MadLad.MadLad.Compiler.Syntax;
using MadLad.MadLad.Compiler.Syntax.Text;

namespace MadLad.MadLad.Compiler.ErrorReporting
{
    // a list of errors to call when theres an error
    public class ErrorList : IEnumerable<Error>
    {
        readonly List<Error> Errors = new();
        
        public IEnumerator<Error> GetEnumerator() => Errors.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        
        public void AddRange(ErrorList diagnostics)
        {
            Errors.AddRange(diagnostics.Errors);
        }
        
        private void Report(TextSpan span, string message)
        {
            var error = new Error(span, message);
            Errors.Add(error);
        }
        public void ReportBadCharacter(int position, char character)
        {
            var message = $"Error: Illegal character: '{character}'";
            var span = new TextSpan(position, 1);
            Report(span, message);
        }

        public void ReportUnexpectedToken(TextSpan span, SyntaxKind actual, SyntaxKind expected)
        {
            var message = $"Error: Unexpected token <{actual}>, expected <{expected}>.";
            Report(span, message);
            
        }
    }
}