using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VSAND.Services.Data.Sports;
using VSAND.Services.Display.Sports;
using VSAND.Services.StatAgg;

namespace VSAND.Frontend.Controllers
{
    public class SportsController : Controller
    {
        private readonly ISportService _sports;
        private readonly IPlayerStatAggregation _playerStatAgg;
        private readonly ITeamStatAggregation _teamStatAgg;
        private readonly ISportsDisplayService _sportDisplayService;

        public SportsController(ISportService sports, IPlayerStatAggregation playerStatAgg, ITeamStatAggregation teamStatAgg, ISportsDisplayService sportDisplayService)
        {
            _sports = sports;
            _playerStatAgg = playerStatAgg;
            _teamStatAgg = teamStatAgg;
            _sportDisplayService = sportDisplayService;
        }

		[Route ("sports/")]
        public IActionResult Index()
        {
            return View();
        }

		[Route ("sport/{sportid:int}")]
		public async Task<IActionResult> Sport([FromRoute(Name = "sportid")] int sportId)
		{
            SetSportContext("Home");
            var model = await _sports.GetSportAsync(sportId);
			return View(model);
		}

        [Route("sport/{sportid:int}/scores")]
        public async Task<IActionResult> Scores([FromRoute(Name = "sportid")] int sportId)
        {
            SetSportContext("Scores");
            var model = await _sports.GetSportAsync(sportId);
            return View(model);
        }

        [Route("sport/{sportid:int}/schedule/{year:int?}/{month:int?}/{day:int?}")]
        public async Task<IActionResult> Schedule([FromRoute(Name = "sportid")] int sportId, [FromRoute(Name = "year")] int? year, [FromRoute(Name = "month")] int? month, [FromRoute(Name = "day")] int? day)
        {
            SetSportContext("Schedule");
            var model = await _sports.GetSportAsync(sportId);

            DateTime viewDate = DateTime.Now;
            if (year.HasValue && year.Value > 0)
            {
                if (month.HasValue && month.Value > 0)
                {
                    if (day.HasValue && day.Value > 0)
                    {
                        try
                        {
                            var testDate = new DateTime(year.Value, month.Value, day.Value);
                            viewDate = testDate;
                        } catch(Exception ex)
                        {
                            // don't care!
                        }
                    }
                }
            }

            ViewData["ViewDate"] = viewDate.ToString("MM/dd/yyyy");

            return View(model);
        }

        [Route("sport/{sportid:int}/standings/{scheduleyearid:int?}")]
        public async Task<IActionResult> Standings([FromRoute(Name = "sportid")] int sportId, [FromRoute(Name = "scheduleyearid")] int? scheduleYearId, [FromQuery(Name = "conference")] string conference)
        {
            SetSportContext("Standings");
                      
            var model = await _sports.GetSportStandingsViewCachedAsync(sportId, scheduleYearId, conference);

            if (model != null && model.Standings != null && model.Standings.Any())
            {
                return View("StandingsConference", model);
            }

            return View(model);
        }

        [Route("sport/{sportid:int}/rankings")]
        public async Task<IActionResult> Rankings([FromRoute(Name = "sportid")] int sportId)
        {
            SetSportContext("Rankings");
            var model = await _sports.GetSportAsync(sportId);
            return View(model);
        }

        [Route("sport/{sportid:int}/stats")]
        public async Task<IActionResult> Stats([FromRoute(Name = "sportid")] int sportId)
        {
            SetSportContext("Stats");
            var model = await _sportDisplayService.GetSportStatsHomeView(sportId);
            return View(model);
        }

        [Route("sport/{sportid:int}/stats/player/{categoryId:int?}")]
        public async Task<IActionResult> StatsPlayerCategory(
            [FromRoute(Name = "sportid")] int sportId, 
            [FromRoute(Name = "categoryId")] int? categoryId,
            [FromQuery(Name = "syid")] int syId = 0,
            [FromQuery(Name = "statId")] int statId = 0,
            [FromQuery(Name = "dir")] string dir = "d",
            [FromQuery(Name = "pg")] int pg = 0
            )
        {
            SetSportContext("Stats");

            // parse the actual stat category out of the string
            int statCategoryId = 0;
            if (categoryId.HasValue)
            {
                statCategoryId = categoryId.Value;
            }

            int scheduleYearId = syId;
            int orderByStatId = statId;
            string orderDir = "DESC";
            if (dir == "a")
            {
                orderDir = "ASC";
            }
            int pageNumber = pg;
            
            var model = await _playerStatAgg.IndividualLeaderBoard(sportId, scheduleYearId, statCategoryId, 0, orderByStatId, orderDir, pageNumber, 50);
            return View(model);
        }

        [Route("sport/{sportid:int}/stats/team/{categoryId:int?}")]
        public async Task<IActionResult> StatsTeamCategory(
            [FromRoute(Name = "sportid")] int sportId,
            [FromRoute(Name = "categoryId")] int? categoryId,
            [FromQuery(Name = "syid")] int syId = 0,
            [FromQuery(Name = "statId")] int statId = 0,
            [FromQuery(Name = "dir")] string dir = "d",
            [FromQuery(Name = "pg")] int pg = 0
            )
        {
            SetSportContext("Stats");

            // parse the actual stat category out of the string
            int statCategoryId = 0;
            if (categoryId.HasValue)
            {
                statCategoryId = categoryId.Value;
            }

            int scheduleYearId = syId;
            int orderByStatId = statId;
            string orderDir = "DESC";
            if (dir == "a")
            {
                orderDir = "ASC";
            }
            int pageNumber = pg;

            var model = await _teamStatAgg.TeamLeaderBoard(sportId, scheduleYearId, statCategoryId, 0, orderByStatId, orderDir, pageNumber, 50);
            return View(model);
        }

        [Route("sport/{sportid:int}/powerpoints/{scheduleyearid:int?}")]
        public async Task<IActionResult> PowerPoints([FromRoute(Name = "sportid")] int sportId, [FromRoute(Name = "scheduleyearid")] int? scheduleYearId, [FromQuery(Name = "section")] string section, [FromQuery(Name = "group")] string group)
        {
            SetSportContext("PowerPoints");

            var model = await _sports.GetSportPowerpointsViewCachedAsync(sportId, scheduleYearId, section, group);

            if (model != null && model.Standings != null && model.Standings.Any())
            {
                return View("PowerPointsClassification", model);
            }

            return View(model);
        }

        [Route("sport/{sportid:int}/brackets")]
        public async Task<IActionResult> Brackets([FromRoute(Name = "sportid")] int sportId)
        {
            SetSportContext("Brackets");
            var model = await _sports.GetSportAsync(sportId);
            return View(model);
        }

        private void SetSportContext(string activeTab)
        {
            ViewData["ActiveTab"] = activeTab;
            // we require a school slug, sport slug and the season slug is optional
            string sportSlug = HttpContext.Items["SportSlug"].ToString();
            string basePath = "/" + sportSlug;
            ViewData["BasePath"] = basePath;
        }
    }
}
