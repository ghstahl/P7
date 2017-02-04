using System.Collections.Generic;
using GraphQL.Language.AST;
using GraphQL.Types;
using Newtonsoft.Json;

namespace P7.BlogStore.Core.GraphQL
{
    public class BlogMetaDataType : ScalarGraphType
    {
        public BlogMetaDataType()
        {
            Name = "BlogMetaDataType";
        }

        public override object Serialize(object value)
        {
            string result;
            /*
            var model = value as Dictionary<string, object>;
            var model2 = value as BlogMetaData;
            if (model != null)
                result = JsonConvert.SerializeObject(model);
            else
            {
                result = JsonConvert.SerializeObject(value);
            }
            */

            result = JsonConvert.SerializeObject(value);
            return result;
        }

        public override object ParseValue(object value)
        {
            return value;
        }

        public override object ParseLiteral(IValue value)
        {
            if (value is StringValue)
            {
                var json = ((StringValue) value).Value;
                var db = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                return db;
            }
            return null;
        }
    }
}