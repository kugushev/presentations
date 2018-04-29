using System;
using System.Collections.Generic;
using System.Text;

namespace Samples.Constructor
{
    class Enity
    {
        public Enity(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; }
        public string Name { get; }

        public Enity WithId(int id)
        {

        }
    }
}
