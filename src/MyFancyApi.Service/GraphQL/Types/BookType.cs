using GraphQL.Types;
using MyFancyApi.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFancyApi.Service.GraphQL.Types
{
    public class BookType : ObjectGraphType<Book>
    {
        public BookType()
        {
            Field(e => e.Id);
            Field(e => e.Title);
            Field(e => e.Publisher);
            Field(e => e.PublishDate);
            Field(e => e.Description);
        }
    }
}
