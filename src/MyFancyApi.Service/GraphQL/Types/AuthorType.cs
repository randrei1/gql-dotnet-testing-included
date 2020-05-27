using GraphQL.Types;
using MyFancyApi.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFancyApi.Service.GraphQL.Types
{
    public class AuthorType : ObjectGraphType<Author>
    {
        public AuthorType()
        {
            Field(e => e.Id);
            Field(e => e.Name);
            Field(e => e.Bio);
        }
    }
}
