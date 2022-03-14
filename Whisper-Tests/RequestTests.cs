using Microsoft.VisualStudio.TestTools.UnitTesting;

using API_Whisperer;

namespace Whisper_Tests
{
    [TestClass]
    public class RequestTests
    {
        #region Methods

        [TestMethod]
        public void DefaultRequest()
        {
            Request request = new Request();

            var t_response = request.Execute();
            t_response.Wait();
            var response = t_response.Result;

            Assert.IsTrue(response.isSuccess);
            Assert.IsFalse(response.bodyAsJson.HasValue);
        }

        [TestMethod]
        public void PostRequest()
        {
            Request request = new Request();

            request.method = "POST";
            request.url = "https://reqbin.com/echo/post/json";
            request.content = new { name = "Test" };

            var t_response = request.Execute();
            t_response.Wait();
            var response = t_response.Result;

            Assert.IsTrue(response.isSuccess);
            Assert.IsTrue(response.bodyAsJson.HasValue);
        }

        #endregion Methods
    }
}