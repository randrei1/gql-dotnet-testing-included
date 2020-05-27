using GraphQL.Types;
using MyFancyApi.Service.GraphQL.Types;
using MyFancyApi.Service.Models;
using MyFancyApi.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFancyApi.Service.GraphQL
{
    public class RootQuery : ObjectGraphType
    {
        public RootQuery(IAuthorService authorService)
        {
            Field<StringGraphType>(
                Name = "ping",
                resolve: context => "I'm alive!"
            );

            FieldAsync<ListGraphType<AuthorType>>(
                Name = "authors",
                resolve: async context => await authorService.GetAuthors()
            );

            FieldAsync<AuthorType>(
                Name = "author",
                arguments: new QueryArguments(
                    new QueryArgument<IntGraphType>
                    {
                        Name = "id"
                    }),
                resolve: async context =>
                {
                    var id = context.GetArgument<int>("id");
                    return await authorService.GetAuthor(id);
                }
            ); ;
        }
    }
}
