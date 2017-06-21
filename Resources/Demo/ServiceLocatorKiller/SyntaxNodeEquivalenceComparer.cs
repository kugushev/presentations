using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace ServiceLocatorKiller
{
    class SyntaxNodeEquivalenceComparer : IEqualityComparer<SyntaxNode>
    {
        public bool Equals(SyntaxNode x, SyntaxNode y) => x?.IsEquivalentTo(y) == true;

        public int GetHashCode(SyntaxNode obj) => 0;
    }
}
