using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using p7.main.Models;
using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace p7.main.Areas.Riot.Controllers
{
   

    [Area("Riot")]
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
           
            return View();
        }

    }
}
