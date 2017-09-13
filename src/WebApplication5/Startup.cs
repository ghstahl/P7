using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using Autofac;
using GraphQL.Language.AST;
using IdentityServer4;
using IdentityServer4.Hosting;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using p7.Authorization.Areas.Identity.Controllers;
using p7.Authorization.Data;
using p7.Authorization.Models;
using p7.Services;
using P7.BlogStore.Hugo.Extensions;
using P7.Core;
using P7.Core.FileProviders;
using P7.Core.Identity;
using Serilog;
using P7.Core.Startup;
using P7.Core.IoC;
using P7.Core.TagHelpers;
using P7.Filters;
using P7.GraphQLCore;
using P7.GraphQLCore.Stores;
using P7.HugoStore.Core;
using P7.IdentityServer4.BiggyStore;
using P7.IdentityServer4.BiggyStore.Extensions;
using P7.IdentityServer4.Common;

using P7.IdentityServer4.Common.ExtensionGrantValidator;
using P7.IdentityServer4.Common.Middleware;
using P7.RazorProvider.Store.Hugo.Extensions;
using P7.RazorProvider.Store.Hugo.Interfaces;
using P7.RazorProvider.Store.Hugo.Models;
using P7.SimpleDocument.Store;
using Module = Autofac.Module;

namespace WebApplication5
{

    // this gates all apis with not only being authenticated, but have one of the following claims.
    class MyAuthApiClaimsProvider : IAuthApiClaimsProvider
    {
        public static string LocalClientIdValue => "local";
        public async Task<List<Claim>> FetchClaimsAsync()
        {
            var claims = new List<Claim>
            {
                new Claim("client_id", MyAuthApiClaimsProvider.LocalClientIdValue),
                new Claim("client_id", "resource-owner-client")
            };
            return claims;
        }
    }

    // this seeds all local identities with a claim {client_id:local}
    // this is so that downstream api filters can let identites of this type in.
    // we let in bearer tokens from external systems that we require to have certain claims, in our case client_id.
    class MyPostAuthClaimsProvider : IPostAuthClaimsProvider
    {
        public async Task<List<Claim>> FetchClaims(ClaimsPrincipal principal)
        {
            var claims = new List<Claim> {new Claim("client_id", MyAuthApiClaimsProvider.LocalClientIdValue) };
            return claims;
        }
    }

    public class MyIdentityServer4BiggyAutofacModule : Module
    {
        private static string TenantId = "02a6f1a2-e183-486d-be92-658cd48d6d94";
      
        protected override void Load(ContainerBuilder builder)
        {
            var env = P7.Core.Global.HostingEnvironment;
            var dbPath = Path.Combine(env.ContentRootPath, "App_Data/identityserver4");
            Directory.CreateDirectory(dbPath);
            builder.AddIdentityServer4BiggyConfiguration(dbPath);

            dbPath = Path.Combine(env.ContentRootPath, "App_Data/blogstore");
            Directory.CreateDirectory(dbPath);
            builder.AddBlogStoreBiggyConfiguration(dbPath, TenantId);


            dbPath = Path.Combine(env.ContentRootPath, "App_Data/razorlocationstore");
            Directory.CreateDirectory(dbPath);
            builder.AddRazorLocationStoreBiggyConfiguration(dbPath, TenantId);

            builder.RegisterType<InMemoryGraphQLFieldAuthority>()
                .As<IGraphQLFieldAuthority>()
                .SingleInstance();

            builder.RegisterType<MyPostAuthClaimsProvider>().As<IPostAuthClaimsProvider>().SingleInstance();
            builder.RegisterType<MyAuthApiClaimsProvider>().As<IAuthApiClaimsProvider>().SingleInstance();
            
        }
    }

    public class Startup
    {

        private readonly IHostingEnvironment _hostingEnvironment;

        public Startup(IHostingEnvironment env)
        {
            _hostingEnvironment = env;
            P7.Core.Global.HostingEnvironment = _hostingEnvironment;

            var appDataPath = Path.Combine(env.ContentRootPath, "App_Data");

            var RollingPath = Path.Combine(env.ContentRootPath, "logs/myapp-{Date}.txt");
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.RollingFile(RollingPath)
                .WriteTo.LiterateConsole()
                .CreateLogger();
            Log.Information("Ah, there you are!");


            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings-filters.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings-filters-graphql.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();
            }


            builder.AddEnvironmentVariables();
            Configuration = builder.Build();

            // Initialize the global configuration static
            GlobalConfigurationRoot.Configuration = Configuration;
        }

        public IContainer ApplicationContainer { get; private set; }
        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // https://identityserver4.readthedocs.io/en/release/quickstarts/0_overview.html
            // NOTE: AddTemporarySigningCredential generates new signing keys on restart, so access_tokens only work for the life of the app.
            
            var identityServerBuilder = services.AddIdentityServer()
                .AddTemporarySigningCredential()
                .AddSecretParser<ClientAssertionSecretParser>()
                .AddSecretValidator<PrivateKeyJwtSecretValidator>()
                .AddExtensionGrantValidator<PublicRefreshTokenExtensionGrantValidator>();
//                .AddEndpoint<CustomTokenEndpoint>(EndpointName.Token);


            services.TryAddSingleton(typeof(IStringLocalizerFactory), typeof(ResourceManagerStringLocalizerFactory));
            services.AddLocalization();

            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            services
                .AddScoped
                <Microsoft.AspNetCore.Identity.IUserClaimsPrincipalFactory<ApplicationUser>, 
                AppClaimsPrincipalFactory<ApplicationUser>>();

            services.AddAntiforgery(opts => opts.HeaderName = "X-XSRF-Token");
            services.AddMvc(opts =>
            {
                opts.Filters.AddService(typeof(AngularAntiforgeryCookieResultFilter));
            }).AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            });
            services.AddTransient<AngularAntiforgeryCookieResultFilter>();

            services.AddAuthorization();
        

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

            services.AddLogging();
            services.AddWebEncoders();
            services.AddCors();

            services.AddDistributedMemoryCache();
            services.AddSession();

            services.AddTransient<ClaimsPrincipal>(
                s => s.GetService<IHttpContextAccessor>().HttpContext.User);

            // AutomaticChallenge = false, we own the logic via our filters what happens with the redirect
            // this is neccesary for the api work where we simply want to return a 401 and not get redirected to login
            services.Configure<IdentityOptions>(options =>
            {
                options.Cookies.ApplicationCookie.LoginPath =
                    new Microsoft.AspNetCore.Http.PathString("/Identity/Account/Login");
                options.Cookies.ApplicationCookie.LogoutPath =
                    new Microsoft.AspNetCore.Http.PathString("/Identity/Account/LogOff");
                options.Cookies.ApplicationCookie.AutomaticChallenge = false;
            });
            services.AddAllConfigureServicesRegistrants(Configuration);
            services.AddDependenciesUsingAutofacModules();
            var serviceProvider = services.BuildServiceProvider(Configuration);
            P7.Core.Global.ServiceProvider = serviceProvider;
            return serviceProvider;

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            IApplicationLifetime appLifetime)
        {
            LoadRazorProviderData();
            LoadIdentityServer4Data();
            LoadGraphQLAuthority();
            var dd = P7.Core.Global.ServiceProvider.GetServices<IQueryFieldRecordRegistration>();
            var vv = P7.Core.Global.ServiceProvider.GetService<IQueryFieldRecordRegistrationStore>();
            var v2 = P7.Core.Global.ServiceProvider.GetService<IPersistedGrantStore>();


            var supportedCultures = new List<CultureInfo>
            {
                new CultureInfo("en-US"),
                new CultureInfo("en-AU"),
                new CultureInfo("en-GB"),
                new CultureInfo("es-ES"),
                new CultureInfo("ja-JP"),
                new CultureInfo("fr-FR"),
                new CultureInfo("zh"),
                new CultureInfo("zh-CN")
            };
            var options = new RequestLocalizationOptions
            {
                //     RequestCultureProviders = new List<IRequestCultureProvider>(),
                DefaultRequestCulture = new RequestCulture("en-US"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            };
            app.UseRequestLocalization(options);



            var version = typeof(Startup).GetTypeInfo()
                .Assembly
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                .InformationalVersion;
            if (env.EnvironmentName == "Development")
            {
                version += "." + Guid.NewGuid().ToString().GetHashCode();
            }

            P7TagHelperBase.Version = version;
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            // Add Serilog to the logging pipeline
            loggerFactory.AddSerilog();
            // Ensure any buffered events are sent at shutdown
            appLifetime.ApplicationStopped.Register(Log.CloseAndFlush);

            app.UseIdentity()
                .UseDevAuthAuthentication(
                    new DevAuthOptions()
                    {
                        ConsumerKey = "uWkHwFNbklXgsLHYzLfRXcThw",
                        ConsumerSecret = "2kyg9WdUiJuU2HeWYJEuvwzaJWoweLadTgG3i0oHI5FeNjD5Iv"
                    })
                .UseTwitterAuthentication(
                    new TwitterOptions()
                    {
                        ConsumerKey = "uWkHwFNbklXgsLHYzLfRXcThw",
                        ConsumerSecret = "2kyg9WdUiJuU2HeWYJEuvwzaJWoweLadTgG3i0oHI5FeNjD5Iv"
                    });
            app.AddAllConfigureRegistrants();
            /*
            CookieAuthenticationOptions cookieAuthenticationOptions = new CookieAuthenticationOptions
            {
                AccessDeniedPath = new PathString("/Account/Forbidden/"),
                LoginPath = new PathString("/Identity/Account/Login"),
                LogoutPath = new PathString("/Identity/Account/LogOff"),
                AuthenticationScheme = "Cookies",
                AutomaticAuthenticate = true,
                AutomaticChallenge = false,
                Events = new CookieAuthenticationEvents()
                {
                    OnRedirectToLogin = ctx =>
                    {
                        if (!ctx.Request.Path.StartsWithSegments("/api"))
                        {
                            ctx.Response.Redirect(ctx.RedirectUri);
                        }

                        return Task.FromResult(0);
                    }
                }
            };
            app.UseCookieAuthentication(cookieAuthenticationOptions);
*/
            app.UsePublicRefreshToken();
            app.UseIdentityServer();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            var contentTypeProvider = new FileExtensionContentTypeProvider();
           
            contentTypeProvider.Mappings.Add(".tag", "text/plain");

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new CbvPhysicalFileProvider(env.WebRootPath),
                RequestPath = new PathString("/cb-v"),
                ServeUnknownFileTypes = true,
                ContentTypeProvider = contentTypeProvider
            });


            var root = env.ContentRootFileProvider;
            var rewriteOptions = new RewriteOptions()
                .AddIISUrlRewrite(root, "IISUrlRewrite.config");
            app.UseRewriter(rewriteOptions);

            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715
            app.UseSession();

            // this gates any bearer token that comes in that does not have the 'abitrary'
            app.UseIdentityServerAuthentication(new IdentityServerAuthenticationOptions
            {
                Authority = "http://localhost:7791",
                RequireHttpsMetadata = false,
                EnableCaching = false,
                AllowedScopes = {"arbitrary"} 
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute("areaRoute", "{area:exists}/{controller=Admin}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{area=Main}/{controller=Home}/{action=Index}/{id?}");
            });
           

        }
        private async Task LoadGraphQLAuthority()
        {
            var graphQLFieldAuthority = P7.Core.Global.ServiceProvider.GetServices<IGraphQLFieldAuthority>().FirstOrDefault();

            await graphQLFieldAuthority.AddClaimsAsync(OperationType.Mutation, "/blog", new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,""),
                new Claim("client_id","resource-owner-client"),
            });
        }

        private async Task LoadRazorProviderData()
        {
            var store = P7.Core.Global.ServiceProvider.GetServices<IRazorLocationStore>().FirstOrDefault();
            var now = DateTime.UtcNow;
            await store.InsertAsync(new SimpleDocument<RazorLocation>()
            {
                MetaData = new MetaData() {Category = "RazorLocation", Version = "1.0.0.0"},
                Document = new RazorLocation()
                {
                    Location = "/ExtSPA/Home/Index",
                    Content =
                        "@using P7.External.SPA.Models @model SectionValue< div id = \"spaSection\" >@Model.Value</ div >",
                    LastModified = now,
                    LastRequested = now
                }
            });
        }

        private async Task LoadIdentityServer4Data()
        {
            var fullClientStore = P7.Core.Global.ServiceProvider.GetServices<IFullClientStore>().FirstOrDefault();

            await fullClientStore.InsertClientAsync(new Client
            {
                ClientId = "client",
                AllowedGrantTypes = GrantTypes.ClientCredentials,

                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },
                AllowedScopes = { "arbitrary" }
            });

            await fullClientStore.InsertClientAsync(new Client
            {
                ClientId = "resource-owner-client",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                AllowOfflineAccess = true,
                RefreshTokenUsage = TokenUsage.OneTimeOnly,
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },
                AllowedScopes = { "arbitrary" }
            });



            await fullClientStore.InsertClientAsync(new Client
            {
                ClientId = "public-resource-owner-client",
                AllowedGrantTypes = GrantTypes.List("public_refresh_token"),
                RequireClientSecret = false,
                AllowedScopes = { "arbitrary" }
            });


            var apiResourceList = new List<ApiResource>
            {
                new ApiResource("arbitrary", "Arbitrary Scope")
                {
                    ApiSecrets =
                    {
                        new Secret("secret".Sha256())
                    }
                }
            };

            var resourceStore = P7.Core.Global.ServiceProvider.GetServices<IResourceStore>().FirstOrDefault();
            var adminResourceStore = P7.Core.Global.ServiceProvider.GetServices<IAdminResourceStore>().FirstOrDefault();

            foreach (var apiResource in apiResourceList)
            {
                await adminResourceStore.ApiResourceStore.InsertApiResourceAsync(apiResource);
            }

            var dd = await adminResourceStore.ApiResourceStore.PageAsync(10, null);
        }
    }
}
