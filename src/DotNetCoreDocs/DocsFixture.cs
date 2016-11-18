using System;
using System.Net.Http;
using System.Threading.Tasks;
using DotNetCoreDocs.Writers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace DotNetCoreDocs
{
    public class DocsFixture<TStartup, TWriter> : IDisposable 
        where TStartup : class
        where TWriter : IWriter
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;
        private const string _defaultBaseAddress = "http://localhost:5000";
        private readonly IWriter _writer;

        public DocsFixture()
        {
            var builder = new WebHostBuilder().UseStartup<TStartup>();
            _server = new TestServer(builder);

            _client = _server.CreateClient();
            SetBaseAddress(new Uri(_defaultBaseAddress));

            _writer = GetWriter();
        }

        public void SetBaseAddress(Uri address)
        {
            _client.BaseAddress = address;
        }

        public async Task<HttpResponseMessage> MakeRequest(HttpRequestMessage request)
        {
            var response = await _client.SendAsync(request);

            await _writer.WriteRequestAsync(request, response);

            return response;
        }

        public async Task<HttpResponseMessage> MakeRequest(HttpMethod method, string route)
        {
            var request = new HttpRequestMessage(method, route);
            return await MakeRequest(request);
        }

        public void Dispose()
        {
            _client.Dispose();
            _server.Dispose();
        }

        private IWriter GetWriter()
        {
            return Activator.CreateInstance<TWriter>();
        }
    }
}
