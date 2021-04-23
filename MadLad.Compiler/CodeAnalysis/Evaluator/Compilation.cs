using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using MadLad.Compiler.CodeAnalysis.Binding;
using MadLad.Compiler.CodeAnalysis.ErrorReporting;
using MadLad.Compiler.CodeAnalysis.Syntax;

namespace MadLad.Compiler.CodeAnalysis.Evaluator
{
    // gather all the inputs (or source files later on) and compile them
    public sealed class Compilation
    {
        readonly Compilation Previous;
        private readonly SyntaxTree SyntaxTree;
        private BoundGlobalScope _GlobalScope;

        // set the constructor without exposing the previous compilation
        public Compilation(SyntaxTree syntaxtree) : this(null, syntaxtree) {}

        private Compilation(Compilation previous, SyntaxTree syntaxTree)
        {
            Previous = previous;
            SyntaxTree = syntaxTree;
        }

        // gets the global scope
        private BoundGlobalScope GlobalScope
        {
            get
            {
                if (_GlobalScope == null)
                {
                    var _globalScope = Binder.BindGlobalScope(Previous?.GlobalScope, SyntaxTree.Root);
                    
                    // set the scope to whatever thing is first
                    Interlocked.CompareExchange(ref _GlobalScope, _globalScope, null);
                }

                return _GlobalScope;
            }
        }

        // this is used to set any previous compilation
        public Compilation Continue(SyntaxTree tree)
        {
            return new (this, tree);
        }

        public EvaluationResult Evaluate(Dictionary<Variable, object> variables)
        {
            // Get the expression
            var globalScope = GlobalScope;

            // Get the errors from the syntax tree and from binding
            var Errors = SyntaxTree.Errors.Concat(globalScope.Errors).ToImmutableArray();
            
            // Evaluate the code
            var evaluator = new Evaluator(globalScope.Statement, variables);
            var value = evaluator.Evaluate();

            if (Errors.Any())
            {
                // return errors
                return new EvaluationResult(Errors, null);
            }

            // Return the value
            return new EvaluationResult(ImmutableArray<Error>.Empty, value);
        }
    }
}