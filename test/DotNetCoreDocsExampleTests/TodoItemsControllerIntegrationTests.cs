using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Bogus;
using DotNetCoreApplicationsExample.Data;
using DotNetCoreDocs;
using DotNetCoreDocs.Writers;
using DotNetCoreDocsExample;
using DotNetCoreDocsExample.Models;
using Newtonsoft.Json;
using Xunit;

namespace DotNetCoreDocsExampleTests
{
    public class TodoItemControllerIntegrationTests : IClassFixture<DocsFixture<TodoItem, Startup, JsonDocWriter>>
    {
        private DocsFixture<TodoItem, Startup, JsonDocWriter> _fixture;
        private AppDbContext _context;
        private Faker<TodoItem> _todoItemFaker;

        public TodoItemControllerIntegrationTests(DocsFixture<TodoItem, Startup, JsonDocWriter> fixture)
        {
            _fixture = fixture;
            _context = fixture.GetService<AppDbContext>();
            _todoItemFaker = new Faker<TodoItem>()
                .RuleFor(t => t.Description, f => f.Lorem.Sentence());
        }

        [Fact]
        public async Task Can_Get_TodoItems()
        {
            // Arrange
            var todoItem = _todoItemFaker.Generate();
            _context.TodoItems.Add(todoItem);
            _context.SaveChanges();

            var httpMethod = new HttpMethod("GET");
            var route = "/api/TodoItems";

            // Act
            var response = await _fixture.MakeRequest("Get TodoItems", httpMethod, route);
            var body = await response.Content.ReadAsStringAsync();
            var deserializedBody = JsonConvert.DeserializeObject<List<TodoItem>>(body);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotEmpty(deserializedBody);
        }

        [Fact]
        public async Task Can_Get_TodoItem_ById()
        {
            // Arrange
            var todoItem = _todoItemFaker.Generate();
            _context.TodoItems.Add(todoItem);
            _context.SaveChanges();

            var httpMethod = new HttpMethod("GET");
            var route = $"/api/TodoItems/{todoItem.Id}";

            // Act
            var response = await _fixture.MakeRequest("Get TodoItem By Id", httpMethod, route);
            var body = await response.Content.ReadAsStringAsync();
            var deserializedBody = JsonConvert.DeserializeObject<TodoItem>(body);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(todoItem.Id, deserializedBody.Id);
            Assert.Equal(todoItem.Description, deserializedBody.Description);
        }

        [Fact]
        public async Task Can_Post_TodoItem()
        {
            // Arrange
            var todoItem = _todoItemFaker.Generate();
            var httpMethod = new HttpMethod("POST");
            var route = "/api/TodoItems";
            var request = new HttpRequestMessage(httpMethod, route);
            request.Content = new StringContent(JsonConvert.SerializeObject(todoItem));
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            
            // Act
            var response = await _fixture.MakeRequest("Post TodoItem", request);
            var body = await response.Content.ReadAsStringAsync();
            var deserializedBody = JsonConvert.DeserializeObject<TodoItem>(body);
            
            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal(todoItem.Description, deserializedBody.Description);
        }
    }
}
