using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http.Json;

namespace API_Whisperer
{
    public class Request
    {
        #region Fields

        private static Random rnd = new Random();
        public static string urlStart = "https://api-eu1.compleat.online/";
        public object content = null;
        public Dictionary<string, string> headers = new Dictionary<string, string>();
        public string method = "GET", url = "https://www.google.com";

        #endregion Fields

        #region Methods

        public async Task<Response> Execute(Authentication auth = null, bool throwError = true)
        {
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
                    int retry_delay_maginification = 1;

                    string error = "";

                    while (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests || error == "429")
                    {
                        if (retry_delay_maginification < 120)
                        {
                            Console.WriteLine($"Request {JsonSerializer.SerializeToElement(this).GetRawText()}\nCouldnt complete request!");
                            break;
                        }

                        Thread.Sleep((rnd.Next(800, 1200) * retry_delay_maginification));
                        response = await httpClient.SendAsync(request);
                        body = await response.Content.ReadAsStringAsync();

                        retry_delay_maginification += retry_delay_maginification;

                        int error_start = body.IndexOf("error");
                        error = error_start != -1 ? body.Substring(error_start + 2, 3) : "";
                    }

                    if (throwError && !response.IsSuccessStatusCode)
                    {
                        throw new Exception($"Request {JsonSerializer.SerializeToElement(this).GetRawText()}\nThrew An Exception: {((int)response.StatusCode)}\n{body}!");
                    }
                    else
                    {
                        return new Response(this, ((int)response.StatusCode), response.IsSuccessStatusCode, body);
                    }
                }
            }
        }

        #endregion Methods
    }
}