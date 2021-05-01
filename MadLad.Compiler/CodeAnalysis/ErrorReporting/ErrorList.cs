using System;
using System.Collections;
using System.Collections.Generic;
using MadLad.Compiler.CodeAnalysis.Syntax;
using MadLad.Compiler.CodeAnalysis.Syntax.Symbols;
using MadLad.Compiler.CodeAnalysis.Syntax.Text;

namespace MadLad.Compiler.CodeAnalysis.ErrorReporting
{
    // a list of errors to call when theres an error
    public sealed class ErrorList : IEnumerable<Error>
    {
        private readonly List<Error> Errors = new();
        
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
            var message = $"Error: Unexpected token <{actual}>, expected <{expected}>";
            Report(span, message);
        }

        public void ReportInvalidNumber(TextSpan span, string text, TypeSymbol type)
        {
            var message = $"The number {text} is not a valid {type}";
            Report(span, message);
        }

        public void ReportUndefinedBinaryOperator(TextSpan span, string op, TypeSymbol left, TypeSymbol right)
        {
            var message = $"Binary operator '{op}' is not defined for the types {left} and {right}";
            Report(span, message);
        }

        public void ReportUndefinedUnaryOperator(TextSpan span, string op, TypeSymbol operand)
        {
            var message = $"Unary operator '{op}' is not defined for the type {operand}";
            Report(span, message);
        }
        
        public void ReportUndefinedName(TextSpan span, string name)
        {
            var message = $"Variable '{name}' does not exist";
            Report(span, message);
        }

        public void ReportVariableAlreadyExists(TextSpan span, string name)
        {
            var message = $"Variable '{name}' is already declared";
            Report(span, message);
        }

        public void ReportCannotConvertType(TypeSymbol from, TypeSymbol to)
        {
            var message = $"Cannot convert type '{from}' to type '{to}'";
            Report(new TextSpan(), message);
        }
    }
}