using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using MadLad.Compiler.CodeAnalysis.Binding;
using MadLad.Compiler.CodeAnalysis.ErrorReporting;
using MadLad.Compiler.CodeAnalysis.Syntax;

namespace MadLad.Compiler.CodeAnalysis.Evaluator
{
    public class Compilation
    {
        readonly Compilation Previous;
        private readonly SyntaxTree SyntaxTree;
        private BoundGlobalScope _GlobalScope;

        public Compilation(SyntaxTree syntaxtree) : this(null, syntaxtree) {}

        private Compilation(Compilation previous, SyntaxTree syntaxTree)
        {
            Previous = previous;
            SyntaxTree = syntaxTree;
        }

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

        public Compilation Continue(SyntaxTree tree)
        {
            return new Compilation(this, tree);
        }

        public EvaluationResult Evaluate(Dictionary<Variable, object> variables)
        {
            // Get the expression
            var globalScope = GlobalScope;

            // Get the errors from the syntax tree and from binding
            var Errors = SyntaxTree.Errors.Concat(globalScope.Errors).ToImmutableArray();
            
            // Evaluate the expression
            var evaluator = new Evaluator(globalScope.Statement, variables);
            var value = evaluator.Evaluate();

            if (Errors.Any())
            {
                return new EvaluationResult(Errors, null);
            }

            // Return the value or errors if there are any
            return new EvaluationResult(ImmutableArray<Error>.Empty, value);
        }
    }
}