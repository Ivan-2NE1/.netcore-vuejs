using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace VSAND.Frontend.Controllers
{
	public class PowerpointsController : Controller
	{

		[Route ("power-points/")]
		public IActionResult Index()
		{
			return View();
		}

		[Route ("{sport}/power-points")]
		public IActionResult Sport()
		{
			return View();
		}

		[Route ("{sport}/power-points/year/{year:int}")]
		public IActionResult SportYear()
		{
			return View();
		}

		[Route ("{sport}/power-points/conference/{conference}")]
		public IActionResult SportConference()
		{
			return View();
		}

		[Route ("{sport}/power-points/grouping/{groupid:int}")]
		public IActionResult Grouping()
		{
			return View();
		}
	}
}