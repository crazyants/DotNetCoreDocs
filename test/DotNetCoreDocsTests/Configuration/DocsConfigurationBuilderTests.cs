using System;
using DotNetCoreDocs.Configuration;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace DotNetCoreDocsTests
{
    public class DocsConfigurationBuilderTests
    {
        [Theory]
        [InlineData("DocsConfiguration:RequestsDirectory", "")]
        [InlineData("DocsConfiguration:RequestsDirectory", null)]
        public void GetConfiguration_ThrowsException_IfRequiredParametersNotSpecified(string parameter, string returnValue)
        {
            // arrange
            var configMock = new Mock<IConfiguration>();
            configMock.Setup(c=> c[parameter]).Returns(returnValue);

            // act
            // assert
            Assert.Throws<ArgumentException>(() => 
                DocsConfigurationBuilder.GetConfiguration(configMock.Object)
            );
        }

        [Fact]
        public void GetConfiguration_UsesDefaultValues_IfNotProvided()
        {
            // arrange
            var configMock = GetConfigMockWithRequiredParameters();
            configMock.Setup(c=> c["DocsConfiguration:BaseAddress"]).Returns("");
            configMock.Setup(c=> c["DocsConfiguration:DocumentationRoute"]).Returns("");
            configMock.Setup(c=> c["DocsConfiguration:ReadmePath"]).Returns("");

            // act
            var result = DocsConfigurationBuilder.GetConfiguration(configMock.Object);

            // assert
            Assert.Equal("http://localhost:5000", result.BaseAddress);
            Assert.Equal("/docs", result.DocumentationRoute);
            Assert.Equal("README.md", result.ReadmePath);
            Assert.Equal("API Documentation", result.DisplayName);
        }

        private Mock<IConfiguration> GetConfigMockWithRequiredParameters()
        {
            var configMock = new Mock<IConfiguration>();
            configMock.Setup(c=> c["DocsConfiguration:RequestsDirectory"]).Returns("some-value");
            return configMock;
        }
    }
}
