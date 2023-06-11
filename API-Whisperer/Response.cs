using System.Net.Http.Headers;
using System.Text.Json;

namespace API_Whisperer
{
    public class Response
    {
        #region Fields

        public string body;
        public JsonElement? bodyAsJson;
        public HttpResponseHeaders headers;
        public bool isSuccess;
        public Request request;
        public int statusCode;

        #endregion Fields

        #region Constructors

        public Response(Request request, int statusCode, bool isSuccess, string body, HttpResponseHeaders headers)
        {
            this.request = request;
            this.body = body;
            this.isSuccess = isSuccess;
            this.statusCode = statusCode;
            this.headers = headers;

            try
            {
                bodyAsJson = JsonSerializer.Deserialize<JsonElement>(body);
            }
            catch
            {
                //Failed To Convert To Json. This isnt necessarily an error
            }
        }

        #endregion Constructors
    }
}