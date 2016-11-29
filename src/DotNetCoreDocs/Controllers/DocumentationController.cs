using System.IO;
using System.Linq;
using System.Reflection;
using CommonMark;
using DotNetCoreDocs.Configuration;
using HandlebarsDotNet;

namespace DotNetCoreDocs.Controllers
{
    public class DocumentationController
    {
        private readonly DocsConfiguration _configuration;
        public DocumentationController(DocsConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected void AddPartial(string partialName, string source)
        {
            using (var reader = new StringReader(source))
            {
                var template = Handlebars.Compile(reader);
                Handlebars.RegisterTemplate(partialName, template);
            }
        }

        protected dynamic GetModelNames()
        {
            return Directory
                .GetFiles(_configuration.RequestsDirectory)
                .Select(filePath => 
                    new 
                    { 
                        Name = GetModelNameFromFilePath(filePath),
                        Uri = $"{_configuration.DocumentationRoute}/{GetModelNameFromFilePath(filePath)}"
                }).ToList();
        }

        private string GetModelNameFromFilePath(string path)
        {
            return path
                .Replace(_configuration.RequestsDirectory, string.Empty)
                .Replace("/", string.Empty)
                .Replace(".json", string.Empty);
        }

        protected string GetRootTemplate()
        {
            if (string.IsNullOrEmpty(_configuration.RootTemplatePath))
                return GetManifestContent("DotNetCoreDocs._templates.root.hbs");
            return GetFileContent(_configuration.RootTemplatePath);
        }

        protected string GetHomeTemplate()
        {
            if (string.IsNullOrEmpty(_configuration.HomeTemplatePath))
                return GetManifestContent("DotNetCoreDocs._templates.home.hbs");
            return GetFileContent(_configuration.HomeTemplatePath);
        }

        protected string GetModelTemplate()
        {
            if (string.IsNullOrEmpty(_configuration.ModelTemplatePath))
                return GetManifestContent("DotNetCoreDocs._templates.model.hbs");
            return GetFileContent(_configuration.ModelTemplatePath);
        }

        protected string GetReadmeTemplate()
        {
            return CommonMarkConverter.Convert(File.ReadAllText(_configuration.ReadmePath));
        }

        private string GetFileContent(string fileName)
        {
            return File.ReadAllText(fileName);
        }

        private string GetManifestContent(string fileName)
        {
            var assemblyName = new AssemblyName(nameof(DotNetCoreDocs));
            var resource = Assembly.Load(assemblyName).GetManifestResourceStream(fileName);

            using (var reader = new StreamReader(resource))
                return reader.ReadToEnd();
        }
    }
}
