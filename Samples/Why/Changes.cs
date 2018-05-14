using System;
using System.Collections.Generic;
using System.Text;

namespace Samples.Why
{
    class Changes
    {
        public Result GetResult(Entity entity)
        {
            /* a lot of code */

            var name = GetEntityName(entity);

            /* a lot of code */

            return LoadResult(name, name == entity.Name);
        }

        public string GetEntityName(Entity entity)
        {
            string name = entity.Name;
            /* a lot of code */

            // Bugfix 666
            entity.Name = $"Name: {entity.Name}";

            /* a lot of code */

            return name;
        }

        Result LoadResult(string name, bool isEqual) { return new Result(); }
    }

    public class Entity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    class Result { }
}
