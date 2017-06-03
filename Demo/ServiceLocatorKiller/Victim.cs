using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLocation;

namespace ServiceLocatorKiller
{
    class Victim
    {
        //public Victim()
        //{
        //}

        public int FeedMonster()
        {
            var cookies = ServiceLocator.Resolve<ICookieService>();
            ServiceLocator.Resolve<ICookieMonster>().Notify(cookies.Kind);
            cookies.Consuming += count => ServiceLocator.Resolve<ICookiesWarehouse>().Send(count);
            cookies.Feed(ServiceLocator.Resolve<ICookieMonster>());
            return ServiceLocator.Resolve<ICookiesWarehouse>().Left;
        }
    }
}
