using System;
using System.Collections.Generic;
using System.Linq;
using MadLad.Compiler.CodeAnalysis.Syntax;
using Xunit;

namespace MadLad.Tests.CodeAnalysis.Syntax
{
    public class AssertingEnumerator : IDisposable
    {
        readonly IEnumerator<SyntaxNode> _Enumerator;
        private bool HasErrors;

        public AssertingEnumerator(SyntaxNode node)
        {
            _Enumerator = Flatten(node).GetEnumerator();
        }

        private bool MarkFailed()
        {
            HasErrors = true;
            return false;
        }

        static IEnumerable<SyntaxNode> Flatten(SyntaxNode node)
        {
            var stack = new Stack<SyntaxNode>();
            stack.Push(node);

            while (stack.Count > 0)
            {
                var n = stack.Pop();
                yield return n;
                foreach (var child in n.GetChildren().Reverse())
                {
                    stack.Push(child);   
                }
            }
        }

        public void AssertNode(SyntaxKind kind)
        {
            try
            {
                Assert.True(_Enumerator.MoveNext());
                if (_Enumerator.Current != null)
                {
                    Assert.Equal(kind, _Enumerator.Current.Kind);
                    Assert.IsNotType<SyntaxToken>(_Enumerator.Current);
                }
            }
            catch when (MarkFailed())
            {
                throw;
            }
        }
        
        public void AssertToken(SyntaxKind kind, string text)
        {
            try
            {
                Assert.True(_Enumerator.MoveNext());
                if (_Enumerator.Current != null)
                {
                    Assert.Equal(kind, _Enumerator.Current.Kind);
                    var token = Assert.IsType<SyntaxToken>(_Enumerator.Current);
                    Assert.Equal(text, token.Text);
                }
            }
            catch when (MarkFailed())
            {
                throw;
            }
        }

        public void Dispose()
        {
            if (!HasErrors)
            {
                Assert.False(_Enumerator.MoveNext());   
            }
            _Enumerator.Dispose();
        }
    }
}