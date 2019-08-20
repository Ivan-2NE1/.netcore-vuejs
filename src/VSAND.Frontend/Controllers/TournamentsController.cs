using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace VSAND.Frontend.Controllers
{
    public class TournamentsController : Controller
    {
		[Route ("tournaments/")]
        public IActionResult Index()
        {
            return View();
        }

		[Route ("tournaments/season/{year:int}")]
		public IActionResult Season()
		{
			return View();
		}

		[Route("{sport}/tournaments/")]
		public IActionResult Sport()
		{
			return View();
		}

		[Route ("{sport}/tournaments/year/{year:int}")]
		public IActionResult SportYear()
		{
			return View();
		}	

		[Route ("tournament/{tournamentid:int}")]
		public IActionResult Brackets()
		{
			return View();
		}

		[Route ("tournament/{tournamentid:int}/bracket/{bracketid:int}")]
		public IActionResult Bracket()
		{
			return View();
		}
    }
}