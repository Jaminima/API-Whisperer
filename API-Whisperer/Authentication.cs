using System.Collections.Concurrent;

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