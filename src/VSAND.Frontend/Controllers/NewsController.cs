using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VSAND.Services.News;

namespace VSAND.Frontend.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsService _newsService;

        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }

        [Route("News")]
        public async Task<IActionResult> Index()
        {
            var stories = await _newsService.GetNewsStoriesAsync();
            return View(stories);
        }

        // article stub
        // articleid and headline required
        [Route("news/{articleid:int}/{headline}")]
        public IActionResult Article()
        {
            return View();
        }
    }
}