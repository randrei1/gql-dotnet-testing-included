using GraphQL.Types;
using MyFancyApi.Service.Models;
using MyFancyApi.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFancyApi.Service.GraphQL.Types
{
    public class AuthorType : ObjectGraphType<Author>
    {
        public AuthorType(IBookService bookService)
        {
            Field(e => e.Id);
            Field(e => e.Name);
            Field(e => e.Bio);
            FieldAsync<ListGraphType<BookType>>("Books", resolve: async context => {
                if (context.Source.Books != null)
                    return context.Source.Books;
                else
                {
                    return await bookService.GetBooksForAuthor(context.Source.Id);
                }
            });
        }
    }
}
