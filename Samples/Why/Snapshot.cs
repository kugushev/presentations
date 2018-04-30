using System;
using System.Collections.Generic;
using System.Text;

namespace Samples.Why
{
    class Snapshot
    {
        public Entity Entity { get; }
        void DoSomething()
        {
            var entity = Entity;
            /* a lot of code */
            
        }
        /*---*/
        void UpdateEntityName(string name)
        {
            Entity.Name = name;
        }
    }
}
