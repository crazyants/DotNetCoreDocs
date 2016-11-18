using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DotNetCoreDocs.Middleware
{
    public class DocumentationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _route;
        public DocumentationMiddleware(RequestDelegate next, string route)
        {
            _next = next;
            _route = route;
        }

        public async Task Invoke(HttpContext context)
        {
            if(context.Request.Path == _route)
            {
                await context.Response.WriteAsync(JsonParser.GetJson());
            }
            else
            {
                await _next.Invoke(context);
            }
        }
    }
}
