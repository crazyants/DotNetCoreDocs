using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetCoreDocs.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDocumentationConfiguration(this IServiceCollection services, IConfiguration config)
        {
            var docsConfiguration = DocsConfigurationBuilder.GetConfiguration(config);

            services.AddSingleton<DocsConfiguration>(docsConfiguration);
        }
    }
}
