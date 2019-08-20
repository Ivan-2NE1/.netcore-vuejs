using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VSAND.Backend.Controllers
{
    public class KeepAliveController : Controller
    {
        [AllowAnonymous]
        public bool Index()
        {
            return true;
        }
    }
}