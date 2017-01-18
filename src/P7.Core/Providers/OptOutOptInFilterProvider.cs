using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using P7.Core.Reflection;
using P7.Core.Settings;

namespace P7.Core.Providers
{
    public class OptOutOptInFilterProvider : IFilterProvider
    {
        private readonly ILogger<OptOutOptInFilterProvider> _logger;

        private IOptions<FiltersConfig> _settings;
        private IServiceProvider _serviceProvider;
        private static readonly object locker = new object();
        private static Dictionary<string, List<FilterItem>> ActionFilterMap = new Dictionary<string, List<FilterItem>>();
        private static Dictionary<string, FilterItem> TypeToFilterItem = new Dictionary<string, FilterItem>();

        public OptOutOptInFilterProvider(IServiceProvider serviceProvider, IOptions<FiltersConfig> settings,
            ILogger<OptOutOptInFilterProvider> logger)
        {
            _logger = logger;
            _settings = settings;
            _serviceProvider = serviceProvider;
            FrontLoadFilterItems();
        }

        private void FrontLoadFilterItems()
        {
            _logger.LogInformation("Enter");
            try
            {
                if (_settings.Value.SimpleMany == null)
                {
                    throw new Exception("_settings.Value.SimpleMany cannot be NULL.  Check your appsettings.json.");
                }
                FilterItem filterItem;
                if (_settings.Value.SimpleMany.OptOut != null)
                {
                    foreach (var record in _settings.Value.SimpleMany.OptOut)
                    {
                        _logger.LogInformation("Processing OptOut Record: {0}", record);
                        try
                        {
                            filterItem = CreateFilterItem(record.Filter);
                        }
                        catch (Exception e)
                        {
                            throw new Exception(
                                $"fiter:{record.Filter}, seems to be bad, are you sure it is referenced.", e);
                        }
                        TypeToFilterItem.Add(record.Filter, filterItem);
                    }
                }

                if (_settings.Value.SimpleMany.OptIn != null)
                {
                    foreach (var record in _settings.Value.SimpleMany.OptIn)
                    {
                        _logger.LogInformation("Processing OptIn Record: {0}", record);
                        try
                        {
                            filterItem = CreateFilterItem(record.Filter);
                        }
                        catch (Exception e)
                        {
                            throw new Exception(
                                $"fiter:{record.Filter}, seems to be bad, are you sure it is referenced.", e);
                        }
                        TypeToFilterItem.Add(record.Filter, filterItem);
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

        private FilterItem CreateFilterItem(string filterType)
        {
            var type = TypeHelper<Type>.GetTypeByFullName(filterType);
            var typeFilterAttribute = new TypeFilterAttribute(type) {Order = 0};
            var filterDescriptor = new FilterDescriptor(typeFilterAttribute, 0);
            var filterInstance = _serviceProvider.GetService(type);
            var filterMetaData = (IFilterMetadata) filterInstance;
            var fi = new FilterItem(filterDescriptor, filterMetaData);
            return fi;
        }

        private List<FilterItem> FetchFilters(FilterProviderContext context)
        {
            lock (locker)
            {
                List<FilterItem> filters;
                if (!ActionFilterMap.TryGetValue(context.ActionContext.ActionDescriptor.DisplayName, out filters))
                {
                    filters = new List<FilterItem>();

                    FilterItem filterItem;

                    // the following are generic optin and optout
                    foreach (var record in _settings.Value.SimpleMany.OptOut)
                    {
                        var match = record.RouteTree.ContainsMatch(context);
                        if (!match)
                        {
                            filterItem = TypeToFilterItem[record.Filter];
                            filters.Add(filterItem);
                        }
                    }

                    foreach (var record in _settings.Value.SimpleMany.OptIn)
                    {
                        var match = record.RouteTree.ContainsMatch(context);
                        if (match)
                        {
                            filterItem = TypeToFilterItem[record.Filter];
                            filters.Add(filterItem);
                        }
                    }
                    ActionFilterMap.Add(context.ActionContext.ActionDescriptor.DisplayName, filters);
                }
                return filters;
            }
        }


        //all framework providers have negative orders, so ours will come later
        public void OnProvidersExecuting(FilterProviderContext context)
        {
            ControllerActionDescriptor cad = (ControllerActionDescriptor) context.ActionContext.ActionDescriptor;
            System.Diagnostics.Debug.WriteLine("Controller: " + cad.ControllerTypeInfo.FullName);
            System.Diagnostics.Debug.WriteLine("ActionName: " + cad.ActionName);
            System.Diagnostics.Debug.WriteLine("DisplayName: " + cad.DisplayName);
            System.Diagnostics.Debug.WriteLine("Area: " + context.ActionContext.RouteData.Values["area"]);

            var filters = FetchFilters(context);
            foreach (var filter in filters)
            {
                context.Results.Add(filter);
            }

        }

        public void OnProvidersExecuted(FilterProviderContext context)
        {
        }

        public int Order => 0;
    }
}