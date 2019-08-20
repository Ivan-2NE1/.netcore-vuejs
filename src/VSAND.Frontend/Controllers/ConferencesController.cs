using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace VSAND.Frontend.Controllers
{
    public class ConferencesController : Controller
    {
		[Route ("conference/")]
        public IActionResult Index()
        {
            return View();
        }

		[Route ("conference/{conference}")]
		public IActionResult Conference()
		{
			return View();
		}
    }
}