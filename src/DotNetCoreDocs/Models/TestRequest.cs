using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DotNetCoreDocs.Models
{
    public class TestRequest
    {
        public TestResponse Response { get; set; }
        public string Uri { get; set; }
        public string Method { get; set; }
        public string Body { get; set; }
        private HttpRequestMessage _request;

        public TestRequest()
        { }

        public TestRequest(HttpRequestMessage request, HttpResponseMessage response)
        {
            _request = request;
            Uri = request.RequestUri.ToString();
            Method = request.Method.Method;
            Response = new TestResponse(response);
        }

        public async Task ReadRequestBody()
        {
            if(_request.Content != null)
                Body = await _request.Content?.ReadAsStringAsync();
            else
                return;
        }

        public async Task<string> GetJsonString()
        {
            await ReadRequestBody();
            await Response.ReadBody();
            return JsonConvert.SerializeObject(this);
        }
    }
}
