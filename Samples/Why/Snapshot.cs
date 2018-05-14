using System;
using System.Collections.Generic;
using System.Text;

namespace Samples.Why
{
    class Snapshot
    {
        public Entity Entity { get; private set; }
        void DoSomething()
        {
            var entity = Entity;
            Console.WriteLine(Entity.Name);
            Console.WriteLine(entity.Name);
            /* a lot of code */
            Console.WriteLine(Entity.Name);
            Console.WriteLine(entity.Name);
        }
        /*---*/
        void UpdateEntityName(string name)
        {
            Entity.Name = name;
        }        
    }
}
