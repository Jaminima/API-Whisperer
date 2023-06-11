using System.Net.Http.Json;
using System.Text.Json;

namespace API_Whisperer
{
    public class Request
    {
        #region Fields

        private static Random rnd = new Random();

        #endregion Fields

        #region Methods

        private string FindStatusCodeInBody(string body)
        {
            string lookFor = "statusCode";
            int error_idx = body.IndexOf(lookFor);
            int error_start = error_idx + lookFor.Length;
            return error_idx != -1 ? body.Substring(error_start + 3, 3) : "";
        }

        #endregion Methods

        public static string urlStart = "https://api-eu1.compleat.online/";
        public object content = null;
        public Dictionary<string, string> headers = new Dictionary<string, string>();
        public string method = "GET", url = "https://www.google.com";

        public async Task<Response> Execute(Authentication auth = null, bool throwError = false, int retry_delay_maginification = 1)
        {
            if (auth != null)
            {
                RateLimiting.HoldForRequestGrant(auth);
            }

            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod(method), urlStart + url))
                {
                    var _headers = auth == null ? headers : headers.Union(auth.headers);

                    foreach (KeyValuePair<string, string> header in _headers)
                    {
                        request.Headers.Add(header.Key, header.Value);
                    }

                    if (content != null)
                    {
                        request.Content = JsonContent.Create(content);
                    }

                    var response = await httpClient.SendAsync(request);
                    string body = await response.Content.ReadAsStringAsync();

                    while (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests || FindStatusCodeInBody(body) == "429")
                    {
                        if (retry_delay_maginification > 120)
                        {
                            Console.WriteLine($"Request {this.url}\nCouldnt complete request!");
                            break;
                        }

                        Thread.Sleep((rnd.Next(800, 1200) * retry_delay_maginification));
                        return await Execute(auth, throwError, retry_delay_maginification + retry_delay_maginification);
                    }

                    if (throwError && !response.IsSuccessStatusCode)
                    {
                        throw new Exception($"Request {JsonSerializer.SerializeToElement(this).GetRawText()}\nThrew An Exception: {((int)response.StatusCode)}\n{body}!");
                    }
                    else
                    {
                        return new Response(this, ((int)response.StatusCode), response.IsSuccessStatusCode, body, response.Headers);
                    }
                }
            }
        }
    }
}