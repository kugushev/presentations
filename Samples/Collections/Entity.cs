using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Samples.Collections
{
    class Entity
    {
        // ...
        public IReadOnlyList<int> Items { get; private set; }

        public Entity AddItems(int item)
        {
            return new Entity
            {
                Items = Items.Append(item).ToList()
            };
        }
    }
}
