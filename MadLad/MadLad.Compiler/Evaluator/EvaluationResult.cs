using System.Collections.Generic;
using System.Linq;
using MadLad.MadLad.Compiler.ErrorReporting;

namespace MadLad.MadLad.Compiler.Evaluator
{
    public class EvaluationResult
    {
        public readonly IEnumerable<Error> Errors;
        public readonly object Value;

        public EvaluationResult(IEnumerable<Error> errors, object value)
        {
            Errors = errors.ToArray();
            Value = value;
        }
    }
}