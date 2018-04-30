using System;
using System.Collections.Generic;
using System.Text;

namespace Samples.Why
{
    class InMemoryCache
    {
        private static readonly Cache cache;
        void DoSomething()
        {
            /* a lot of code */

            var entity = cache.Cached(GetEntity);

            /* a lot of code */

            entity.Name = "Hello World!";
        }
        Entity GetEntity()
        { 
            /*---*/
            return null;
        }
    }

    class Cache
    {
        public T Cached<T>(Func<T> func)
        {
            return default(T);
        }
    }
}
