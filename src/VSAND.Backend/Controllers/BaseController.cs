using IdentityModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using VSAND.Data.Identity;
using VSAND.Services.Data.Manage.Users;

namespace VSAND.Backend.Controllers
{
    public class BaseController : Controller
    {
        private ApplicationUser _ApplicationUser;
        public ApplicationUser ApplicationUser
        {
            get {
                if (_ApplicationUser != null)
                {
                    return _ApplicationUser;
                }

                Claim userIdClaim = User.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Subject);
                if (userIdClaim == null)
                {
                    throw new Exception("No User Id claim found");
                }

                _ApplicationUser = _userService.GetUserAsync(int.Parse(userIdClaim.Value)).Result.obj;
                if (_ApplicationUser == null)
                {
                    throw new Exception("Application User is null");
                }

                return _ApplicationUser;
            }
        }

        private readonly IUserService _userService;
        public BaseController(IUserService userService)
        {
            _userService = userService;
        }
    }
}
