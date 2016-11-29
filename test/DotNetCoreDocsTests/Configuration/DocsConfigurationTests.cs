using DotNetCoreDocs.Configuration;
using Xunit;

namespace DotNetCoreDocsTests
{
    public class DocsConfigurationTests
    {
        [Theory]
        [InlineData("/src/myApp","myModel", "/src/myApp/myModel.json")]
        public void GetRequestsFileName_ReturnsJsonFilePath(string requestsDirectory, string modelName, string expectedResult)
        {
            // arrange
            var config = new DocsConfiguration {
                RequestsDirectory = requestsDirectory
            };

            // act
            var result = config.GetRequestsFileName(modelName);

            // assert
            Assert.Equal(expectedResult, result);
        }
    }
}
