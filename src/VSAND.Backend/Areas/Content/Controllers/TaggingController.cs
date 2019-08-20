using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VSAND.Services.News;

namespace VSAND.Backend.Areas.Content.Controllers
{
    [Area("Content")]
    [Route("[area]/[controller]")]
    public class TaggingController : Controller
    {
        private readonly INewsService _newsService;

        public TaggingController(INewsService newsService)
        {
            _newsService = newsService;
        }

        public async Task<IActionResult> Index(string headline, DateTime? startDate, DateTime? endDate, bool? published)
        {
            ViewData["SearchHeadline"] = headline;
            ViewData["SearchStartDate"] = startDate;
            ViewData["SearchEndDate"] = endDate;

            if (published.HasValue)
            {
                ViewData["SearchPublished"] = published.Value;
            }

            var stories = await _newsService.SearchStoriesAsync(headline, published, startDate, endDate);
            return View(stories);
        }
    }
}