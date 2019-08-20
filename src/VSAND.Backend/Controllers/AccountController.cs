using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using VSAND.Services.Data.Manage.Users;

namespace VSAND.Backend.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IConfiguration Configuration;
        public AccountController(IUserService userService, IConfiguration configuration) : base(userService)
        {
            _userService = userService;
            Configuration = configuration;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }

            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync(Configuration["AuthenticationUrl"]);

            return StartAuthentication(disco, returnUrl);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync(Configuration["AuthenticationUrl"]);

            var returnUrl = new UriBuilder(Request.Scheme, Request.Host.Host, Request.Host.Port ?? 443).Uri.ToString();
            var queryStringVariables = new Dictionary<string, string>
            {
                { "id_token_hint", User.Claims.FirstOrDefault(c => c.Type == "id_token")?.Value ?? "" },
                { "post_logout_redirect_uri", returnUrl }
            };

            var redirectUrl = QueryHelpers.AddQueryString(disco.EndSessionEndpoint, queryStringVariables);
            return Redirect(redirectUrl);
        }

        [ActionName("Access Denied")]
        [HttpGet("[controller]/AccessDenied")]
        public IActionResult AccessDenied()
        {
            return View("AccessDenied");
        }

        [HttpGet("[controller]/Manage")]
        public IActionResult Manage()
        {
            return View(ApplicationUser);
        }

        [AllowAnonymous]
        public async Task<IActionResult> LoginCallback()
        {
            // On the request, we stuffed the return url value into state
            var returnUrl = Request.Form["state"].FirstOrDefault();
            var idToken = Request.Form["id_token"].FirstOrDefault();
            var error = Request.Form["error"].FirstOrDefault();

            if (!string.IsNullOrEmpty(error))
            {
                throw new Exception(error);
            }

            // read discovery document to find authorize endpoint
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync(Configuration["AuthenticationUrl"]);

            ClaimsPrincipal user;
            try
            {
                user = ValidateIdentityToken(disco, idToken);
            }
            catch (SecurityTokenExpiredException)
            {
                return Redirect(nameof(Login));
            }

            var userIdClaim = user.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Subject);
            if (userIdClaim == null)
            {
                throw new Exception("Unknown User Id");
            }

            int userId = int.Parse(userIdClaim.Value);
            var applicationUser = (await _userService.GetUserAsync(userId)).obj;

            ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            // assign role membership claims
            var roleClaims = _userService.GetRoleMembership(applicationUser.AppxUserAdminId).Select(r => new Claim(r, "true"));
            identity.AddClaims(roleClaims);

            // add id token so we can log the user out without the confirmation message on Identity Server
            identity.AddClaim(new Claim("id_token", idToken));
            
            // check to see if account is a School Master Account
            if (roleClaims.Any(c => c.Type == "UserFunction.SchoolMasterAccount"))
            {
                user.AddIdentity(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, user);
                return Redirect("/SelfProvision");
            }

            // stuff the identity full of user claims
            identity.AddClaim(new Claim(JwtClaimTypes.Email, applicationUser.Email));
            identity.AddClaim(new Claim(JwtClaimTypes.Name, applicationUser.AppxUser.FirstName + " " + applicationUser.AppxUser.LastName));

            if (applicationUser.AppxUser.IsAdmin)
            {
                identity.AddClaim(new Claim("Admin", "true"));
            }

            user.AddIdentity(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, user);

            if (returnUrl != null && !string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            // Redirect to the home page
            return Redirect("/");
        }

        private IActionResult StartAuthentication(DiscoveryResponse disco, string returnUrl)
        {
            if (disco == null || disco.AuthorizeEndpoint == null)
            {
                throw new Exception("Unable to reach Identity Server");
            }

            // This is an environment agnostic way to build the callback url for the request
            var callbackUrl = Url.Action("logincallback", null, null, Request.Scheme);

            var authorizeUrl = new RequestUrl(disco.AuthorizeEndpoint).CreateAuthorizeUrl(
                clientId: "vsand.backend",
                responseType: "code id_token",
                scope: "openid profile api1",
                redirectUri: callbackUrl,
                state: returnUrl,
                nonce: "random_nonce",
                responseMode: "form_post");

            return Redirect(authorizeUrl);
        }

        private ClaimsPrincipal ValidateIdentityToken(DiscoveryResponse disco, string idToken)
        {
            var user = ValidateJwt(disco, idToken);

            var nonce = user.FindFirst("nonce")?.Value ?? "";
            if (!string.Equals(nonce, "random_nonce"))
            {
                throw new Exception("invalid nonce");
            }

            return user;
        }

        private static ClaimsPrincipal ValidateJwt(DiscoveryResponse disco, string jwt)
        {
            var keys = new List<SecurityKey>();
            foreach (var webKey in disco.KeySet.Keys)
            {
                var e = Base64Url.Decode(webKey.E);
                var n = Base64Url.Decode(webKey.N);

                var key = new RsaSecurityKey(new RSAParameters { Exponent = e, Modulus = n })
                {
                    KeyId = webKey.Kid
                };

                keys.Add(key);
            }

            var parameters = new TokenValidationParameters
            {
                ValidIssuer = disco.Issuer,
                ValidAudience = "vsand.backend",
                IssuerSigningKeys = keys,

                NameClaimType = JwtClaimTypes.Name,
                RoleClaimType = JwtClaimTypes.Role,

                RequireSignedTokens = true
            };

            var handler = new JwtSecurityTokenHandler();
            handler.InboundClaimTypeMap.Clear();

            var user = handler.ValidateToken(jwt, parameters, out var _);
            return user;
        }
    }
}