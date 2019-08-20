using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using VSAND.Data;

namespace VSAND.Backend.Controllers
{
    [Obsolete("Use the AccountController's Login method instead")]
    public class LoginController : Controller
    {
        private static HttpClient _tokenClient = new HttpClient();

        private readonly VsandContext _context;
        private readonly IConfiguration Configuration;
        public LoginController(VsandContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index([FromQuery] string username, [FromQuery] string password)
        {
            var disco = await _tokenClient.GetDiscoveryDocumentAsync(Configuration["AuthenticationUrl"]);
            if (disco.IsError)
            {
                throw new Exception(disco.Error);
            }

            // request token
            var tokenResponse = await _tokenClient.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "vsand.backend.ro",
                ClientSecret = "63E46F9A8F68F04C86B5F44EE280D6C3BA866DBAC1CC3D049C17AD63E65FF6AB",

                UserName = username,
                Password = password
            });

            if (tokenResponse.IsError)
            {
                throw new Exception(tokenResponse.Error);
            }

            IEnumerable<Claim> claims = await GetClaimsAsync(disco, tokenResponse.AccessToken);

            var userIdClaim = claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Subject);
            if (userIdClaim == null)
            {
                throw new Exception("Unknown User Id");
            }
            int userId = int.Parse(userIdClaim.Value);
            var applicationUser = await _context.Users.Include(u => u.AppxUser).FirstOrDefaultAsync(u => u.Id == userId);

            ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            // stuff the identity full of user claims
            identity.AddClaim(new Claim(JwtClaimTypes.Email, applicationUser.Email));
            identity.AddClaim(new Claim(JwtClaimTypes.Name, applicationUser.AppxUser.FirstName + " " + applicationUser.AppxUser.LastName));

            ClaimsPrincipal user = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, user);

            return Redirect("/");
        }

        private static async Task<IEnumerable<Claim>> GetClaimsAsync(DiscoveryResponse disco, string bearerToken)
        {
            if (disco.IsError)
            {
                throw new Exception(disco.Error);
            }

            UserInfoResponse user = await _tokenClient.GetUserInfoAsync(new UserInfoRequest
            {
                Address = disco.UserInfoEndpoint,
                Token = bearerToken
            });

            if (user.IsError)
            {
                throw new Exception(user.Error);
            }

            return user.Claims;
        }
    }
}