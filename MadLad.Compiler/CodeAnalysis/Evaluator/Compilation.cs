using System;
using System.Collections.Generic;
using System.Linq;
using MadLad.Compiler.CodeAnalysis.Binding;
using MadLad.Compiler.CodeAnalysis.ErrorReporting;
using MadLad.Compiler.CodeAnalysis.Syntax;

namespace MadLad.Compiler.CodeAnalysis.Evaluator
{
    public class Compilation
    {
        private readonly SyntaxTree SyntaxTree;

        public Compilation(SyntaxTree syntaxtree)
        {
            SyntaxTree = syntaxtree;
        }

        public EvaluationResult Evaluate(Dictionary<Variable, object> variables)
        {
            // Get the expression
            var binder = new Binder(variables);
            var expression = binder.BindExpression(SyntaxTree.Root);
            
            // Get the errors from the syntax tree and from binding
            var Errors = SyntaxTree.Errors.Concat(binder.Errors);
            
            // Evaluate the expression
            var evaluator = new Evaluator(expression, variables);
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