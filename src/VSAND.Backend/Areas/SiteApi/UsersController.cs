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
    [ApiController]
    [Produces("application/json")]
    [Route("SiteApi/[controller]")]
    public class UsersController : BaseController
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService) : base(userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<ApiResult<ApplicationUser>> CreateUser([FromForm] string email, [FromForm] string password, [FromForm] string firstName, [FromForm] string lastName, [FromForm] string phone)
        {
            AppxUser createdBy = ApplicationUser.AppxUser;

            var result = await _userService.CreateUserAsync(createdBy, email, password, firstName, lastName, phone);
            return new ApiResult<ApplicationUser>(result);
        }

        [HttpPut("{applicationUserId}")]
        public async Task<ApiResult<ApplicationUser>> UpdateUser(int applicationUserId, [FromForm] UserEditViewModel viewModel)
        {
            AppxUser updatedBy = ApplicationUser.AppxUser;

            var result = await _userService.UpdateUserWithRolesAsync(updatedBy, applicationUserId, viewModel);
            return new ApiResult<ApplicationUser>(result);
        }

        [HttpPost("SchoolMasterAccount")]
        public async Task<ApiResult<ApplicationUser>> CreateSchoolMasterAccount([FromForm] int schoolId, [FromForm] string username, [FromForm] string password)
        {
            AppxUser createdBy = ApplicationUser.AppxUser;

            var result = await _userService.CreateSchoolMasterAccountAsync(createdBy, schoolId, username, password);
            return new ApiResult<ApplicationUser>(result);
        }
    }
}
