using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using GraphQL;

namespace MyFancyApi.Service.GraphQL
{
    public class MyFancySchema : Schema
    {
        public MyFancySchema(IDependencyResolver resolver) : base(resolver)
        {
            Query = resolver.Resolve<RootQuery>();
        }
    }
}
