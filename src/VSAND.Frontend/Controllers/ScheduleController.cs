using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace VSAND.Frontend.Controllers
{
    public class ScheduleController : Controller
    {
		[Route ("schedule/")]
        public IActionResult Index()
        {
            return View();
        }

		#region school schedule views
		[Route("school/{school}/schedule")]
		public IActionResult School()
		{
			return View();
		}
		#endregion

		#region sport schedule views
		[Route("{sport}/schedule")]
		public IActionResult Schedule()
		{
			return View();
		}

		[Route("{sport}/schedule/group/{groupid:int}")]
		public IActionResult Group()
		{
			return View();
		}

		[Route("{sport}/schedule/group/season/{year:int}/{groupid:int}")]
		public IActionResult GroupYear()
		{
			return View();
		}
		#endregion

		#region team schedule views
		[Route("{sport}/{school}/schedule")]
		public IActionResult Team()
		{
			return View();
		}

		[Route("{sport}/{school}/schedule/season/{year:int}")]
		public IActionResult TeamYear()
		{
			return View();
		}
		#endregion
	}
}