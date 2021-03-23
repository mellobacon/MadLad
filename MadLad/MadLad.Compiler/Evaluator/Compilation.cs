using System;
using System.Linq;
using MadLad.MadLad.Compiler.Binding;
using MadLad.MadLad.Compiler.ErrorReporting;
using MadLad.MadLad.Compiler.Syntax;

namespace MadLad.MadLad.Compiler.Evaluator
{
    public class Compilation
    {
        private readonly SyntaxTree SyntaxTree;

        public Compilation(SyntaxTree syntaxtree)
        {
            SyntaxTree = syntaxtree;
        }

        public EvaluationResult Evaluate()
        {
            // Get the expression
            var binder = new Binder();
            var expression = binder.BindExpression(SyntaxTree.Root);
            
            // Get the errors from the syntax tree and from binding
            var Errors = SyntaxTree.Errors.Concat(binder.Errors);
            
            // Evaluate the expression
            var evaluator = new Evaluator(expression);
            var value = evaluator.Evaluate();

            if (Errors.Any())
            {
                return new EvaluationResult(Errors, null);
            }

            // Return the value or errors if there are any
            return new EvaluationResult(Array.Empty<Error>(), value);
        }
    }
}