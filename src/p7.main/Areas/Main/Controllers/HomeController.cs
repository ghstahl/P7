using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using p7.main.Models;
using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace p7.main.Areas.Main.Controllers
{
    class HomeControllerLoggingEvents
    {
        public const int EMULTATE_ERROR = 1000;

    }

    [Area("Main")]
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

        public IActionResult Index()
        {
            Logger.LogInformation("Hello from the Home Index Controller");
            Session.SetString("Test", "Ben Rules!");
            return View();
        }

        public IActionResult About()
        {
            Logger.LogInformation("Hello from the Home About Controller");
            var message = Session.GetString("Test");
            ViewData["Message"] = message;

            var result = HttpContext.User.Claims.Select(
              c => new ClaimType { Type = c.Type, Value = c.Value });

            return View(result);
        }

        public IActionResult Contact()
        {
            Logger.LogInformation("Hello from the Home Contact Controller");
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            Logger.LogInformation("Hello from the Home Error Controller");
            return View();
        }
        public IActionResult EmulateError()
        {
            try
            {
                throw new Exception("Let's assume that there is an error in the controller action.");

            }
            catch (Exception e)
            {
                Log.Logger.Error(e,"in eumulate error");
                Logger.LogError(HomeControllerLoggingEvents.EMULTATE_ERROR,e, "in eumulate error");
                throw;
            }
        }
    }
}
