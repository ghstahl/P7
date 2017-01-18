using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using P7.Core.Providers;
using P7.Core.Reflection;
using P7.Core.Settings;

namespace P7.Core.Middleware
{
    public class AuthorizeMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IOptions<FiltersConfig> _settings;
        private IServiceProvider _serviceProvider;
        private readonly ILogger<OptOutOptInFilterProvider> _logger;
        private static Dictionary<string, IMiddlewarePlugin> TypeToMiddlewarePlugins = new Dictionary<string, IMiddlewarePlugin>();

        public AuthorizeMiddleware(RequestDelegate next, IServiceProvider serviceProvider, IOptions<FiltersConfig> settings, ILogger<OptOutOptInFilterProvider> logger)
        {
            _next = next;
            _serviceProvider = serviceProvider;
            _settings = settings;
            _logger = logger;
            FrontLoadFilterItems();
        }

        private IMiddlewarePlugin CreateMiddlewareInstance(string filterType)
        {
            var type = TypeHelper<Type>.GetTypeByFullName(filterType);
            var typeFilterAttribute = new TypeFilterAttribute(type) { Order = 0 };
            var filterDescriptor = new FilterDescriptor(typeFilterAttribute, 0);
            var instance = _serviceProvider.GetService(type);
            var iMiddlewarePlugin = (IMiddlewarePlugin)instance;
            return iMiddlewarePlugin;
        }
        private void FrontLoadFilterItems()
        {
            _logger.LogInformation("Enter");
            try
            {
                if (_settings.Value.GlobalPath == null)
                {
                    throw new Exception("_settings.Value.GlobalPath cannot be NULL.  Check your appsettings.json.");
                }
                IMiddlewarePlugin middlewarePluginInstance;
                if (_settings.Value.GlobalPath.OptOut != null)
                {
                    foreach (var record in _settings.Value.GlobalPath.OptOut)
                    {
                        _logger.LogInformation("Processing OptOut Record: {0}", record);
                        try
                        {
                            middlewarePluginInstance = CreateMiddlewareInstance(record.Filter);
                        }
                        catch (Exception e)
                        {
                            throw new Exception(
                                $"fiter:{record.Filter}, seems to be bad, are you sure it is referenced.", e);
                        }
                        TypeToMiddlewarePlugins.Add(record.Filter, middlewarePluginInstance);
                    }
                }

                if (_settings.Value.GlobalPath.OptIn != null)
                {
                    foreach (var record in _settings.Value.GlobalPath.OptIn)
                    {
                        _logger.LogInformation("Processing OptIn Record: {0}", record);
                        try
                        {
                            middlewarePluginInstance = CreateMiddlewareInstance(record.Filter);
                        }
                        catch (Exception e)
                        {
                            throw new Exception(
                                $"fiter:{record.Filter}, seems to be bad, are you sure it is referenced.", e);
                        }
                        TypeToMiddlewarePlugins.Add(record.Filter, middlewarePluginInstance);
                    }
                }

            }
            catch (Exception e)
            {
                _logger.LogCritical(e.Message);
                throw;
            }
            _logger.LogInformation("Exit");
        }
        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Method != "GET")
            {
                await _next(httpContext);
                return;
            }
            var middlewarePlugins = new List<IMiddlewarePlugin>();

            foreach (var record in _settings.Value.GlobalPath.OptIn)
            {
                foreach (var strRegex in record.Paths)
                {

                    Regex myRegex = new Regex(strRegex, RegexOptions.IgnoreCase);
                    string strTargetString = httpContext.Request.Path;
                    var match = myRegex.Matches(strTargetString).Cast<Match>().Any(myMatch => myMatch.Success);
                    if (match)
                    {
                        middlewarePlugins.Add(TypeToMiddlewarePlugins[record.Filter]);
                        break;
                    }
                }
            }
            bool continueToNext = true;

            foreach (var mp in middlewarePlugins)
            {
                continueToNext = mp.Invoke(httpContext);
                if (!continueToNext)
                {
                    break;
                }
            }


            if (continueToNext)
            {
                await _next(httpContext);
            }
        }
    }
    // You may need to install the Microsoft.AspNet.Http.Abstractions package into your project

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthorizeMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthorizeMiddleware>();
        }
    }
}