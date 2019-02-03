using System;
using System.Collections.Generic;
using System.Text;

namespace Samples.ImmutableInterface
{
    public interface IBicycle
    {
        int Id { get; }

        string Name { get; }
    }

    public class Bicycle : IBicycle
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
