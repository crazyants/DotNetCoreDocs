using System.Collections.Generic;
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
        public string Description { get; set; }
        public string ContentType { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public bool HasBody {
            get {
                return !string.IsNullOrEmpty(Body);
            }
        }
        private HttpRequestMessage _request;

        public TestRequest()
        { }

        public TestRequest(string description, HttpRequestMessage request, HttpResponseMessage response)
        {
            _request = request;
            Description = description;
            Uri = request.RequestUri.ToString();
            Method = request.Method.Method;
            ContentType = request.Content?.Headers?.ContentType?.MediaType;
            Response = new TestResponse(response);
            Headers = GetHeaders(request);
        }

        public async Task ReadRequestBody()
        {
            if(_request.Content != null)
                Body = await _request.Content?.ReadAsStringAsync();
            else
                return;
        }

        public async Task<TestRequest> LoadRequestBody()
        {
            await ReadRequestBody();
            await Response.ReadBody();
            return this;
        }

        private Dictionary<string, string> GetHeaders(HttpRequestMessage request)
        {
            var headers = new Dictionary<string, string>();
            if (request.Headers != null)
            {
                foreach (var header in request?.Headers)
                {
                    headers.Add(header.Key, JsonConvert.SerializeObject(header.Value));
                }
            }

            return headers;
        }
    }
}
