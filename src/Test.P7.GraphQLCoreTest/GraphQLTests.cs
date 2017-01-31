using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter;
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
using P7.BlogStore.Core;
using P7.BlogStore.Hugo;
using P7.Core;
using P7.Core.Writers;
using P7.GraphQLCore;
using P7.Store;
using Shouldly;
namespace Test.P7.GraphQLCoreTest
{
    class MyBiggyConfiguration : IBiggyConfiguration
    {
        public string DatabaseName { get; set; }
        public string FolderStorage { get; set; }
    }
    [TestClass]
    [DeploymentItem("source", "source")]
    public class GraphQLTests
    {
        private JsonDocumentWriter _DocumentWriter;

        protected JsonDocumentWriter JsonDocumentWriter
        {
            get
            {
                return _DocumentWriter ?? new JsonDocumentWriter();
            }
        }

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
            _targetFolder = Path.Combine(UnitTestHelpers.BaseDir, @"source", DateTime.Now.ToString("yyyy-dd-M__HH-mm-ss"));
            Directory.CreateDirectory(_targetFolder);
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

        [TestMethod]
        public void blog_paging_request()
        {

            var dtBase = DateTime.UtcNow;
            var tsS = dtBase.ToString(JsonDocumentWriter.JsonSerializerSettings.DateFormatString);

            dtBase = DateTime.Parse(tsS, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);



            List<Blog> blogEntries = new List<Blog>();

            int timeMultipier = 1;
            for (int i = 0; i < 10; ++i)
            {
                timeMultipier = timeMultipier * -1;
                DateTime timeStamp = dtBase.AddMinutes(i* timeMultipier);
                int n = i % 2;
                Blog blog = new Blog()
                {
                    Id = Guid.NewGuid(),
                    Categories = new List<string>() { "c1"+n, "c2" + n },
                    Tags = new List<string>() { "t1" + n, "t2" + n },
                    MetaData = new BlogMetaData() { Category = "c0", Version = "1.0.0.0" },
                    Data = "This is my blog",
                    TimeStamp = timeStamp,
                    Summary = "My Summary",
                    Title = "My Title"
                };
                blogEntries.Add(blog);
            }
            foreach (var blogEntry in blogEntries)
            {
                var jsonBlog = JsonDocumentWriter.SerializeObjectSingleQuote(blogEntry);

                var rawInput = $"{{'input': {jsonBlog} }}";
                var gqlInputs = rawInput.ToInputs();
                var mutation = @"
                mutation Q($input: BlogMutationInput!) {
                  blog(input: $input)
                }";

                var expected = @"{'blog':true}";
                AssertQuerySuccess(mutation, expected, gqlInputs, root: null, userContext: GraphQLUserContext);
                rawInput = $"{{'input': {{'id':'{blogEntry.Id.ToString()}' }} }}";
                gqlInputs = rawInput.ToInputs();
                var query = @"query Q($input: BlogQueryInput!) {
                  blog(input: $input)
                }";
                var runResult = ExecuteQuery(query, gqlInputs, root: null, userContext: GraphQLUserContext);
                bool bRun = runResult.Errors?.Any() == true;
                Assert.IsFalse(bRun);

                Dictionary<string, object> data = (Dictionary<string, object>)runResult.Data;
                Dictionary<string, object> dataExpected = new Dictionary<string, object> { { "blog", blogEntry } };

                string additionalInfo = null;
                blogEntry.EnableDeepCompare = true;
                data.ShouldBe(dataExpected, additionalInfo);
            }
            blogEntries = blogEntries.OrderBy(o => o.TimeStamp).ToList();


            var pageSize = 3;
            var rawInput2 = $"{{'input': {{'pageSize':'{pageSize}' }} }}";
            var gqlInputs2 = rawInput2.ToInputs();
            var query2 = @"query Q($input: BlogsQueryInput!) {
                  blogs(input: $input)
                }";
            var runResult2 = ExecuteQuery(query2, gqlInputs2, root: null, userContext: GraphQLUserContext);
            bool bRun2 = runResult2.Errors?.Any() == true;
            Assert.IsFalse(bRun2);

        
            var slice = blogEntries.Skip(0).Take(pageSize).ToList();
            foreach (var item in slice)
            {
                item.EnableDeepCompare = true;
            }
            Dictionary<string, object> result = (Dictionary<string, object>) runResult2.Data;
            IPage<Blog> page = (IPage<Blog>) result["blogs"];

            var read = page.ToList();
            slice.ShouldBe(read);

            var pagingState = page.PagingState;
            rawInput2 = $"{{'input': {{'pageSize':'{pageSize}','pagingState':'{pagingState.SafeConvertToBase64String()}' }} }}";
            gqlInputs2 = rawInput2.ToInputs();
            query2 = @"query Q($input: BlogsQueryInput!) {
                  blogs(input: $input)
                }";
            runResult2 = ExecuteQuery(query2, gqlInputs2, root: null, userContext: GraphQLUserContext);
            bRun2 = runResult2.Errors?.Any() == true;
            Assert.IsFalse(bRun2);

            slice = blogEntries.Skip(3).Take(pageSize).ToList();
            foreach (var item in slice)
            {
                item.EnableDeepCompare = true;
            }
            result = (Dictionary<string, object>)runResult2.Data;
            page = (IPage<Blog>)result["blogs"];

            read = page.ToList();
            slice.ShouldBe(read);
            page.CurrentPagingState.ShouldBe(pagingState);



            // ASK FOR THEM ALL
            pageSize = 100;
            rawInput2 = $"{{'input': {{'pageSize':'{pageSize}' }} }}";
            gqlInputs2 = rawInput2.ToInputs();
            query2 = @"query Q($input: BlogsQueryInput!) {
                  blogs(input: $input)
                }";
            runResult2 = ExecuteQuery(query2, gqlInputs2, root: null, userContext: GraphQLUserContext);
            bRun2 = runResult2.Errors?.Any() == true;
            Assert.IsFalse(bRun2);

            slice = blogEntries.Skip(0).Take(pageSize).ToList();
            foreach (var item in slice)
            {
                item.EnableDeepCompare = true;
            }
            result = (Dictionary<string, object>)runResult2.Data;
            page = (IPage<Blog>)result["blogs"];

            read = page.ToList();
            slice.ShouldBe(read);
            page.Count.ShouldBe(slice.Count);

        }

        [TestMethod]
        public void blog_add_query_request()
        {
            var d = AutofacStoreFactory.AutofacContainer;

            var dd = AutofacStoreFactory.Resolve<IMutationFieldRecordRegistration>();
            var cc = AutofacStoreFactory.Resolve<IMutationFieldRecordRegistrationStore>();
            var timeStamp = DateTime.UtcNow;
            var tsS = timeStamp.ToString(JsonDocumentWriter.JsonSerializerSettings.DateFormatString);

            var simpleTS = DateTime.Parse(tsS, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);


            Blog blog = new Blog()
            {
                Id = Guid.NewGuid(),
                Categories = new List<string>() {"c1", "c2"},
                Tags = new List<string>() {"t1", "t2"},
                MetaData = new BlogMetaData() {Category = "c0", Version = "1.0.0.0"},
                Data = "This is my blog",
                TimeStamp = simpleTS,
                Summary = "My Summary",
                Title = "My Title"
            };
            var jsonBlog = JsonDocumentWriter.SerializeObjectSingleQuote(blog);

            var rawInput = $"{{'input': {jsonBlog} }}";

            var gqlInputs = rawInput.ToInputs();
            var mutation = @"
                mutation Q($input: BlogMutationInput!) {
                  blog(input: $input)
                }";

            var expected = @"{'blog':true}";
            AssertQuerySuccess(mutation, expected, gqlInputs, root: null, userContext: GraphQLUserContext);


            rawInput =
                $"{{'input': {{'id':'{blog.Id.ToString()}' }} }}";
            gqlInputs = rawInput.ToInputs();
            var query = @"
                query Q($input: BlogQueryInput!) {
                  blog(input: $input)
                }";


            var runResult = ExecuteQuery(query,  gqlInputs, root: null, userContext: GraphQLUserContext);
            bool bRun = runResult.Errors?.Any() == true;
            Assert.IsFalse(bRun);

            Dictionary<string, object> data = (Dictionary<string, object>) runResult.Data;
            Dictionary<string, object> dataExpected = new Dictionary<string, object> {{"blog", blog}};

            string additionalInfo = null;
            blog.EnableDeepCompare = true;
            data.ShouldBe(dataExpected, additionalInfo);
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



        public ExecutionResult ExecuteQuery(
         string query,
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

            return runResult;
        }

    }
}
