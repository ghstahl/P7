using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using Autofac;
using IdentityServer4;
using IdentityServer4.Hosting;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using p7.Authorization.Data;
using p7.Authorization.Models;
using p7.Services;
using P7.Core;
using P7.Core.FileProviders;
using Serilog;
using P7.Core.Startup;
using P7.Core.IoC;
using P7.Core.TagHelpers;
using P7.GraphQLCore;
using P7.HugoStore.Core;
using P7.IdentityServer4.BiggyStore;
using P7.IdentityServer4.Common;
using P7.IdentityServer4.Common.Endpoints;
using P7.IdentityServer4.Common.ExtensionGrantValidator;
using Module = Autofac.Module;

namespace WebApplication5
{
    class MyBiggyConfiguration : IIdentityServer4BiggyConfiguration
    {
        public string DatabaseName { get; set; }
        public string FolderStorage { get; set; }
    }

    public class MyIdentityServer4BiggyAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var env = P7.Core.Global.HostingEnvironment;
            var identityserver4Path = Path.Combine(env.ContentRootPath, "App_Data/identityserver4");
            Directory.CreateDirectory(identityserver4Path);
            var globalTenantDatabaseBiggyConfig = new TenantDatabaseBiggyConfig();
            globalTenantDatabaseBiggyConfig.UsingFolder(identityserver4Path);
            globalTenantDatabaseBiggyConfig.UsingTenantId(TenantDatabaseBiggyConfig.GlobalTenantId);
            IIdentityServer4BiggyConfiguration biggyConfiguration = new MyBiggyConfiguration()
            {
                FolderStorage = globalTenantDatabaseBiggyConfig.Folder,
                DatabaseName = globalTenantDatabaseBiggyConfig.Database
            };

            builder.Register(c => biggyConfiguration)
                .As<IIdentityServer4BiggyConfiguration>()
                .SingleInstance();

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
            var identityServerBuilder = services.AddIdentityServer()
                .AddTemporarySigningCredential()
                .AddSecretParser<ClientAssertionSecretParser>()
                .AddSecretValidator<PrivateKeyJwtSecretValidator>()
                .AddExtensionGrantValidator<PublicRefreshTokenExtensionGrantValidator>();


            services.TryAddSingleton(typeof(IStringLocalizerFactory), typeof(ResourceManagerStringLocalizerFactory));
            services.AddLocalization();

            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });

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

            services.Configure<IdentityOptions>(options =>
            {
                options.Cookies.ApplicationCookie.LoginPath =
                    new Microsoft.AspNetCore.Http.PathString("/Identity/Account/Login");
                options.Cookies.ApplicationCookie.LogoutPath =
                    new Microsoft.AspNetCore.Http.PathString("/Identity/Account/LogOff");
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
            LoadIdentityServer4Data();
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
            CookieAuthenticationOptions cookieAuthenticationOptions = new CookieAuthenticationOptions
            {
                LoginPath = new PathString("/Identity/Account/Login"),
                LogoutPath = new PathString("/Identity/Account/LogOff")
            };
            app.UseCookieAuthentication(cookieAuthenticationOptions);

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
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new CbvPhysicalFileProvider(env.WebRootPath),
                RequestPath = new PathString("/cb-v")
            });


            var root = env.ContentRootFileProvider;
            var rewriteOptions = new RewriteOptions()
                .AddIISUrlRewrite(root, "IISUrlRewrite.config");
            app.UseRewriter(rewriteOptions);

            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715
            app.UseSession();

            app.UseIdentityServerAuthentication(new IdentityServerAuthenticationOptions
            {
                Authority = "http://localhost:7791",
                RequireHttpsMetadata = false,
                EnableCaching = false,
                AllowedScopes = {"arbitrary"}
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{area=Main}/{controller=Home}/{action=Index}/{id?}");
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
                AllowOfflineAccess = true,
                RefreshTokenUsage = TokenUsage.OneTimeOnly,
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },
                AllowedScopes = { "arbitrary" }
            });


            var apiResourceList = new List<ApiResource>
            {
                new ApiResource("arbitrary", "Arbitrary Scope")
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
