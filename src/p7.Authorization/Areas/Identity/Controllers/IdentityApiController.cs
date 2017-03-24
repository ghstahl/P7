using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace p7.Authorization.Areas.Identity.Controllers
{
    [Area("Api")]
    [Route("api/[controller]")]
 
    public class IdentityApiController : ControllerBase
    {
        [HttpGet]
        public ActionResult Get()
        {
            return new JsonResult(User.Claims.Select(
                c => new { c.Type, c.Value }));
        }
    }
}