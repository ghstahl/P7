﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GraphQL.Language.AST;
using Microsoft.Extensions.Caching.Memory;
using P7.GraphQLCore.Validators;

namespace Test.P7.GraphQLCoreTest.GraphQLAuth
{
    public class OptOutGraphQlAuthorizationCheck : IGraphQLAuthorizationCheck
    {
        private static Dictionary<OperationType, Dictionary<string, bool>> _allUsersOptOut;
        private static Dictionary<OperationType, Dictionary<string, bool>> AllUsersOptOut
        {
            get
            {
                return _allUsersOptOut ?? (_allUsersOptOut = new Dictionary<OperationType, Dictionary<string, bool>>()
                {
                    {OperationType.Query, new Dictionary<string, bool>() {{"blogs", true}}}
                });
            }
        }

        public bool ShouldDoAuthorizationCheck(OperationType operationTye, string fieldName)
        {
            if (!AllUsersOptOut.ContainsKey(operationTye))
                return true;

            var theMap = AllUsersOptOut[operationTye];

            // if the fieldname is in the optout map then everybody gets access.
            //  i.e. the field name has opted out of this authorization check
            if (theMap.ContainsKey(fieldName))
                return false;

            return true;
        }
    }

}
