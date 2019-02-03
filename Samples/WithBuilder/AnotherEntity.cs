using System;
using System.Collections.Generic;
using System.Text;

namespace Samples.WithBuilder
{
    interface IAnotherEntity
    {
        string Name { get; }
    }

    class AnotherEntity : IAnotherEntity
    {
        public string Name { get; set; }
    }
}
