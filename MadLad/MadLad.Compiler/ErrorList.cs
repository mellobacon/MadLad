using System.Collections;
using System.Collections.Generic;

namespace MadLad.Lexer
{
    public class ErrorList : IEnumerable<Error>
    {
        readonly List<Error> Errors = new();
        
        public IEnumerator<Error> GetEnumerator() => Errors.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        
        private void Report(string message)
        {
            var error = new Error(message);
            Errors.Add(error);
        }
        public void ReportBadCharacter(char character)
        {
            var message = $"Error: Illegal character: '{character}'";
            Report(message);
        }
    }
}