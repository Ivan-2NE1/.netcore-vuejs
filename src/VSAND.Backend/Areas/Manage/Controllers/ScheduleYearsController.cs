using Hangfire;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using VSAND.Backend.Controllers;
using VSAND.Data.Entities;
using VSAND.Data.Identity;
using VSAND.Data.ViewModels;
using VSAND.Services.Data.Manage.ScheduleYears;
using VSAND.Services.Data.Manage.Users;
using VSAND.Services.Data.Sports;
using VSAND.Services.Data.Teams;

namespace VSAND.Backend.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Route("[area]/[controller]")]
    public class ScheduleYearsController : BaseController
    {
        private readonly IScheduleYearService _scheduleYearService;
        private readonly ITeamService _teamService;
        private readonly ISportService _sportService;
        public ScheduleYearsController(IScheduleYearService scheduleYearService, ITeamService teamService, ISportService sportService, IUserService userService) : base(userService)
        {
            _scheduleYearService = scheduleYearService ?? throw new ArgumentException("Schedule year service is null");
            _teamService = teamService ?? throw new ArgumentException("Team service is null");
            _sportService = sportService ?? throw new ArgumentException("Sport service is null");
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("{scheduleYearId}")]
        public async Task<IActionResult> Summary(int scheduleYearId)
        {
            var oModel = await _scheduleYearService.GetSummary(scheduleYearId);

            ViewData["ScheduleYearId"] = scheduleYearId;
            ViewData["ScheduleYearName"] = oModel.Name;

            return View(oModel);
        }

        [HttpGet("{scheduleYearId}/Provisioning/{sportId}")]
        public async Task<IActionResult> Provisioning(int scheduleYearId, int sportId)
        {
            var eventTypes = await _sportService.GetTopLevelEventTypesAsync(sportId, scheduleYearId);
            var regularSeason = eventTypes.FirstOrDefault(t => t.DefaultSelected.HasValue && t.DefaultSelected.Value);

            ViewData["RegularSeasonEventTypeId"] = (regularSeason != null) ? regularSeason.EventTypeId : 0;

            var oModel = await _scheduleYearService.GetProvisioning(scheduleYearId, sportId);

            ViewData["ScheduleYearId"] = scheduleYearId;
            ViewData["SportId"] = sportId;
            ViewData["ScheduleYearName"] = oModel.Name;
            ViewData["SportName"] = oModel.SportInfo.name;
            ViewData["SubHeader"] = "Provisioning";

            return View(oModel);
        }

        [HttpPost("ProvisionTeams")]
        public bool ProvisionTeams([FromBody] ProvisioningRequest provisioningRequest)
        {
            // TODO: try catch this and return false if it doesn't enqueue properly
            BackgroundJob.Enqueue(() => _teamService.ProvisionTeams(ApplicationUser.AppxUser, provisioningRequest.ScheduleYearId, provisioningRequest.SportId, provisioningRequest.SchoolIds));
            return true;
        }

        #region Event Types
        [HttpGet("{scheduleYearId}/EventTypes/{sportId}")]
        public async Task<IActionResult> EventTypes(int scheduleYearId, int sportId)
        {
            var sport = await _sportService.GetSportAsync(sportId);

            ViewData["ScheduleYearId"] = scheduleYearId;
            ViewData["SportId"] = sport.SportId;
            ViewData["SportName"] = sport.Name;
            ViewData["SubHeader"] = "Event Types";

            var scheduleYear = await _scheduleYearService.GetScheduleYear(scheduleYearId);
            ViewData["ScheduleYearName"] = scheduleYear.Name;

            var eventTypes = await _sportService.GetTopLevelEventTypesAsync(sportId, scheduleYearId);
            return View("EventTypes/Index", eventTypes);
        }

        [HttpGet("{scheduleYearId}/EventTypes/{sportId}/Overview/{eventTypeId}")]
        public async Task<IActionResult> EventTypeOverview(int scheduleYearId, int sportId, int eventTypeId)
        {
            var eventType = await _sportService.GetEventTypeAsync(eventTypeId);
            if (eventType == null)
            {
                return NotFound();
            }

            ViewData["EventTypeId"] = eventType.EventTypeId;

            var scheduleYear = await _scheduleYearService.GetSummary(scheduleYearId);
            var sport = await _sportService.GetSportAsync(sportId);

            ViewData["ScheduleYearId"] = scheduleYear.ScheduleYearId;
            ViewData["SportId"] = sport.SportId;
            ViewData["ScheduleYearName"] = scheduleYear.Name;
            ViewData["SportName"] = sport.Name;
            ViewData["SubHeader"] = $"Event Types ({eventType.Name})";

            return View("EventTypes/Overview", eventType);
        }

        [ActionName("Rounds")]
        [HttpGet("{scheduleYearId}/EventTypes/{sportId}/Rounds/{eventTypeId}")]
        public async Task<IActionResult> EventTypeRounds(int scheduleYearId, int sportId, int eventTypeId)
        {
            var eventType = await _sportService.GetEventTypeAsync(eventTypeId);
            ViewData["EventTypeId"] = eventType.EventTypeId;
            ViewData["EventTypeName"] = eventType.Name;

            var scheduleYear = await _scheduleYearService.GetScheduleYear(scheduleYearId);
            ViewData["ScheduleYearName"] = scheduleYear.Name;

            var sport = await _sportService.GetSportAsync(sportId);
            ViewData["SportName"] = sport.Name;
            ViewData["SportId"] = sport.SportId;
            ViewData["SubHeader"] = $"Event Types ({eventType.Name}) Rounds";

            var rounds = await _sportService.GetEventTypeRoundsAsync(eventTypeId);
            return View("EventTypes/Rounds", rounds);
        }

        [ActionName("Groupings")]
        [HttpGet("{scheduleYearId}/EventTypes/{sportId}/Groupings/{eventTypeId}")]
        public async Task<IActionResult> EventTypeGroupings(int scheduleYearId, int sportId, int eventTypeId)
        {
            var eventType = await _sportService.GetEventTypeAsync(eventTypeId);
            ViewData["EventTypeId"] = eventType.EventTypeId;
            ViewData["EventTypeName"] = eventType.Name;

            var scheduleYear = await _scheduleYearService.GetSummary(scheduleYearId);
            ViewData["ScheduleYearName"] = scheduleYear.Name;

            var sport = await _sportService.GetSportAsync(sportId);
            ViewData["SportName"] = sport.Name;
            ViewData["SportId"] = sport.SportId;
            ViewData["SubHeader"] = $"Event Types ({eventType.Name}) Groupings";

            var sections = await _sportService.GetEventTypeSectionsAsync(eventTypeId);
            return View("EventTypes/Groupings", sections);
        }

        [HttpGet("{scheduleYearId}/EventTypes/{sportId}/ChildEventTypes/{eventTypeId}")]
        public async Task<IActionResult> ChildEventTypes(int scheduleYearId, int sportId, int eventTypeId)
        {
            var childEventTypes = await _sportService.GetChildEventTypes(eventTypeId);
            ViewData["EventTypeId"] = eventTypeId;

            var scheduleYear = await _scheduleYearService.GetSummary(scheduleYearId);
            var sport = await _sportService.GetSportAsync(sportId);
            var eventType = await _sportService.GetEventTypeAsync(eventTypeId);

            ViewData["ScheduleYearId"] = scheduleYear.ScheduleYearId;
            ViewData["ScheduleYearName"] = scheduleYear.Name;
            ViewData["SportId"] = sport.SportId;
            ViewData["SportName"] = sport.Name;

            ViewData["SubHeader"] = $"Event Types ({eventType.Name}) Child Event Types";


            return View("EventTypes/ChildEventTypes", childEventTypes);
        }
        #endregion

        #region Schedules
        [HttpGet("{scheduleYearId}/Schedules/{sportId}")]
        public async Task<IActionResult> Schedules(int scheduleYearId, int sportId)
        {
            var scheduleYear = await _scheduleYearService.GetScheduleYear(scheduleYearId);
            var sport = await _sportService.GetSportAsync(sportId);

            ViewData["ScheduleYearId"] = scheduleYear.ScheduleYearId;
            ViewData["SportId"] = sport.SportId;
            ViewData["ScheduleYearName"] = scheduleYear.Name;
            ViewData["SportName"] = sport.Name;
            ViewData["SubHeader"] = "Schedules";

            var model = await _scheduleYearService.GetScheduleFiles(scheduleYearId, sportId);

            return View("Schedules/Index", model);
        }

        [HttpGet("{scheduleYearId}/Schedules/{sportId}/ProcessFile/{fileId}")]
        public async Task<IActionResult> GetProcessScheduleFile(int scheduleYearId, int sportId, int fileId)
        {
            var scheduleYear = await _scheduleYearService.GetScheduleYear(scheduleYearId);
            var sport = await _sportService.GetSportAsync(sportId);

            ViewData["ScheduleYearId"] = scheduleYear.ScheduleYearId;
            ViewData["SportId"] = sport.SportId;
            ViewData["ScheduleYearName"] = scheduleYear.Name;
            ViewData["SportName"] = sport.Name;
            ViewData["SubHeader"] = "Schedules";
            ViewData["FileId"] = fileId;

            var model = await _scheduleYearService.GetScheduleFile(scheduleYearId, sportId, fileId);

            return View("Schedules/ProcessFile", model);
        }



        #endregion

        [HttpGet("{scheduleYearId}/PowerPoints/{sportId}")]
        public async Task<IActionResult> PowerPoints(int scheduleYearId, int sportId)
        {
            var scheduleYear = await _scheduleYearService.GetSummary(scheduleYearId);
            var sport = await _sportService.GetSportAsync(sportId);

            ViewData["ScheduleYearId"] = scheduleYear.ScheduleYearId;
            ViewData["ScheduleYearName"] = scheduleYear.Name;
            ViewData["SportName"] = sport.Name;
            ViewData["SportId"] = sport.SportId;
            ViewData["SubHeader"] = "Power Points";

            var powerPointsConfig = await _scheduleYearService.GetPowerPointsConfig(scheduleYearId, sportId);
            if (powerPointsConfig == null)
            {
                powerPointsConfig = new VsandPowerPointsConfig();
            }

            return View(powerPointsConfig);
        }

        [HttpGet("{scheduleYearId}/LeagueRules/{sportId}")]
        public async Task<IActionResult> LeagueRules(int scheduleYearId, int sportId)
        {
            var scheduleYear = await _scheduleYearService.GetSummary(scheduleYearId);
            ViewData["ScheduleYearId"] = scheduleYear.ScheduleYearId;
            ViewData["ScheduleYearName"] = scheduleYear.Name;

            var sport = await _sportService.GetSportAsync(sportId);
            ViewData["SportId"] = sport.SportId;
            ViewData["SportName"] = sport.Name;

            ViewData["SubHeader"] = "League Rules";

            var leagueRules = await _scheduleYearService.GetLeagueRulesAsync(scheduleYearId, sportId);

            ViewData["LeagueRuleOptions"] = leagueRules.Select(lr => new ListItem<string>
            {
                id = $"{lr.Conference}|{lr.Division}",
                name = $"{lr.Conference} - {lr.Division}"
            }).Distinct().ToList();

            var leagueRuleVm = leagueRules.Select(lr => new LeagueRule(lr)).ToList();
            return View(leagueRuleVm);
        }
    }
}