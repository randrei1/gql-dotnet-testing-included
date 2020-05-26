using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using Xunit;

namespace MyFancyApi.IntegrationTest
{
    public class BasicTests
    : IClassFixture<WebApplicationFactory<MyFancyApi.Service.Startup>>
    {
        private readonly WebApplicationFactory<MyFancyApi.Service.Startup> _factory;

        public BasicTests(WebApplicationFactory<MyFancyApi.Service.Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/")]//we will route to some response
        [InlineData("/api/values")]//will work because controller is implemented
        [InlineData("/graphql")]//will work after graphql is added
        [InlineData("/ui/playground")]//will work after graphql playground is added
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
        }
    }
}