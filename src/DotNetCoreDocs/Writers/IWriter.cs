using System.Net.Http;
using System.Threading.Tasks;

namespace DotNetCoreDocs.Writers
{
    public interface IWriter
    {
        Task WriteRequestAsync(HttpRequestMessage request, HttpResponseMessage response);
    }
}
