using Microsoft.AspNetCore.Mvc;
using NLog;
using System.Threading.Tasks;
using VSAND.Services.Data.Teams;

namespace VSAND.Frontend.Controllers
{
    public class TeamsController : Controller
    {
        NLog.ILogger log = LogManager.GetCurrentClassLogger();

        private ITeamService _teamService;

        public TeamsController(ITeamService teamService)
        {
            _teamService = teamService;
        }
        
        public IActionResult Index()
        {
            return View();
        }

        [Route ("teams/{schoolid:int}/{sportid:int}/{syid:int}")]
		public async Task<IActionResult> Team([FromRoute(Name = "schoolid")] int schoolId, [FromRoute(Name = "sportid")] int sportId, [FromRoute(Name = "syid")] int syid)
		{
            log.Info($"Team requested with syid {syid}");
            int scheduleYearId = syid;

            ViewData["ActiveTab"] = "Home";
            SetTeamContext();
            var team = await _teamService.GetFullTeamCachedAsync(schoolId, sportId, scheduleYearId);
            if (team == null)
            {
                log.Warn($"Couldn't load team from SchoolId:{schoolId}, SportId:{sportId}, ScheduleYearId:{scheduleYearId}");
            }
            return View("Team", team);
        }

        [Route("teams/{schoolid:int}/{sportid:int}/{syid:int}/roster")]
        public async Task<IActionResult> Roster([FromRoute(Name = "schoolid")] int schoolId, [FromRoute(Name = "sportid")] int sportId, [FromRoute(Name = "syid")] int syid)
        {
            log.Info($"TeamRoster requested with syid {syid}");
            int scheduleYearId = syid;

            ViewData["ActiveTab"] = "Roster";
            SetTeamContext();
            var team = await _teamService.GetFullTeamCachedAsync(schoolId, sportId, scheduleYearId);
            if (team == null)
            {
                log.Warn($"Couldn't load team from SchoolId:{schoolId}, SportId:{sportId}, ScheduleYearId:{scheduleYearId}");
            }
            return View("Roster", team);
        }

        [Route("teams/{schoolid:int}/{sportid:int}/{syid:int}/stats")]
        public async Task<IActionResult> Stats([FromRoute(Name = "schoolid")] int schoolId, [FromRoute(Name = "sportid")] int sportId, [FromRoute(Name = "syid")] int syid)
        {
            log.Info($"TeamStats requested with syid {syid}");
            int scheduleYearId = syid;
            
            ViewData["ActiveTab"] = "Stats";
            SetTeamContext();
            var team = await _teamService.GetFullTeamCachedAsync(schoolId, sportId, scheduleYearId);
            if (team == null)
            {
                log.Warn($"Couldn't load team from SchoolId:{schoolId}, SportId:{sportId}, ScheduleYearId:{scheduleYearId}");
            }
            return View("Stats", team);
        }

        [Route("teams/{schoolid:int}/{sportid:int}/{syid:int}/schedule")]
        public async Task<IActionResult> Schedule([FromRoute(Name = "schoolid")] int schoolId, [FromRoute(Name = "sportid")] int sportId, [FromRoute(Name = "syid")] int syid)
        {
            log.Info($"TeamSchedule requested with syid {syid}");
            int scheduleYearId = syid;

            ViewData["ActiveTab"] = "Schedule";
            SetTeamContext();
            var team = await _teamService.GetFullTeamCachedAsync(schoolId, sportId, scheduleYearId);
            if (team == null)
            {
                log.Warn($"Couldn't load team from SchoolId:{schoolId}, SportId:{sportId}, ScheduleYearId:{scheduleYearId}");
            }
            return View("Schedule", team);
        }

        [Route("teams/{schoolid:int}/{sportid:int}/{syid:int}/powerpoints")]
        public async Task<IActionResult> PowerPoints([FromRoute(Name = "schoolid")] int schoolId, [FromRoute(Name = "sportid")] int sportId, [FromRoute(Name = "syid")] int syid)
        {
            log.Info($"TeamSchedule requested with syid {syid}");
            int scheduleYearId = syid;

            ViewData["ActiveTab"] = "PowerPoints";
            SetTeamContext();
            var team = await _teamService.GetFullTeamCachedAsync(schoolId, sportId, scheduleYearId);
            if (team == null)
            {
                log.Warn($"Couldn't load team from SchoolId:{schoolId}, SportId:{sportId}, ScheduleYearId:{scheduleYearId}");
            }
            return View("PowerPoints", team);
        }

        private void SetTeamContext()
        {
            // we require a school slug, sport slug and the season slug is optional
            string schoolSlug = HttpContext.Items["SchoolSlug"].ToString();
            string sportSlug = HttpContext.Items["SportSlug"].ToString();
            string seasonSlug = "";
            if (HttpContext.Items.ContainsKey("SeasonSlug"))
            {
                seasonSlug = HttpContext.Items["SeasonSlug"].ToString();
            }
            string basePath = "/school/" + schoolSlug + "/" + sportSlug;
            if (!string.IsNullOrEmpty(seasonSlug))
            {
                basePath += "/season/" + seasonSlug;
            }

            ViewData["BasePath"] = basePath;
        }
    }
}