using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example
{
    class SyntaxNodeEquivalenceComparer : IEqualityComparer<SyntaxNode>
    {
        public bool Equals(SyntaxNode x, SyntaxNode y) => x?.IsEquivalentTo(y, false) == true;

        public int GetHashCode(SyntaxNode obj) => obj?.ToString().GetHashCode() ?? 0;
    }
}
