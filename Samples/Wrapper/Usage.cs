using ImmutableNet;
using Samples.Why;
using System;
using System.Collections.Generic;
using System.Text;

namespace Samples.Wrapper
{
    class Usage
    {
        void DoSomething(Immutable<Entity> entity)
        {
            string name = entity.Get(e => e.Name);
            Immutable<Entity> renamed = entity
                .Modify(e => e.Name = "new name");
        }
    }
}
