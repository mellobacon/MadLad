using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace MadLad.Compiler.CodeAnalysis.Syntax.Symbols
{
    public sealed class SeperatedSyntaxList<T> : IEnumerable<T> where T: SyntaxNode
    {
        private readonly ImmutableArray<SyntaxNode> Separatorsandnodes;

        public SeperatedSyntaxList(ImmutableArray<SyntaxNode> separatorsandnodes)
        {
            Separatorsandnodes = separatorsandnodes;
        }

        public int Count => (Separatorsandnodes.Length + 1) / 2;

        private T this[int index] => (T) Separatorsandnodes[index * 2];

        public ImmutableArray<SyntaxNode> GetWithSeparators() => Separatorsandnodes;
        
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}