using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFancyApi.Service.GraphQL
{
    public class RootQuery : ObjectGraphType
    {
        public RootQuery()
        {
            Field<StringGraphType>(
                Name = "ping",
                resolve: context => "I'm alive!"
            );
        }
    }
}
