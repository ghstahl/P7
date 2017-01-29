using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Types;

namespace P7.BlogStore.Hugo.GraphQL
{
    public class BlogQueryInput : InputObjectGraphType
    {
        public BlogQueryInput()
        {
            Name = "BlogQueryInput";
            Field<NonNullGraphType<StringGraphType>>("id");
        }
    }
}
