using IdentityModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using VSAND.Data;
using VSAND.Data.Identity;

namespace VSAND.Backend.Areas.SiteApi
{
    [ApiController]
    [Route("siteapi/[controller]")]
    public class AuthController : ControllerBase
    {
        private ApplicationUser _ApplicationUser;
        public ApplicationUser ApplicationUser
        {
            get
            {
                if (_ApplicationUser != null)
                {
                    return _ApplicationUser;
                }

                Claim userIdClaim = User.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Subject);
                if (userIdClaim == null)
                {
                    throw new Exception("No User Id claim found");
                }

                _ApplicationUser = _context.Users.Include(u => u.AppxUser).FirstOrDefault(u => u.Id == int.Parse(userIdClaim.Value));
                if (_ApplicationUser == null)
                {
                    throw new Exception("Application User is null");
                }

                return _ApplicationUser;
            }
        }

        private readonly VsandContext _context;

        public AuthController(VsandContext context)
        {
            _context = context;
        }

        // GET: siteapi/auth/isadmin
        [HttpGet("isadmin")]
        public bool IsAdmin()
        {
            return ApplicationUser.AppxUser.IsAdmin;
        }
    }
}