using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLocatorKiller
{
    class CookieService
    {
        internal event Func<object, object> Consuming;

        public int Kind { get; set; }

        internal void Feed(CookieMonster cookieMonster)
        {
            throw new NotImplementedException();
        }
    }

    class CookieMonster
    {
        internal void Notify(int kind)
        {
            throw new NotImplementedException();
        }
    }

    class CookiesWarehouse
    {
        public int Left { get; internal set; }

        internal object Send(object count)
        {
            throw new NotImplementedException();
        }
    }
}
