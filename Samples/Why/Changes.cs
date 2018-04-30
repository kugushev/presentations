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

            return LoadResult(name);
        }

        string GetEntityName(Entity entity)
        {
            /* a lot of code */

            entity.Name = $"Name: {entity.Name}";

            /* a lot of code */

            return entity.Name;
        }

        Result LoadResult(string name) { return new Result(); }
    }

    public class Entity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    class Result { }
}
