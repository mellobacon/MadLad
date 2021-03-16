using System.Collections;
using System.Collections.Generic;

namespace MadLad.MadLad.Compiler.ErrorReporting
{
    // a list of errors to call when theres an error
    public class ErrorList : IEnumerable<Error>
    {
        readonly List<Error> Errors = new();
        
        public IEnumerator<Error> GetEnumerator() => Errors.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        
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
    }
}