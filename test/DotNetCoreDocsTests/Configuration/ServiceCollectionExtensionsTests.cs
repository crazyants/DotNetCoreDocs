using DotNetCoreDocs.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace DotNetCoreDocsTests
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddDocumentationConfiguration_AddsConfigurationToServiceCollection()
        {
            // arrange
            var configMock = new Mock<IConfiguration>();
            configMock.Setup(c=> c["DocsConfiguration:RequestsDirectory"]).Returns("some-value");
            var services = new ServiceCollection();

            // act
            services.AddDocumentationConfiguration(configMock.Object);

            // assert
            var service = services.BuildServiceProvider().GetService<DocsConfiguration>();
            Assert.Equal("some-value", service.RequestsDirectory);
        }
    }
}
