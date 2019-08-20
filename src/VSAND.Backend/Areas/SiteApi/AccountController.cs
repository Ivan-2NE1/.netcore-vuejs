using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VSAND.Backend.Controllers;
using VSAND.Data.Entities;
using VSAND.Data.Identity;
using VSAND.Data.ViewModels;
using VSAND.Data.ViewModels.Users;
using VSAND.Services.Data.Manage.Users;

namespace VSAND.Backend.Areas.SiteApi
{
    [Route("SiteApi/[controller]")]
    [ApiController]
    public class AccountController : BaseController
    {
        private readonly IUserService _userService;
        public AccountController(IUserService userService) : base(userService)
        {
            _userService = userService;
        }

        [HttpPut("{applicationUserId}")]
        public async Task<ApiResult<ApplicationUser>> UpdateUser(int applicationUserId, [FromForm] UserEditViewModel viewModel)
        {
            AppxUser updatedBy = ApplicationUser.AppxUser;

            var result = await _userService.UpdateUserAsync(updatedBy, applicationUserId, viewModel);
            return new ApiResult<ApplicationUser>(result);
        }
    }
}