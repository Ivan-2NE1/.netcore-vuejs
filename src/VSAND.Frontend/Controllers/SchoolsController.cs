using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using VSAND.Services.Data.Schools;

namespace VSAND.Frontend.Controllers
{
    public class SchoolsController : Controller
    {
        private ISchoolService _schoolService;

        public SchoolsController(ISchoolService schoolService)
        {
            _schoolService = schoolService;
        }

        [Route ("school")]
        public async Task<IActionResult> Index()
        {
            var model = await _schoolService.GetFrontEndDisplayListCachedAsync();
            return View(model);
        }

		[Route ("school/{schoolid:int}")]
		public async Task<IActionResult> School([FromRoute(Name = "schoolid")]int schoolId, [FromQuery(Name = "viewdate")] DateTime? viewDate)
		{
            string schoolSlug = HttpContext.Items["SchoolSlug"].ToString();
            string basePath = "/school/" + schoolSlug;
            ViewData["BasePath"] = basePath;

            DateTime defaultSchedDate = DateTime.Now;
            if (viewDate.HasValue)
            {
                defaultSchedDate = viewDate.Value;
            }
            ViewData["ViewDate"] = defaultSchedDate.ToString("MM/dd/yyyy");

            var oSchool = await _schoolService.GetFullSchoolAsync(schoolId);
            ViewData["SchoolId"] = schoolId;
			return View(oSchool);
		}
    }
}