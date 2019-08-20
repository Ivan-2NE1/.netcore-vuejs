using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VSAND.Common;
using VSAND.Services.Data.Manage.Users;
using VSAND.Services.Data.Schools;

namespace VSAND.Backend.Controllers
{
    [Route("Schools")]
    public class SchoolsController : Controller
    {
        private ISchoolService _schoolService;
        private IUserService _userService;
        public SchoolsController(ISchoolService schoolService, IUserService userService)
        {
            _schoolService = schoolService;
            _userService = userService;
        }

        public async Task<IActionResult> Index([FromQuery(Name = "name")] string name, [FromQuery(Name = "city")] string city, [FromQuery(Name = "state")] string state, [FromQuery(Name = "core")] bool coreCoverage, [FromQuery(Name = "pg")] int pageNumber = 1)
        {
            // default core coverage to true when nothing else is set in the search params
            if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(city) && string.IsNullOrEmpty(state))
            {
                coreCoverage = true;
            }
            ViewData["SearchName"] = name;
            ViewData["SearchCity"] = city;
            ViewData["SearchState"] = state;
            ViewData["SearchCoreCoverage"] = coreCoverage;
            var pagedResult = await _schoolService.SearchAsync(name, city, state, coreCoverage, 50, pageNumber);

            ViewData["PagedResultMessage"] = PaginationHelp.PaginationMessage(pagedResult.PageNumber, pagedResult.PageSize, pagedResult.TotalResults);

            return View(pagedResult);
        }

        [Route("{schoolId}")]
        public async Task<IActionResult> School([FromRoute] int schoolId)
        {
            var oSchool = await _schoolService.GetSchoolAsync(schoolId);
            if (oSchool != null)
            {
                ViewData["SchoolName"] = oSchool.Name;
                ViewData["SchoolId"] = oSchool.SchoolId;

                var masterAccountId = await _userService.GetMasterAccountIdAsync(schoolId);

                ViewData["MasterAccountId"] = masterAccountId;

                // check if a master account needs to be created
                ViewData["MasterAccountUsername"] = "";
                if (masterAccountId == 0)
                {
                    var userNameRgx = new Regex("[^A-Z0-9a-z]");

                    var suggestedUserName = userNameRgx.Replace(oSchool.Name, "");

                    var foundUser = await _userService.GetUserAsync(suggestedUserName);
                    if (!foundUser.Success)
                    {
                        ViewData["MasterAccountUsername"] = suggestedUserName;
                    }
                }

                return View(oSchool);
            }

            Response.StatusCode = 404;
            return View();
        }

        [Route("{schoolId}/Teams/")]
        public async Task<IActionResult> Teams([FromRoute] int schoolId)
        {
            var oSchool = await _schoolService.GetFullSchoolAsync(schoolId);
            if (oSchool != null)
            {
                ViewData["SchoolName"] = oSchool.Name;
                ViewData["SchoolId"] = oSchool.SchoolId;
                return View(oSchool);
            }

            Response.StatusCode = 404;
            return View();
        }

        [ActionName("Custom Codes")]
        [Route("{schoolId}/CustomCodes/")]
        public async Task<IActionResult> CustomCodes([FromRoute] int schoolId)
        {
            var oSchool = await _schoolService.GetFullSchoolAsync(schoolId);
            if (oSchool != null)
            {
                ViewData["SchoolName"] = oSchool.Name;
                ViewData["SchoolId"] = oSchool.SchoolId;
                return View("CustomCodes", oSchool);
            }

            Response.StatusCode = 404;
            return View();
        }
    }
}