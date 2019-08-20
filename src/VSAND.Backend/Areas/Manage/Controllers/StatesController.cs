using Microsoft.AspNetCore.Mvc;

namespace VSAND.Backend.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class StatesController : Controller
    {
        // GET: Manage/States
        public IActionResult Index()
        {
            return View();
        }
    }
}
