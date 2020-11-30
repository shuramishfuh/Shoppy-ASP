using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shop.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace Shop.Controllers
{
    public class HomeController : Controller
    {
        public ILogger<HomeController> Logger1 { get; }

        public HomeController(ILogger<HomeController> logger)
        {
            Logger1 = logger;
        }
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
       public IActionResult Policy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
