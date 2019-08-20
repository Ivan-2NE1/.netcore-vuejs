using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace VSAND.Backend.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class PublicationsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}