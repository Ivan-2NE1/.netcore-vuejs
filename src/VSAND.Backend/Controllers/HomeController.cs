using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VSAND.Backend.Models;

namespace VSAND.Backend.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            //TODO: Identify the various dashboard views we need to build for different types of users (SysAdmin, Editor, Staff, AD, Coach)
            return View("DashboardAdmin");
            //return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
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
