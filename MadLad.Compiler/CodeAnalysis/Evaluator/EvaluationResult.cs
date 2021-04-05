using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using MadLad.Compiler.CodeAnalysis.ErrorReporting;

namespace MadLad.Compiler.CodeAnalysis.Evaluator
{
    public class EvaluationResult
    {
        public readonly ImmutableArray<Error> Errors;
        public readonly object Value;

        public EvaluationResult(ImmutableArray<Error> errors, object value)
        {
            Errors = errors;
            Value = value;
        }
    }
}