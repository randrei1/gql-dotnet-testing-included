using GraphQL;
using GraphQL.Client.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyFancyApi.IntegrationTest.Response;
using MyFancyApi.Service;
using MyFancyApi.Service.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace MyFancyApi.IntegrationTest
{
    public class BookTypeTest : IClassFixture<WebApplicationFactory<MyFancyApi.Service.Startup>>, IAsyncLifetime
    {
        private readonly WebApplicationFactory<MyFancyApi.Service.Startup> _factory;
        private readonly ITestOutputHelper _testOutputHelper;

        public BookTypeTest(WebApplicationFactory<MyFancyApi.Service.Startup> factory, ITestOutputHelper testOutputHelper)
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
        public async Task CanGetAnyBooks()
        {
            var client = _factory.CreateClient();
            client.BaseAddress = new Uri("http://localhost/");
            var graphQLClient = CreateGraphQLClient();

            var BookQuery = new GraphQLRequest
            {
                Query = @"
query {
    books{
        name
    }
}"
            };

            var graphQLResponse = await graphQLClient.SendQueryAsync<BooksResponse>(BookQuery);
            _testOutputHelper.WriteLine(JsonConvert.SerializeObject(graphQLResponse));
            Assert.True(graphQLResponse.Data.Books.Any());
        }

        [Fact]
        public async Task CanGetBookId()
        {
            var client = _factory.CreateClient();
            client.BaseAddress = new Uri("http://localhost/");
            var graphQLClient = CreateGraphQLClient();

            var BookQueryWithVariables = new GraphQLRequest
            {
                Query = @"
                    query($id: Int) {
                        book(id: $id){
                            title
                            description
                        }
                    }",
                Variables = new
                {
                    id = 1
                }
            };

            var graphQLResponse = await graphQLClient.SendQueryAsync<BookResponse>(BookQueryWithVariables);
            _testOutputHelper.WriteLine(JsonConvert.SerializeObject(graphQLResponse));
            Assert.False(string.IsNullOrEmpty(graphQLResponse?.Data?.Book?.Title));
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
                dbContext.Books.Add(new Book { Title = "Foundation", AuthorId=1, Description = "Book description", Publisher = "Gnome Press", PublishDate = DateTime.Parse("1951-01-01") });

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