using System;
using System.Threading.Tasks;
using DotNetCoreDocs.Configuration;
using DotNetCoreDocs.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DotNetCoreDocs.Middleware
{
    public class DocumentationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly DocsConfiguration _configuration;
        private readonly ILogger _logger;

        public DocumentationMiddleware(RequestDelegate next, DocsConfiguration configuration, ILoggerFactory loggerFactory)
        {
            _next = next;
            _configuration = configuration;
            _logger = loggerFactory.CreateLogger<DocumentationMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            var modelName = GetModelNameFromRoute(context.Request.Path);
            if(modelName != null)
            {
                if(modelName == string.Empty)                
                    await context.Response.WriteAsync(HandleRootRoute());
                else
                    await context.Response.WriteAsync(HandleModelRoute(modelName));
            }
            else
            {
                await _next.Invoke(context);
            }
        }

        // Required route format /{DocumentationRoute}/{modelName}/
        private string GetModelNameFromRoute(PathString route)
        {
            PathString modelPath;
            if(route.StartsWithSegments(
                new PathString(_configuration.DocumentationRoute), 
                StringComparison.OrdinalIgnoreCase, 
                out modelPath))
            {
                return modelPath.Value.Replace("/", string.Empty);
            }
            return null;
        }

        private string HandleRootRoute()
        {
            _logger.LogInformation($"Documentation Route Activated for Root route");
            
            var controller = new RootController(_configuration);
            return controller.GetMarkup();
        }

        private string HandleModelRoute(string modelName)
        {
            _logger.LogInformation($"Documentation Route Activated for Model {modelName}");
            
            var controller = new ModelController(_configuration);
            return controller.GetMarkup(modelName);
        }
    }
}
