using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLocation;

namespace Example
{
    class Victim
    {
        public int FeedMonster()
        {
            var cookies = ServiceLocator.Resolve<CookieService>();
            ServiceLocator.Resolve<CookieMonster>().Notify(cookies.Kind);
            cookies.Consuming += count => ServiceLocator.Resolve<CookiesWarehouse>().Send(count);
            cookies.Feed(ServiceLocator.Resolve<CookieMonster>());
            return ServiceLocator.Resolve<CookiesWarehouse>().Left;
        }
    }
}
