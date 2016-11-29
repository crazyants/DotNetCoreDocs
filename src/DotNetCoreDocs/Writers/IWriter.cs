using System.Net.Http;
using System.Threading.Tasks;

namespace DotNetCoreDocs.Writers
{
    public interface IWriter
    {
        Task WriteRequestAsync(string modelName, string filePath, string description, HttpRequestMessage request, HttpResponseMessage response);
        Task LoadRequestBodyAsync(HttpRequestMessage request);
    }
}
