using System.Collections.Generic;
using System.Linq;
using MadLad.Compiler.CodeAnalysis.Syntax.Text;

namespace MadLad.Compiler.CodeAnalysis.Syntax
{
    public class SyntaxToken : SyntaxNode
    {
        public readonly string Text;
        public override SyntaxKind Kind { get; }
        public readonly object Value;
        public readonly int Position;
        public TextSpan Span => new(Position, Text.Length);
        
        /// <summary>
        /// Defines what a syntax token is ie what components make up a syntax token.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="kind"></param>
        /// <param name="value"></param>
        /// <param name="position"></param>
        public SyntaxToken(string text, SyntaxKind kind, object value, int position)
        {
            Text = text;
            Kind = kind;
            Value = value;
            Position = position;
        }
        
        public override IEnumerable<SyntaxNode> GetChildren()
        {
            return Enumerable.Empty<SyntaxNode>();
        }
    }
}