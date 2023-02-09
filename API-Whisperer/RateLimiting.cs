using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Whisperer
{
    public static class RateLimiting
    {
        #region Fields

        private static int maxReqInMinute = 50;
        private static int secWaitOnTooMany = 15;

        #endregion Fields

        #region Methods

        private static bool TooManyRequests(Authentication cookie)
        {
            var now = DateTime.Now;
            return cookie.requestTimes.Count(x => (now - x.Key).TotalSeconds <= 60) >= maxReqInMinute;
        }

        private static void Trim(Authentication cookie)
        {
            var now = DateTime.Now;
            var outdated = cookie.requestTimes.Where(x => (now - x.Key).TotalSeconds > 60);

            foreach (var ee in outdated)
            {
                cookie.requestTimes.TryRemove(ee);
            }
        }

        public static void HoldForRequestGrant(Authentication cookie)
        {
            while (TooManyRequests(cookie))
            {
                Console.WriteLine("IC Requests Held @ " + DateTime.Now.ToString());
                Thread.Sleep(secWaitOnTooMany * 1000);
            }
            cookie.requestTimes.TryAdd(DateTime.Now, "");
            Trim(cookie);
        }

        #endregion Methods
    }
}
