using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using GraphQL;
using GraphQL.Http;
using GraphQL.Types;
using GraphQL.Validation;
using GraphQLParser.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using P7.BlogStore.Hugo;
using P7.Core;
using P7.GraphQLCore;
using Shouldly;
namespace Test.P7.GraphQLCoreTest
{
    class MyBiggyConfiguration : IBiggyConfiguration
    {
        public string DatabaseName { get; set; }
        public string FolderStorage { get; set; }
    }
    [TestClass]
    public class LibraryTests
    {
        public TenantDatabaseBiggyConfig GlobalTenantDatabaseBiggyConfig { get; set; }
        private string _targetFolder;
        protected string TargetFolder => _targetFolder;
        public IDocumentExecuter Executer { get; private set; }

        public IDocumentWriter Writer { get; private set; }
        public GraphQLUserContext GraphQLUserContext { get; private set; }
        public ISchema Schema
        {
            get
            {
                var schema = AutofacStoreFactory.Resolve<ISchema>();
                return schema;
            }
        }
        private MyAutofacFactory AutofacStoreFactory { get; set; }
        [TestInitialize]
        public void Initialize()
        {
            _targetFolder = Path.Combine(UnitTestHelpers.BaseDir, @"source");

            GlobalTenantDatabaseBiggyConfig = new TenantDatabaseBiggyConfig();
            GlobalTenantDatabaseBiggyConfig.UsingFolder(TargetFolder);
            GlobalTenantDatabaseBiggyConfig.UsingTenantId(TenantDatabaseBiggyConfig.GlobalTenantId);
            IBiggyConfiguration biggyConfiguration = new MyBiggyConfiguration()
            {
                FolderStorage = GlobalTenantDatabaseBiggyConfig.Folder,
                DatabaseName = GlobalTenantDatabaseBiggyConfig.Database
            };

            var hostName = typeof(MyAutofacFactory).GetTypeInfo().Assembly.GetName().Name;
            var hostingEnvironment = A.Fake<IHostingEnvironment>();
            var httpContextAccessor = A.Fake<IHttpContextAccessor>();


            hostingEnvironment.ApplicationName = hostName;
            Global.HostingEnvironment = hostingEnvironment;
            AutofacStoreFactory = new MyAutofacFactory() {BiggyConfiguration = biggyConfiguration };

            Executer = AutofacStoreFactory.Resolve<IDocumentExecuter>();
            Writer = AutofacStoreFactory.Resolve<IDocumentWriter>();
            GraphQLUserContext = AutofacStoreFactory.Resolve<GraphQLUserContext>();
        }
        [TestMethod]
        public void default_culture_kvo_success_request()
        {
            var d = AutofacStoreFactory.AutofacContainer;

            var dd = AutofacStoreFactory.Resolve<IQueryFieldRecordRegistration>();
            var cc = AutofacStoreFactory.Resolve<IQueryFieldRecordRegistrationStore>();
            var id = @"Test.P7.GraphQLCoreTest.Resources.Main,Test.P7.GraphQLCoreTest";
            var treatment = @"kvo";
            var gqlInputs = $"{{'input': {{'id':'{id}', 'treatment': '{treatment}' }} }}"
                .ToInputs();
            var query = @"
                query Q($input: ResourceQueryInput!) {
                  resource(input: $input)
                }";

            var expected = @"{'resource':{'hello':'Hello'}}";
            AssertQuerySuccess(query, expected, gqlInputs,root:null,userContext: GraphQLUserContext);


            Assert.AreEqual(42, 42);
        }
        [TestMethod]
        public void default_culture_kva_success_request()
        {
            var d = AutofacStoreFactory.AutofacContainer;

            var dd = AutofacStoreFactory.Resolve<IQueryFieldRecordRegistration>();
            var cc = AutofacStoreFactory.Resolve<IQueryFieldRecordRegistrationStore>();
            var id = @"Test.P7.GraphQLCoreTest.Resources.Main,Test.P7.GraphQLCoreTest";
            var treatment = @"kva";
            var gqlInputs = $"{{'input': {{'id':'{id}', 'treatment': '{treatment}' }} }}"
                .ToInputs();
            var query = @"
                query Q($input: ResourceQueryInput!) {
                  resource(input: $input)
                }";


            var expected = @"{'resource':[{'key': 'Hello','value': 'Hello'}]}";
            AssertQuerySuccess(query, expected, gqlInputs, root: null, userContext: GraphQLUserContext);


            Assert.AreEqual(42, 42);
        }

        public ExecutionResult CreateQueryResult(string result)
        {
            object expected = null;
            if (!string.IsNullOrWhiteSpace(result))
            {
                expected = JObject.Parse(result);
            }
            return new ExecutionResult { Data = expected };
        }
        public ExecutionResult AssertQuerySuccess(
            string query,
            string expected,
            Inputs inputs = null,
            object root = null,
            object userContext = null,
            CancellationToken cancellationToken = default(CancellationToken),
            IEnumerable<IValidationRule> rules = null)
        {
            var queryResult = CreateQueryResult(expected);
            return AssertQuery(query, queryResult, inputs, root, userContext, cancellationToken, rules);
        }
        public ExecutionResult AssertQueryWithErrors(
           string query,
           string expected,
           Inputs inputs = null,
           object root = null,
           object userContext = null,
           CancellationToken cancellationToken = default(CancellationToken),
           int expectedErrorCount = 0)
        {
            var queryResult = CreateQueryResult(expected);
            return AssertQueryIgnoreErrors(query, queryResult, inputs, root, userContext, cancellationToken, expectedErrorCount);
        }

        public ExecutionResult AssertQueryIgnoreErrors(
            string query,
            ExecutionResult expectedExecutionResult,
            Inputs inputs,
            object root,
            object userContext = null,
            CancellationToken cancellationToken = default(CancellationToken),
            int expectedErrorCount = 0)
        {
            var runResult = Executer.ExecuteAsync(Schema, root, query, null, inputs, userContext, cancellationToken).Result;

            var writtenResult = Writer.Write(new ExecutionResult { Data = runResult.Data });
            var expectedResult = Writer.Write(expectedExecutionResult);

            // #if DEBUG
            //             Console.WriteLine(writtenResult);
            // #endif

            writtenResult.ShouldBe(expectedResult);

            var errors = runResult.Errors ?? new ExecutionErrors();

            errors.Count().ShouldBe(expectedErrorCount);

            return runResult;
        }
        public ExecutionResult AssertQuery(
            string query,
            ExecutionResult expectedExecutionResult,
            Inputs inputs,
            object root,
            object userContext = null,
            CancellationToken cancellationToken = default(CancellationToken),
            IEnumerable<IValidationRule> rules = null)
        {
            var runResult = Executer.ExecuteAsync(
                Schema,
                root,
                query,
                null,
                inputs,
                userContext,
                cancellationToken,
                rules
            ).Result;

            var writtenResult = Writer.Write(runResult);
            var expectedResult = Writer.Write(expectedExecutionResult);

            // #if DEBUG
            //             Console.WriteLine(writtenResult);
            // #endif

            string additionalInfo = null;

            if (runResult.Errors?.Any() == true)
            {
                additionalInfo = string.Join(Environment.NewLine, runResult.Errors
                    .Where(x => x.InnerException is GraphQLSyntaxErrorException)
                    .Select(x => x.InnerException.Message));
            }

            writtenResult.ShouldBe(expectedResult, additionalInfo);

            return runResult;
        }
    }
}
