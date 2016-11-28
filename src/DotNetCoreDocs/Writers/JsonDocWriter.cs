using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DotNetCoreDocs.Configuration;
using DotNetCoreDocs.Models;
using Newtonsoft.Json;

namespace DotNetCoreDocs.Writers
{
    public class JsonDocWriter : IWriter
    {
        private readonly DocsConfiguration _config;
        private string _requestFilePath;
        private string _requestBody;
        
        public JsonDocWriter(DocsConfiguration config)
        {
            _config = config;
        }

        public async Task WriteRequestAsync(string modelName, string filePath, string description, HttpRequestMessage request, HttpResponseMessage response)
        {
            _requestFilePath = filePath;
            var testRequest = await GetTestRequestAsync(description, request, response);
            var newRequestsFileContents = AddRequestToFile(modelName, testRequest);
            File.WriteAllLines(_requestFilePath, new List<string> { newRequestsFileContents });
        }

        private async Task<TestRequest> GetTestRequestAsync(string description, HttpRequestMessage request, HttpResponseMessage response)
        {
            var testRequest = new TestRequest(description, request, response);
            testRequest.Body = _requestBody ?? string.Empty;
            _requestBody = string.Empty;

            if(response.Content != null)
                testRequest.Response.Body = await response.Content.ReadAsStringAsync();

            // reset the content since it will now be disposed and may be needed by the tests
            request.Content = new StringContent(testRequest.Body);
            response.Content = new StringContent(testRequest.Response.Body);
            return testRequest;
        }

        public async Task LoadRequestBodyAsync(HttpRequestMessage request)
        {
            if(request.Content != null)
                _requestBody = await request.Content.ReadAsStringAsync();
        }

        private string AddRequestToFile(string modelName, TestRequest request)
        {
            var savedRequests = GetTestRequestsFromFile(modelName);
            savedRequests.TestRequests.Add(request);
            savedRequests.TestRequests.OrderBy(x => x.Method);
            return JsonConvert.SerializeObject(savedRequests, Formatting.Indented);
        }

        private RequestsDocument GetTestRequestsFromFile(string modelName)
        {
            if(!Directory.Exists(_config.RequestsDirectory))
                Directory.CreateDirectory(_config.RequestsDirectory);
            if(!File.Exists(_requestFilePath))
                return new RequestsDocument {
                    ModelName = modelName,
                    TestRequests = new List<TestRequest>()
                };

            var requestsJson = File.ReadAllText(_requestFilePath);

            return JsonConvert.DeserializeObject<RequestsDocument>(requestsJson);               
        }
    }
}
