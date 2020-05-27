using GraphQL;
using GraphQL.Client.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace MyFancyApi.IntegrationTest
{
    public class BasicTests
    : IClassFixture<WebApplicationFactory<MyFancyApi.Service.Startup>>
    {
        private readonly WebApplicationFactory<MyFancyApi.Service.Startup> _factory;
        private readonly ITestOutputHelper _testOutputHelper;

        public BasicTests(WebApplicationFactory<MyFancyApi.Service.Startup> factory, ITestOutputHelper testOutputHelper)
        {
            _factory = factory;
            _testOutputHelper = testOutputHelper;
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
            _testOutputHelper.WriteLine(await response.Content.ReadAsStringAsync());
            response.EnsureSuccessStatusCode(); // Status Code 200-299
        }
        [Fact]
        public async Task TestGraphQL()
        {
            var client = _factory.CreateClient();
            client.BaseAddress = new Uri("http://localhost/");
            var graphQLClient = CreateGraphQLClient();

            var schemaIntrospectionQuery = new GraphQLRequest
            {
                Query = @"
query IntrospectionQuery {
  __schema {
    queryType {
      name
    }
    mutationType {
      name
    }
    subscriptionType {
      name
    }
    types {
      ...FullType
    }
    directives {
      name
      description
      locations
      args {
        ...InputValue
      }
    }
  }
}

fragment FullType on __Type {
  kind
  name
  description
  fields(includeDeprecated: true) {
    name
    description
    args {
      ...InputValue
    }
    type {
      ...TypeRef
    }
    isDeprecated
    deprecationReason
  }
  inputFields {
    ...InputValue
  }
  interfaces {
    ...TypeRef
  }
  enumValues(includeDeprecated: true) {
    name
    description
    isDeprecated
    deprecationReason
  }
  possibleTypes {
    ...TypeRef
  }
}

fragment InputValue on __InputValue {
  name
  description
  type {
    ...TypeRef
  }
  defaultValue
}

fragment TypeRef on __Type {
  kind
  name
  ofType {
    kind
    name
    ofType {
      kind
      name
      ofType {
        kind
        name
        ofType {
          kind
          name
          ofType {
            kind
            name
            ofType {
              kind
              name
              ofType {
                kind
                name
              }
            }
          }
        }
      }
    }
  }
}"
            };

            var graphQLResponse = await graphQLClient.SendQueryAsync<JObject>(schemaIntrospectionQuery);
            _testOutputHelper.WriteLine(JsonConvert.SerializeObject(graphQLResponse));
            Assert.True(graphQLResponse.Errors == null);
        }
        public GraphQLHttpClient CreateGraphQLClient()
        {
            HttpClient httpClient = _factory.CreateClient();
            return httpClient.AsGraphQLClient("http://localhost/graphql");
        }
    }
}