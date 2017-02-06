﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Language.AST;
using GraphQL.Validation;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace P7.GraphQLCore.Validators
{
    public interface IRequiresAuthValidationRuleConfig
    {
        bool AllowAccess(ClaimsPrincipal claimsPrincipal, string fieldName);

    }

    public class InMemoryRequiresAuthValidationRuleConfig : IRequiresAuthValidationRuleConfig
    {
        private static Dictionary<string, bool> _allUsers;

        private static Dictionary<string, bool> AllUsers
        {
            get
            {
                return _allUsers ?? (_allUsers = new Dictionary<string, bool>()
                {
                    {"droids", true}
                });
            }
        }

        private static Dictionary<string, Dictionary<string, bool>> _individualUsers;

        private static Dictionary<string, Dictionary<string, bool>> IndividualUsers
        {
            get
            {
                return _individualUsers ?? (_individualUsers = new Dictionary<string, Dictionary<string, bool>>()
                {
                    {"blogs", new Dictionary<string, bool>() {{"herb", true}, {"susan", true}}}
                });
            }
        }

        private IMemoryCache _cache;

        public InMemoryRequiresAuthValidationRuleConfig(IMemoryCache cache)
        {
            _cache = cache;
        }

        public bool AllowAccess(ClaimsPrincipal claimsPrincipal, string fieldName)
        {
            if (claimsPrincipal == null)
            {
                return false;
            }

            // AllUsers win   
            if (AllUsers.ContainsKey(fieldName))
            {
                return true;
            }
            if (IndividualUsers.ContainsKey(fieldName))
            {
                var individualMap = IndividualUsers[fieldName];
                var result = from claim in claimsPrincipal.Claims
                    where claim.Type == ClaimTypes.NameIdentifier
                    select claim;

                foreach (var item in result)
                {
                    var id = item.Value;
                    if (individualMap.ContainsKey(id))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }

    public class TestValidationRule : IValidationRule
    {
        private IRequiresAuthValidationRuleConfig _requiresAuthValidationRuleConfig;
        public TestValidationRule(IRequiresAuthValidationRuleConfig requiresAuthValidationRuleConfig)
        {
            _requiresAuthValidationRuleConfig = requiresAuthValidationRuleConfig;

        }
        public INodeVisitor Validate(ValidationContext context)
        {
            var userContext = context.UserContext.As<GraphQLUserContext>();
            var user = userContext.HttpContextAccessor.HttpContext.User;
          

            var authenticated = user?.Identity.IsAuthenticated ?? false;

            return new EnterLeaveListener(_ =>
            {
                _.Match<Operation>(op =>
                {
                    var opType = op.OperationType;
                    var query = from item in op.SelectionSet.Selections
                        select ((GraphQL.Language.AST.Field) item).Name;

                    List<string> fieldNames = query.ToList();
                    foreach (var fieldName in fieldNames)
                    {
                        if (!_requiresAuthValidationRuleConfig.AllowAccess(user, fieldName))
                        {
                            context.ReportError(new ValidationError(
                                context.OriginalQuery,
                                "auth-required",
                                $"Authorization is required to access {op.Name} and field {fieldName}.",
                                op));
                            break;
                        }
                    }


                });
                _.Match<Field>(fieldAst =>
                {
                    var fieldDef = context.TypeInfo.GetFieldDef();
                    var name = fieldAst.Name;
                });
                /*
                // this could leak info about hidden fields in error messages
                // it would be better to implement a filter on the schema so it
                // acts as if they just don't exist vs. an auth denied error
                // - filtering the schema is not currently supported
                _.Match<Field>(fieldAst =>
                {
                    var fieldDef = context.TypeInfo.GetFieldDef();
                    if (fieldDef.RequiresPermissions() &&
                        (!authenticated || !fieldDef.CanAccess(userContext.User.Claims)))
                    {
                        context.ReportError(new ValidationError(
                            context.OriginalQuery,
                            "auth-required",
                            $"You are not authorized to run this query.",
                            fieldAst));
                    }
                });
                */
            });
        }
    }

    public class RequiresAuthValidationRule : IValidationRule
    {
        public INodeVisitor Validate(ValidationContext context)
        {
            var userContext = context.UserContext.As<GraphQLUserContext>();
            var user = userContext.HttpContextAccessor.HttpContext.User;


            var authenticated = user?.Identity.IsAuthenticated ?? false;

            return new EnterLeaveListener(_ =>
            {
                _.Match<Operation>(op =>
                {
                    if (op.OperationType == OperationType.Mutation && !authenticated)
                    {
                        context.ReportError(new ValidationError(
                            context.OriginalQuery,
                            "auth-required",
                            $"Authorization is required to access {op.Name}.",
                            op));
                    }
                });
                /*
                // this could leak info about hidden fields in error messages
                // it would be better to implement a filter on the schema so it
                // acts as if they just don't exist vs. an auth denied error
                // - filtering the schema is not currently supported
                _.Match<Field>(fieldAst =>
                {
                    var fieldDef = context.TypeInfo.GetFieldDef();
                    if (fieldDef.RequiresPermissions() &&
                        (!authenticated || !fieldDef.CanAccess(userContext.User.Claims)))
                    {
                        context.ReportError(new ValidationError(
                            context.OriginalQuery,
                            "auth-required",
                            $"You are not authorized to run this query.",
                            fieldAst));
                    }
                });
                */
            });
        }
    }
}
