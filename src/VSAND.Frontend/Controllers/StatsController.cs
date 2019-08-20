using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace VSAND.Frontend.Controllers
{
    public class StatsController : Controller
    {
		[Route ("stats/")]
        public IActionResult Index()
        {
            return View();
        }

        [Route ("{sport}/stats")]
		public IActionResult Sport()
		{
			return View();
		}

		[Route ("{sport}/stats/{category}")]
		public IActionResult SportCategory()
		{
			return View();
		}

		[Route ("{sport}/stats/conference/{conference}")]
		public IActionResult SportConference()
		{
			return View();
		}

		[Route ("{sport}/stats/year/{year:int}")]
		public IActionResult SportYear()
		{
			return View();
		}
    }
}