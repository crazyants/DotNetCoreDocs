using System.IO;
using DotNetCoreDocs.Models;
using HandlebarsDotNet;
using Newtonsoft.Json;

namespace DotNetCoreDocs
{
    internal static class JsonParser
    {
        private const string _readFile = "_data/requests.json";
        public static string GetJson()
        {
            string source =
                @"<div class=""entry"">
                <h1>Request</h1>
                <div class=""body"">
                    <p>{{Request.RequestUri}}</p>
                    <p>{{Response.Body}}</p>
                </div>
                </div>";
            var json = File.ReadAllText(_readFile);
            var template = Handlebars.Compile(source);
            var data = JsonConvert.DeserializeObject<TestRequest>(json);
            return template(data);
        }
    }
}
