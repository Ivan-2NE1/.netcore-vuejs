using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace VSAND.Frontend.Controllers
{
    public class StandingsController : Controller
    {
		[Route ("standings/")]
		public IActionResult Index()
        {
            return View();
        }

		[Route ("{sport}/standings")]
		public IActionResult Sport()
		{
			return View();
		}

		[Route ("{sport}/standings/year/{year:int}")]
		public IActionResult SportYear()
		{
			return View();
		}

		[Route ("{sport}/standings/conference/{conference}")]
		public IActionResult SportConference()
		{
			return View();
		}
    }
}