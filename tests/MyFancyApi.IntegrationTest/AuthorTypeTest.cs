using GraphQL;
using GraphQL.Client.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyFancyApi.Service;
using MyFancyApi.Service.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace MyFancyApi.IntegrationTest
{
    public class AuthorTypeTest : IClassFixture<WebApplicationFactory<MyFancyApi.Service.Startup>>, IAsyncLifetime
    {
        private readonly WebApplicationFactory<MyFancyApi.Service.Startup> _factory;
        private readonly ITestOutputHelper _testOutputHelper;
        private LibContext _context;

        public AuthorTypeTest(WebApplicationFactory<MyFancyApi.Service.Startup> factory, ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            _factory = factory.WithWebHostBuilder(builder =>
               builder.ConfigureServices(services =>
               {
                   // Create a new service provider.
                   var serviceProvider = new ServiceCollection()
                      .AddEntityFrameworkInMemoryDatabase()
                      .BuildServiceProvider();
                   // Add a database context (LibContext) using an in-memory 
                   // database for testing.
                   services.AddDbContext<LibContext>(dbOptions =>
                   {
                       dbOptions.UseInMemoryDatabase("InMemoryDbForTesting");
                       dbOptions.UseInternalServiceProvider(serviceProvider);
                   });
               })
            );
            _factory.CreateClient();
            
            
        }

        [Fact]
        public async Task CanPing()
        {
            var client = _factory.CreateClient();
            client.BaseAddress = new Uri("http://localhost/");
            var graphQLClient = CreateGraphQLClient();

            var authorQuery = new GraphQLRequest
            {
                Query = @"
query {
    ping
}"
            };

            var graphQLResponse = await graphQLClient.SendQueryAsync<JObject>(authorQuery);
            _testOutputHelper.WriteLine(JsonConvert.SerializeObject(graphQLResponse));
            Assert.True(graphQLResponse.Errors == null);
        }

        [Fact]
        public async Task CanGetAnyAuthors()
        {
            var client = _factory.CreateClient();
            client.BaseAddress = new Uri("http://localhost/");
            var graphQLClient = CreateGraphQLClient();

            var authorQuery = new GraphQLRequest
            {
                Query = @"
query {
    authors{
        name
    }
}"
            };

            var graphQLResponse = await graphQLClient.SendQueryAsync<JObject>(authorQuery);
            _testOutputHelper.WriteLine(JsonConvert.SerializeObject(graphQLResponse));
            Assert.True(graphQLResponse.Errors == null);
        }

        public Task DisposeAsync()
        {
            using (var scope = _factory.Server.Host.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<LibContext>();
                return Task.Run(() => dbContext.Dispose());
            }
        }

        public async Task InitializeAsync()
        {
            using (var scope = _factory.Server.Host.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<LibContext>();

                dbContext.Authors.Add(new Author { Name = "Isaac Asimov", Bio = "American writer and professor of biochemistry at Boston University. He was known for his works of science fiction and popular science." });
                
                await dbContext.SaveChangesAsync();
            }
        }
        public GraphQLHttpClient CreateGraphQLClient()
        {
            HttpClient httpClient = _factory.CreateClient();
            return httpClient.AsGraphQLClient("http://localhost/graphql");
        }
    }
}
