using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using p7.external.spa.Models;

namespace p7.external.spa.Areas.ExtSpa.Controllers
{
    [Area("ExtSPA")]
    public class HomeController : Controller
    {
        private ILogger Logger { get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession Session => _httpContextAccessor.HttpContext.Session;



        public HomeController(IHttpContextAccessor httpContextAccessor,
            ILogger<HomeController> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            Logger = logger;
        }

        public IActionResult Index(string id)
        {
            Logger.LogInformation("Hello from the External SPA Home Index Controller");
            var model = new HtmlString($"<div id={id}>Well Hello There</div>");

            return View(new SectionValue(){Value = model});
        }

    }
}
