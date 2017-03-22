using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Language.AST;
using GraphQL.Types;
using GraphQL.Validation;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Caching.Memory;

namespace P7.GraphQLCore.Validators
{
    interface ICurrentEnterLeaveListenerState
    {
        EnterLeaveListenerState EnterLeaveListenerState { get; }
    }

    public interface IPluginValidationRule: IValidationRule { }
    public class TestValidationRule : IPluginValidationRule
    {
        class MyEnterLeaveListenerSink : IEnterLeaveListenerEventSink, ICurrentEnterLeaveListenerState
        {
            public EnterLeaveListenerState EnterLeaveListenerState { get; private set; }

            public void OnEvent(EnterLeaveListenerState enterLeaveListenerState)
            {
                EnterLeaveListenerState = enterLeaveListenerState;
            }
        }

        private List<IGraphQLAuthorizationCheck> _graphQLAuthorizationChecks;
        private List<IGraphQLClaimsAuthorizationCheck> _graphQLClaimsAuthorizationChecks;


        public TestValidationRule(
            IEnumerable<IGraphQLAuthorizationCheck> graphQLAuthorizationChecks,
            IEnumerable<IGraphQLClaimsAuthorizationCheck> graphQLClaimsAuthorizationChecks)
        {
            _graphQLAuthorizationChecks = graphQLAuthorizationChecks.ToList();
            _graphQLClaimsAuthorizationChecks = graphQLClaimsAuthorizationChecks.ToList();
        }
        
        public INodeVisitor Validate(ValidationContext context)
        {
            var userContext = context.UserContext.As<GraphQLUserContext>();
            var user = userContext.HttpContextAccessor.HttpContext.User;


            var authenticated = user?.Identity.IsAuthenticated ?? false;
            var myEnterLeaveListenerSink = new MyEnterLeaveListenerSink();
            var currentEnterLeaveListenerState = (ICurrentEnterLeaveListenerState) myEnterLeaveListenerSink;
            var myEnterLeaveListener = new MyEnterLeaveListener(_ =>
            {
                
                _.Match<Operation>(op =>
                {
                    var opType = op.OperationType;
                    var query = from item in op.SelectionSet.Selections
                        select ((GraphQL.Language.AST.Field) item).Name;

                    List<string> fieldNames = query.ToList();
                    foreach (var fieldName in fieldNames)
                    {
                        bool doAuthCheck = true;

                        bool AllUsersCheck = false;
                        foreach (var authCheck in _graphQLAuthorizationChecks)
                        {
                            AllUsersCheck = authCheck.ShouldDoAuthorizationCheck(opType, fieldName);
                            if (AllUsersCheck == true)
                                break;

                        }

                        bool justThisUserCheck = true;
                        foreach (var authCheck in _graphQLClaimsAuthorizationChecks)
                        {
                            justThisUserCheck = authCheck.ShouldDoAuthorizationCheck(user, opType, fieldName);
                            if (justThisUserCheck == false)
                                break;
                        }

                        // Any no is a no
                        doAuthCheck = AllUsersCheck && justThisUserCheck;


                        if (doAuthCheck && !authenticated)
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
                    var currentPath = currentEnterLeaveListenerState.EnterLeaveListenerState.CurrentFieldPath;
                    var fieldDef = context.TypeInfo.GetFieldDef();
                    var lastType = context.TypeInfo.GetLastType() as IGraphType;
                    var parentType = context.TypeInfo.GetParentType();
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
            myEnterLeaveListener.RegisterEventSink(myEnterLeaveListenerSink);
            return myEnterLeaveListener;
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
