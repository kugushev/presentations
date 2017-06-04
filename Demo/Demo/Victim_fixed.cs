using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLocatorKiller
{
    class Victim
    {
        public Victim(CookieService cookieService, CookieMonster cookieMonster, CookiesWarehouse cookiesWarehouse)
        {
            _cookieService = cookieService;
            _cookieMonster = cookieMonster;
            _cookiesWarehouse = cookiesWarehouse;
        }

        CookiesWarehouse _cookiesWarehouse;
        CookieMonster _cookieMonster;
        CookieService _cookieService;

        public int FeedMonster()
        {
            var cookies = _cookieService;
            _cookieMonster.Notify(cookies.Kind);
            cookies.Consuming += count => _cookiesWarehouse.Send(count);
            cookies.Feed(_cookieMonster);
            return _cookiesWarehouse.Left;
        }
    }
}