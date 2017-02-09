using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security.Claims;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Filters;
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

using WebApplication5.Services;
using P7.Core.Startup;
using P7.Core.IoC;
using P7.Core.Localization;
using P7.Core.TagHelpers;
using P7.GraphQLCore;
using Serilog.Exceptions;

namespace WebApplication5
{
    public class Startup
    {


        private readonly IHostingEnvironment _hostingEnvironment;

        public Startup(IHostingEnvironment env)
        {
            _hostingEnvironment = env;
            P7.Core.Global.HostingEnvironment = _hostingEnvironment;

            var RollingPath = Path.Combine(env.ContentRootPath, "logs/myapp-{Date}.txt");
            Log.Logger = new LoggerConfiguration()
                .Enrich.WithExceptionDetails()
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

            services.TryAddSingleton(typeof(IStringLocalizerFactory), typeof(ResourceManagerStringLocalizerFactory));
            services.AddLocalization();

            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

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
            var dd = P7.Core.Global.ServiceProvider.GetServices<IQueryFieldRecordRegistration>();
            var vv = P7.Core.Global.ServiceProvider.GetService<IQueryFieldRecordRegistrationStore>();


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
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{area=Main}/{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
