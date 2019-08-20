using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using VSAND.Frontend.Models;

namespace VSAND.Frontend.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
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

        // Route for latest scores page
        [Route("scores/{year:int?}/{month:int?}/{day:int?}")]
        public IActionResult Scores([FromRoute(Name = "year")] int? year, [FromRoute(Name = "month")] int? month, [FromRoute(Name = "day")] int? day)
        {
            DateTime viewDate = DateTime.Now;
            if (year.HasValue && year.Value > 0)
            {
                if (month.HasValue && month.Value > 0)
                {
                    if (day.HasValue && day.Value > 0)
                    {
                        try
                        {
                            var testDate = new DateTime(year.Value, month.Value, day.Value);
                            viewDate = testDate;
                        }
                        catch (Exception ex)
                        {
                            // don't care!
                        }
                    }
                }
            }

            ViewData["ViewDate"] = viewDate.ToString("MM/dd/yyyy");

            return View();
        }

        // Route for school sports live video page
        [Route ("watch")]
		public IActionResult Watch()
		{
			return View();
		}
    }
}
