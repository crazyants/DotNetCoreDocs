using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using DotNetCoreDocs.Configuration;
using DotNetCoreDocs.Writers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetCoreDocs
{
    public class DocsFixture<TEntity, TStartup, TWriter> : IDisposable 
        where TStartup : class
        where TWriter : IWriter
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;
        private readonly DocsConfiguration _config;
        private readonly IWriter _writer;
        private IServiceProvider _services;

        public DocsFixture()
        {
            var builder = new WebHostBuilder()
                .UseStartup<TStartup>();

            _server = new TestServer(builder);
            _services = builder.Build().Services;
            _config = _services.GetService<DocsConfiguration>();
            _client = _server.CreateClient();
            _client.BaseAddress = new Uri(_config.BaseAddress);
            _writer = GetWriter();

            DeleteFile();
        }

        public T GetService<T>()
        {
            return (T)_services.GetService(typeof(T));
        }

        public async Task<HttpResponseMessage> MakeRequest(string description, HttpRequestMessage request)
        {
            await _writer.LoadRequestBodyAsync(request);

            var response = await _client.SendAsync(request);
            var modelName = typeof(TEntity).Name;
            await _writer.WriteRequestAsync(modelName, GetFileName(), description, request, response);

            return response;
        }

        public async Task<HttpResponseMessage> MakeRequest(string description, HttpMethod method, string route)
        {
            var request = new HttpRequestMessage(method, route);
            return await MakeRequest(description, request);
        }

        public void Dispose()
        {
            _client.Dispose();
            _server.Dispose();
        }

        private IWriter GetWriter()
        {
            return (IWriter)Activator.CreateInstance(typeof(TWriter), _config);
        }

        private string GetFileName()
        {
            return _config.GetRequestsFileName(typeof(TEntity).Name);
        }

        private void DeleteFile()
        {
            var fileName = GetFileName();
            if(File.Exists(fileName))
                File.Delete(fileName);
        }
    }
}
