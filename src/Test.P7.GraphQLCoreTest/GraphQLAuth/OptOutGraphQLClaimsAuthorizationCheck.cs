using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using GraphQL.Language.AST;
using P7.GraphQLCore.Validators;

namespace Test.P7.GraphQLCoreTest.GraphQLAuth
{
    public class OptOutGraphQLClaimsAuthorizationCheck : IGraphQLClaimsAuthorizationCheck
    {
        private static Dictionary<OperationType, Dictionary<string, List<string>>> _individualUsersOptOut;

        private static Dictionary<OperationType, Dictionary<string, List<string>>> IndividualUsersOptOut
        {
            get
            {
                return _individualUsersOptOut ?? (_individualUsersOptOut
                           = new Dictionary<OperationType, Dictionary<string, List<string>>>()
                           {
                               {
                                   OperationType.Query,
                                   new Dictionary<string, List<string>>()
                                   {
                                       {"droids", new List<string>() {"herb"}}
                                   }
                               }
                           });
            }
        }

        public bool ShouldDoAuthorizationCheck(ClaimsPrincipal claimsPrincipal, OperationType operationTye, string fieldName)
        {
            if (!IndividualUsersOptOut.ContainsKey(operationTye))
                return true;

            var individualMap = IndividualUsersOptOut[operationTye];
            if (!individualMap.ContainsKey(fieldName))
                return true;

            var fieldList = individualMap[fieldName];


            var result = from claim in claimsPrincipal.Claims
                where claim.Type == ClaimTypes.NameIdentifier
                select claim;

            foreach (var item in result)
            {
                var id = item.Value;
                var q = from f in fieldList
                    where f == id
                    select f;
                if (q.Any())
                    return false;
            }

            return true;
        }
    }
}