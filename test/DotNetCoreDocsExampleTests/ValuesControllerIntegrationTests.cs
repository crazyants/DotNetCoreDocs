using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DotNetCoreDocs;
using DotNetCoreDocs.Writers;
using DotNetCoreDocsExample;
using Xunit;

namespace DotNetCoreDocsExampleTests
{
    // see example explanation on xUnit.net website:
    // https://xunit.github.io/docs/getting-started-dotnet-core.html
    public class ValuesControllerIntegrationTests : IClassFixture<DocsFixture<Startup, JsonWriter>>
    {
        private DocsFixture<Startup, JsonWriter> _fixture;

        public ValuesControllerIntegrationTests(DocsFixture<Startup, JsonWriter> fixture)
        { 
            _fixture = fixture;
        }

        public HttpClient Client { get; }

        [Theory]
        [InlineData("GET", "/api/values")]
        public async Task AllMethods_ReturnSomething(string method, string route)
        {
            // Arrange
            var httpMethod = new HttpMethod(method);

            // Act
            var response = await _fixture.MakeRequest(httpMethod, route);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
        }
    }
}
