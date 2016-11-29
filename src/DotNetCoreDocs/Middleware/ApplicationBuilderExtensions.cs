using Microsoft.AspNetCore.Builder;

namespace DotNetCoreDocs.Middleware
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseDocs(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<DocumentationMiddleware>();
        }
    }
}
