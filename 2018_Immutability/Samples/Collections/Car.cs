using Demo.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Samples.Collections
{
    class Car
    {
        // ...
        public IReadOnlyList<IOwner> Owners { get; private set; }

        public Car AddItems(IOwner owner)
        {
            return new Car
            {
                Owners = Owners.Append(owner).ToList()
            };
        }
    }
}
