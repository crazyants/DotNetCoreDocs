using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DotNetCoreDocs.Models
{
    public class TestResponse
    {
        private HttpResponseMessage _response;
        public string Body { get; set; }
        public string ReasonPhrase { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public TestResponse()
        {
            
        }
        
        public TestResponse(HttpResponseMessage response)
        {
            _response = response;
            Headers = GetHeaders(response);
            StatusCode = response.StatusCode;
            ReasonPhrase = response.ReasonPhrase;
        }

        public async Task ReadBody()
        {
            Body = await _response.Content.ReadAsStringAsync();
        }

        private Dictionary<string, string> GetHeaders(HttpResponseMessage response)
        {
            var headers = new Dictionary<string, string>();
            if (response.Headers != null)
            {
                foreach (var header in response?.Headers)
                {
                    headers.Add(header.Key, header.Value.ToString());
                }
            }

            return headers;
        }
    }
}
