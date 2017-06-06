using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace p7.Authorization.Areas.Identity.Controllers
{
    [Area("Api")]
    [Route("api/[controller]")]

    public class AntiforgeryController : ControllerBase
    {
        [Route("ImportantData")]
        [ValidateAntiForgeryToken]
        public async Task PostImportantData()
        {

        }
        [Route("UnimportantData")]
        public async Task PostUnimportantData()
        {

        }
    }
}
