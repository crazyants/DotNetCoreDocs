using System.IO;
using DotNetCoreDocs.Configuration;
using DotNetCoreDocs.Models;
using HandlebarsDotNet;
using Newtonsoft.Json;

namespace DotNetCoreDocs.Controllers
{
    public class ModelController : DocumentationController
    {
        private readonly DocsConfiguration _configuration;
        public ModelController(DocsConfiguration configuration)
        : base(configuration)
        {
            _configuration = configuration;
        }

        public string GetMarkup(string modelName)
        {
            var rootSource = GetRootTemplate();

            var modelSource = GetModelTemplate();

            var json = File.ReadAllText(_configuration.GetRequestsFileName(modelName));

            AddPartial("content", modelSource);

            var template = Handlebars.Compile(rootSource);

            var data = new {
                Configuration =  _configuration,
                Models = GetModelNames(),
                Model = JsonConvert.DeserializeObject<RequestsDocument>(json)
            };

            return template(data);
        }
        
    }
}
