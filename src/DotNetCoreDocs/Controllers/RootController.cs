using DotNetCoreDocs.Configuration;
using HandlebarsDotNet;

namespace DotNetCoreDocs.Controllers
{
    public class RootController : DocumentationController
    {
        private readonly DocsConfiguration _configuration;
        public RootController(DocsConfiguration configuration)
         : base(configuration)
        {
            _configuration = configuration;
        }


        public string GetMarkup()
        {
            var rootSource = GetRootTemplate();
            var homeSource = GetHomeTemplate();
            var readmeSource = GetReadmeTemplate();

            AddPartial("content", homeSource);
            AddPartial("readme", readmeSource);

            var template = Handlebars.Compile(rootSource);

            var data = new
            {
                Configuration = _configuration,
                Models = GetModelNames()
            };

            return template(data);
        }
    }
}
