using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

namespace P7.Filters
{
    public class AuthApiActionFilter : ActionFilterAttribute
    {
        private static IConfigurationRoot _configurationRoot;
        public AuthApiActionFilter(IConfigurationRoot configurationRoot)
        {
            _configurationRoot = configurationRoot;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
            }
            base.OnActionExecuting(context);
        }
    }
}