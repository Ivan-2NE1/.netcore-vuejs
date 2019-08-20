using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VSAND.Data.ViewModels.SelfProvision;
using VSAND.Services.Data.Manage.Users;
using VSAND.Services.Data.Sports;

namespace VSAND.Backend.Controllers
{
    [Authorize(Policy = "UserFunction.SchoolMasterAccount")]
    public class SelfProvisionController : BaseController
    {
        private readonly ISportService _sportService;
        private readonly IUserService _userService;
        public SelfProvisionController(IUserService userService, ISportService sportService) : base(userService)
        {
            _sportService = sportService;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["ActiveSports"] = await _sportService.GetActiveListAsync();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromForm] SelfProvisionForm viewModel)
        {
            ViewData["ActiveSports"] = await _sportService.GetActiveListAsync();

            if (viewModel.Password != viewModel.ConfirmPassword)
            {
                ViewData["Errors"] = new List<string> { "Passwords do not match." };
                return View(viewModel);
            }

            var createdBy = ApplicationUser.AppxUser;

            var result = await _userService.CreateUserAsync(createdBy, viewModel.Email, viewModel.Password, viewModel.FirstName, viewModel.LastName, viewModel.Phone, viewModel.OtherPhone, createdBy.SchoolId);
            if (result.Success == false)
            {
                ViewData["Errors"] = result.Message.Split(new string[] { "\n" }, StringSplitOptions.None).ToList();
                return View(viewModel);
            }

            // save the user's interested sports
            if (viewModel.Sports != null && viewModel.Sports.Any())
            {
                await _userService.UpdateInterestedSports(result.obj.AppxUser.AdminId, viewModel.Sports);
            }

            // empty the password so it doesn't show in the query string
            viewModel.Password = "";
            viewModel.ConfirmPassword = "";

            return RedirectToAction("Success", viewModel);
        }

        public IActionResult Success(SelfProvisionForm viewModel)
        {
            return View(viewModel);
        }
    }
}