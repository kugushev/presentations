using System;
using System.Collections.Generic;
using System.Text;

namespace Samples.Builder
{
    class Enity
    {
        public Enity(Builder builder)
        {
            Id = builder.Id;
            Name = builder.Name;
        }

        public int Id { get; }
        public string Name { get; }

        public class Builder
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
