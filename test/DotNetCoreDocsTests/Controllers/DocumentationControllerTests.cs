using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DotNetCoreDocs.Configuration;
using DotNetCoreDocs.Controllers;
using Xunit;

namespace DotNetCoreDocsTests.Controllers
{
    public class DocumentationControllerTests : DocumentationController
    {
        private const string _requestsDirectory = "./temp";
        private const string _rootTemplatePath = "./root.hbs";
        private const string _homeTemplatePath = "./home.hbs";
        private const string _modelTemplatePath = "./model.hbs";

        public DocumentationControllerTests() 
        : base(new DocsConfiguration{
            RequestsDirectory = _requestsDirectory,
            RootTemplatePath = _rootTemplatePath,
            HomeTemplatePath = _homeTemplatePath,
            ModelTemplatePath = _modelTemplatePath
        })
        { }

        [Fact]
        public void GetModelNames_GetsNamesFromFiles()
        {
            // arrange
            var filePath = _requestsDirectory + "/MyModel.json";
            Directory.CreateDirectory(_requestsDirectory);
            
            var stream = File.Create(filePath);
            stream.Dispose();

            // act
            var modelNames = GetModelNames();

            // assert
            var data = (modelNames as IEnumerable<dynamic>).First();
            Assert.Equal("MyModel", data.GetType().GetProperty("Name").GetValue(data, null));

            // dispose
            File.Delete(filePath);
            Directory.Delete(_requestsDirectory);
        }

        [Fact]
        public void GetRootTemplate_ReturnsRootTemplateContent_IfSpecified()
        {
            // arrange
            var content = Guid.NewGuid().ToString();
            File.WriteAllText(_rootTemplatePath, content);

            // act
            var result = GetRootTemplate();

            // assert
            Assert.Equal(content, result);

            // dispose
            File.Delete(_rootTemplatePath);
        }

        [Fact]
        public void GetHomeTemplate_ReturnsHomeTemplateContent_IfSpecified()
        {
            // arrange
            var content = Guid.NewGuid().ToString();
            File.WriteAllText(_homeTemplatePath, content);

            // act
            var result = GetHomeTemplate();

            // assert
            Assert.Equal(content, result);

            // dispose
            File.Delete(_homeTemplatePath);
        }

        [Fact]
        public void GetModelTemplate_ReturnsModelTemplateContent_IfSpecified()
        {
            // arrange
            var content = Guid.NewGuid().ToString();
            File.WriteAllText(_modelTemplatePath, content);

            // act
            var result = GetModelTemplate();

            // assert
            Assert.Equal(content, result);

            // dispose
            File.Delete(_modelTemplatePath);
        }
    }
}
