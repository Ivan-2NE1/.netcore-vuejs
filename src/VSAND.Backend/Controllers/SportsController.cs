using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using VSAND.Services.Data.Sports;

namespace VSAND.Backend.Areas.Sports.Controllers
{
    public class SportsController : Controller
    {
        private readonly ISportService _sportService;
        public SportsController(ISportService sportService)
        {
            _sportService = sportService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Sports/{sportId}/Edit")]
        public async Task<IActionResult> Edit(int sportId)
        {
            var sport = await _sportService.GetSportAsync(sportId);
            ViewData["SportId"] = sportId;

            // used to populate drop-downs
            ViewData["Positions"] = (await _sportService.GetPositionsAsync(sportId)).Positions;
            ViewData["PlayerStatCategories"] = (await _sportService.GetPlayerStatsAsync(sportId)).PlayerStatCategories;

            return View(sport);
        }

        [HttpGet("Sports/{sportId}/Positions")]
        public async Task<IActionResult> Positions(int sportId)
        {
            var sport = await _sportService.GetPositionsAsync(sportId);

            ViewData["SportId"] = sportId;

            return View(sport);
        }

        [HttpGet("Sports/{sportId}/Positions/{positionId}/FeaturedStats")]
        public async Task<IActionResult> FeaturedStats(int sportId, int positionId)
        {
            var sport = await _sportService.GetPositionsAsync(sportId);

            ViewData["SportId"] = sport.SportId;
            ViewData["Position"] = sport.Positions.FirstOrDefault(p => p.SportPositionId == positionId);
            ViewData["PlayerStats"] = (await _sportService.GetPlayerStatsAsync(sportId)).PlayerStatCategories.SelectMany(psc => psc.PlayerStats).OrderBy(psc => psc.Name).ToList();

            return View(sport);
        }

        [ActionName("Game Meta")]
        [HttpGet("Sports/{sportId}/GameMeta")]
        public async Task<IActionResult> GameMeta(int sportId)
        {
            var sport = await _sportService.GetGameMetaAsync(sportId);
            ViewData["SportId"] = sportId;
            return View("GameMeta", sport);
        }

        [HttpGet("Sports/{sportId}/Events")]
        public async Task<IActionResult> Events(int sportId)
        {
            var sport = await _sportService.GetEventsAsync(sportId);
            ViewData["SportId"] = sportId;
            return View(sport);
        }

        [HttpGet("Sports/{sportId}/EventResultTypes")]
        public async Task<IActionResult> EventResultTypes(int sportId)
        {
            var sport = await _sportService.GetEventsAsync(sportId);
            ViewData["SportId"] = sportId;
            return View(sport);
        }

        [ActionName("Team Stat Categories")]
        [HttpGet("Sports/{sportId}/TeamStatCategories")]
        public async Task<IActionResult> TeamStatCategories(int sportId)
        {
            var sport = await _sportService.GetTeamStatsAsync(sportId);
            ViewData["SportId"] = sportId;
            return View("TeamStatCategories", sport);
        }

        [ActionName("Player Stat Categories")]
        [HttpGet("Sports/{sportId}/PlayerStatCategories")]
        public async Task<IActionResult> PlayerStatCategories(int sportId)
        {
            var sport = await _sportService.GetPlayerStatsAsync(sportId);
            ViewData["SportId"] = sportId;
            return View("PlayerStatCategories", sport);
        }

        [ActionName("Team Stats")]
        [HttpGet("Sports/{sportId}/TeamStats/{teamStatCategoryId}")]
        public async Task<IActionResult> TeamStats(int sportId, int teamStatCategoryId)
        {
            var sport = await _sportService.GetTeamStatsAsync(sportId);
            ViewData["SportId"] = sportId;
            ViewData["TeamStatCategoryId"] = teamStatCategoryId;
            return View("TeamStats", sport);
        }

        [ActionName("Player Stats")]
        [HttpGet("Sports/{sportId}/PlayerStats/{playerStatCategoryId}")]
        public async Task<IActionResult> PlayerStats(int sportId, int playerStatCategoryId)
        {
            var sport = await _sportService.GetPlayerStatsAsync(sportId);
            ViewData["SportId"] = sportId;
            ViewData["PlayerStatCategoryId"] = playerStatCategoryId;
            return View("PlayerStats", sport);
        }
    }
}