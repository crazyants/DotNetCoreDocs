using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using DotNetCoreDocs.Models;

namespace DotNetCoreDocs.Writers
{
    public class JsonWriter : IWriter
    {
        private const string _writeDir = "_data";
        private const string _writeFile = "requests.json"; 
        
        public async Task WriteRequestAsync(HttpRequestMessage request, HttpResponseMessage response)
        {
            var testRequest = new TestRequest(request, response);
            var str = await testRequest.GetJsonString();
            Directory.CreateDirectory(_writeDir);
            File.AppendAllLines(BuildFilePath(), new List<string> { str });
        }

        private string BuildFilePath()
        {
            return $"{_writeDir}/{_writeFile}";
        }
    }
}
