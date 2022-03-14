using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace API_Whisperer
{
    public class Response
    {
        #region Fields

        public string body;
        public JsonElement? bodyAsJson;
        public bool isSuccess;
        public Request request;
        public int statusCode;

        #endregion Fields

        #region Constructors

        public Response(Request request, int statusCode, bool isSuccess, string body)
        {
            this.request = request;
            this.body = body;
            this.isSuccess = isSuccess;
            this.statusCode = statusCode;

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