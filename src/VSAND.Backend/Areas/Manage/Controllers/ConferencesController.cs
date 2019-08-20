using Microsoft.AspNetCore.Mvc;

namespace VSAND.Backend.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ConferencesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}