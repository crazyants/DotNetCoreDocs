using Microsoft.AspNetCore.Builder;

namespace DotNetCoreDocs.Middleware
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseDocs(this IApplicationBuilder builder, string route)
        {
            return builder.UseMiddleware<DocumentationMiddleware>(route);
        }
    }
}
