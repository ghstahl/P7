using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace P7.Filters
{
    public class AuthActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result =  new ChallengeResult();
            }
            else
            {
                var result = from claim in context.HttpContext.User.Claims
                             where claim.Type == ClaimTypes.NameIdentifier
                             select claim;
                if (!result.Any())
                {
                    context.Result = new UnauthorizedResult();
                }
            }

            base.OnActionExecuting(context);
        }
    }
}