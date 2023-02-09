using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Whisperer
{
    public class Authentication
    {
        #region Fields

        public Dictionary<string, string> headers = new Dictionary<string, string>();

        public ConcurrentDictionary<DateTime, string> requestTimes = new ConcurrentDictionary<DateTime, string>();

        #endregion Fields
    }
}