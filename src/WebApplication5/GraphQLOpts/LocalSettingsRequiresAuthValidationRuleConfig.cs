using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using P7.Core.Providers;
using P7.Core.Settings;
using P7.GraphQLCore.Validators;

namespace WebApplication5.GraphQLOpts
{
    public class LocalSettingsRequiresAuthValidationRuleConfig : IRequiresAuthValidationRuleConfig
    {
        private readonly IOptions<FiltersGraphQLConfig> _settings;
        private readonly ILogger<LocalSettingsRequiresAuthValidationRuleConfig> _logger;
        public LocalSettingsRequiresAuthValidationRuleConfig(IOptions<FiltersGraphQLConfig> settings,
            ILogger<LocalSettingsRequiresAuthValidationRuleConfig> logger)
        {
            _settings = settings;
            _logger = logger;
        }
        public bool AllowAccess(ClaimsPrincipal claimsPrincipal, string fieldName)
        {
            throw new NotImplementedException();
        }
    }
}
