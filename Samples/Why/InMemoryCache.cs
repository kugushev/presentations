using System;
using System.Collections.Generic;
using System.Text;

namespace Samples.Why
{
    class InMemoryCache
    {
        private readonly Dictionary<int, Entity> cache = new Dictionary<int, Entity>();
        void DoSomething(int id)
        {
            /* a lot of code */

            if (!cache.TryGetValue(id, out var entity))
                entity = cache[id] = GetEntity(id);

            /* a lot of code */
            //Bugfix 1313
            entity.Name = "Hello World!";
        }
        Entity GetEntity(int id) { /*---*/ return null; }
    }

    class Cache
    {
        public T Cached<T>(Func<T> func)
        {
            return default(T);
        }
    }
}
