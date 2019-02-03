using System;
using System.Collections.Generic;
using System.Text;

namespace Samples.Constructor
{
    class Bicycle
    {
        public Bicycle(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; }
        public string Name { get; }
    }
}
