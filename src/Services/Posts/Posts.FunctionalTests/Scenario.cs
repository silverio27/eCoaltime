using Posts.Api;
using Posts.Api.Recipes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Posts.FunctionalTests
{
    public class Scenario 
    {
        private readonly HttpClient _client;
        public Scenario()
        {
            var server = new TestServer(new WebHostBuilder()
                .UseEnvironment("Development")
                .UseStartup<Startup>());
            _client = server.CreateClient();
        }
        [Fact]
        public async Task DraftSuccess()
        {
            string draft = BuildCreateDraft();
            StringContent content = new(draft, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("api/v1/posts", content);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        private static string BuildCreateDraft()
        {
            Author user = new("img.jpg","Lucas Silvério", Guid.NewGuid());
            CreatePost draft = new(user);
            return JsonSerializer.Serialize(draft);
        }

    }
}
