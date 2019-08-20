using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VSAND.Backend.Controllers;
using VSAND.Common;
using VSAND.Data.Repositories;
using VSAND.Services.Data.Manage.Users;
using VSAND.Services.Files;

namespace VSAND.Backend.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Route("[area]/[controller]")]
    public class UsersController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IUnitOfWork _uow;
        private readonly IExcelService _excelService;

        public UsersController(IUserService userService, IUnitOfWork uow, IExcelService excelService) : base(userService)
        {
            _userService = userService;
            _uow = uow;
            _excelService = excelService;
        }

        public async Task<IActionResult> Index([FromQuery(Name = "username")] string username, [FromQuery(Name = "email")] string email, [FromQuery(Name = "first")] string firstName, [FromQuery(Name = "last")] string lastName, [FromQuery(Name = "admin")] bool isAdmin, [FromQuery(Name = "school")] int schoolId, [FromQuery(Name = "publication")] int publicationId, [FromQuery(Name = "pg")] int pageNumber = 1)
        {
            ViewData["SearchUsername"] = username;
            ViewData["SearchEmail"] = email;
            ViewData["SearchFirstName"] = firstName;
            ViewData["SearchLastName"] = lastName;
            ViewData["SearchIsAdmin"] = isAdmin;
            ViewData["SearchSchoolId"] = schoolId;
            ViewData["SearchPublicationId"] = publicationId;

            var pagedResult = await _userService.SearchAsync(username, email, firstName, lastName, isAdmin, schoolId, publicationId, 50, pageNumber);

            ViewData["PagedResultMessage"] = PaginationHelp.PaginationMessage(pagedResult.PageNumber, pagedResult.PageSize, pagedResult.TotalResults);

            return View(pagedResult);
        }

        [HttpGet("{applicationUserId:int}")]
        public async Task<IActionResult> Edit(int applicationUserId)
        {
            var result = await _userService.GetUserFullAsync(applicationUserId);
            if (result.Success == false)
            {
                return RedirectToAction("Index");
            }

            ViewData["UserRoleCategories"] = await _uow.AppxUserRoles.GetRoleCategoriesAsync();

            var applicationUser = result.obj;
            return View(applicationUser);
        }

        #region School Master Accounts
        [HttpGet("SchoolMasterAccounts")]
        public IActionResult SchoolMasterAccounts()
        {
            return View();
        }

        [HttpGet("SchoolMasterAccounts/Download")]
        public async Task<ActionResult> SchoolMasterAccountsDownload()
        {
            var masterAccounts = await _userService.GetMasterAccounts();

            var fileName = "School Master Accounts.xlsx";
            var fileBytes = _excelService.SchoolMasterAccountFile(masterAccounts, fileName);
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HttpPost("SchoolMasterAccounts/Upload")]
        public async Task<IActionResult> SchoolMasterAccountsUpload([FromForm] IFormFile spreadsheet)
        {
            var updatedBy = ApplicationUser.AppxUser;
            var result = await _userService.UpdateSchoolMasterAccountsFromFile(updatedBy, spreadsheet);

            return View("SchoolMasterAccounts", result);
        }
        #endregion
    }
}