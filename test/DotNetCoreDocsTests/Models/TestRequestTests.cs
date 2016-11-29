using System.Net.Http;
using System.Threading.Tasks;
using DotNetCoreDocs.Models;
using Newtonsoft.Json;
using Xunit;

namespace DotNetCoreDocsTests.Models
{
    public class TestRequestTests
    {
        [Fact]
        public void Constructor_CreatesRequest_FromMessages()
        {
            // arrange
            var description = "This is a request description";
            var route = "/api/entities";
            var method = "GET";
            
            var httpRequestMessage = new HttpRequestMessage(new HttpMethod(method), route);

            var requestContent = "request";
            httpRequestMessage.Content = new StringContent(requestContent);

            var contentType = "application/text";
            httpRequestMessage.Content.Headers.ContentType.MediaType = contentType;

            var testHeaderKey = "X-MyHeader";
            var testHeaderValue = new string[] { "MyHeaderValue" };
            httpRequestMessage.Headers.Add(testHeaderKey, testHeaderValue);
            var testHeaderJson = JsonConvert.SerializeObject(testHeaderValue);

            var httpResponseMessage = new HttpResponseMessage();

            // act
            var request = new TestRequest(description, httpRequestMessage, httpResponseMessage);

            // assert
            Assert.Equal(description, request.Description);
            Assert.Equal(route, request.Uri);
            Assert.Equal(method, request.Method);
            Assert.Equal(contentType, request.ContentType);
            Assert.Equal(testHeaderJson, request.Headers[testHeaderKey]);
            Assert.NotNull(request.Response);
        }

        [Fact]
        public async Task LoadRequestBody_LoadsRequestAndResponseBodies()
        {
            // arrange
            var route = "/api/entities";
            var method = "GET";
            var httpRequestMessage = new HttpRequestMessage(new HttpMethod(method), route);

            var requestContent = "request";
            httpRequestMessage.Content = new StringContent(requestContent);

            var httpResponseMessage = new HttpResponseMessage();

            var responseContent = "request";
            httpResponseMessage.Content = new StringContent(responseContent);

            var request = new TestRequest(null, httpRequestMessage, httpResponseMessage);

            // act
            await request.LoadRequestBody();

            // assert
            Assert.Equal(requestContent, request.Body);
            Assert.Equal(responseContent, request.Response.Body);
        }
    }
}
