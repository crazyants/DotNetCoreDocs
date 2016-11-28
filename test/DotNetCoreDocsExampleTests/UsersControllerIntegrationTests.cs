using System;
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
    public class UserControllerIntegrationTests : IClassFixture<DocsFixture<User, Startup, JsonDocWriter>>
    {
        private DocsFixture<User, Startup, JsonDocWriter> _fixture;
        private AppDbContext _context;
        private Faker<User> _userFaker;

        public UserControllerIntegrationTests(DocsFixture<User, Startup, JsonDocWriter> fixture)
        {
            _fixture = fixture;
            _context = fixture.GetService<AppDbContext>();
            _userFaker = new Faker<User>()
                .RuleFor(u => u.FirstName, f=> f.Name.FirstName())
                .RuleFor(u => u.LastName, f=> f.Name.LastName());
        }

        [Fact]
        public async Task Can_Get_Users()
        {
            // Arrange
            var user = _userFaker.Generate();
            _context.Users.Add(user);
            _context.SaveChanges();

            var httpMethod = new HttpMethod("GET");
            var route = "/api/Users";

            // Act
            var response = await _fixture.MakeRequest("Get Users", httpMethod, route);
            var body = await response.Content.ReadAsStringAsync();
            var deserializedBody = JsonConvert.DeserializeObject<List<User>>(body);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotEmpty(deserializedBody);
        }

        [Fact]
        public async Task Can_Get_User_ById()
        {
            // Arrange
            var user = _userFaker.Generate();
            _context.Users.Add(user);
            _context.SaveChanges();

            var httpMethod = new HttpMethod("GET");
            var route = $"/api/Users/{user.Id}";

            // Act
            var response = await _fixture.MakeRequest("Get User By Id", httpMethod, route);
            var body = await response.Content.ReadAsStringAsync();
            var deserializedBody = JsonConvert.DeserializeObject<User>(body);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(user.Id, deserializedBody.Id);
            Assert.Equal(user.FirstName, deserializedBody.FirstName);
            Assert.Equal(user.LastName, deserializedBody.LastName);
        }

        [Fact]
        public async Task Can_Post_User()
        {
            // Arrange
            var user = _userFaker.Generate();
            var httpMethod = new HttpMethod("POST");
            var route = "/api/Users";
            var request = new HttpRequestMessage(httpMethod, route);
            request.Content = new StringContent(JsonConvert.SerializeObject(user));
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            
            // Act
            var response = await _fixture.MakeRequest("Post User", request);
            var body = await response.Content.ReadAsStringAsync();
            var deserializedBody = JsonConvert.DeserializeObject<User>(body);
            
            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal(user.FirstName, deserializedBody.FirstName);
            Assert.Equal(user.LastName, deserializedBody.LastName);
        }
    }
}
