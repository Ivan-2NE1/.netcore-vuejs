using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VSAND.Data.ViewModels.Teams;
using VSAND.Services.Data.Teams;
using VSAND.Services.Files;
using VSAND.Services.Razor;

namespace VSAND.Backend.Controllers
{
    [Route("[controller]")]
    public class TeamsController : Controller
    {
        private readonly ITeamService _teamService;
        private readonly IExcelService _excelService;
        private readonly IPDFService _pdfService;
        private readonly IRazorViewRenderer _viewRenderer;

        public TeamsController(ITeamService teamService, IExcelService excelService, IPDFService pdfService, IRazorViewRenderer viewRenderer)
        {
            _teamService = teamService;
            _excelService = excelService;
            _pdfService = pdfService;
            _viewRenderer = viewRenderer;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Search")]
        public async Task<IActionResult> Search([FromQuery(Name = "q")] string query)
        {
            var oSearch = Newtonsoft.Json.JsonConvert.DeserializeObject<SearchRequest>(query);
            var oTeams = await _teamService.GetTeamsAsync(oSearch);
            ViewData["SearchRequest"] = oSearch;
            return View(oTeams);
        }

        [HttpGet("CustomCodeImporter")]
        public IActionResult CustomCodeImporter()
        {
            return View();
        }

        #region Specific Team Pages
        [HttpGet("{teamId}")]
        public async Task<IActionResult> Team([FromRoute] int teamId)
        {
            var oTeam = await _teamService.GetTeamAsync(teamId);
            if (oTeam != null)
            {
                ViewData["SportId"] = oTeam.SportId;
                ViewData["TeamName"] = oTeam.Name;
                return View(oTeam);
            }
            Response.StatusCode = 404;
            return View();
        }

        [HttpGet("{teamId}/Info")]
        public async Task<IActionResult> Info([FromRoute] int teamId)
        {
            var oTeam = await _teamService.GetTeamAsync(teamId);
            if (oTeam != null)
            {
                ViewData["SportId"] = oTeam.SportId;
                ViewData["TeamName"] = oTeam.Name;
                return View(oTeam);
            }
            Response.StatusCode = 404;
            return View();
        }

        [HttpGet("{teamId}/DistributionList")]
        public async Task<IActionResult> DistributionList([FromRoute] int teamId)
        {
            var oTeam = await _teamService.GetTeamDistribution(teamId);
            if (oTeam != null)
            {
                ViewData["SportId"] = oTeam.SportId;
                ViewData["TeamName"] = oTeam.Name;
                return View(oTeam);
            }
            Response.StatusCode = 404;
            return View();
        }

        [HttpGet("{teamId}/Book")]
        public async Task<IActionResult> Book([FromRoute] int teamId)
        {
            var oTeam = await _teamService.GetTeamAsync(teamId);
            if (oTeam != null)
            {
                ViewData["SportId"] = oTeam.SportId;
                ViewData["TeamName"] = oTeam.Name;
                return View(oTeam);
            }
            Response.StatusCode = 404;
            return View();
        }

        [HttpGet("{teamId}/Roster")]
        public async Task<IActionResult> Roster([FromRoute] int teamId)
        {
            var oTeam = await _teamService.GetTeamRoster(teamId);
            if (oTeam != null)
            {
                ViewData["SportId"] = oTeam.SportId;
                ViewData["TeamId"] = oTeam.TeamId;
                ViewData["TeamName"] = oTeam.School.Name;
                ViewData["Positions"] = (await _teamService.GetPositionsAsync(oTeam.SportId));
                return View(oTeam);
            }
            Response.StatusCode = 404;
            return View();
        }

        [HttpGet("{teamid}/Roster/excel")]
        public async Task<ActionResult> RosterExportExcel([FromRoute] int teamId)
        {
            var oTeam = await _teamService.GetTeamAsync(teamId);
            var oRoster = await _teamService.GetRoster(teamId);
            var fileName = $"{oTeam.School.Name} {oTeam.ScheduleYear.Name} {oTeam.Sport.Name}.xlsx";
            var fileBytes = _excelService.TeamRosterFile(oRoster, fileName);
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HttpGet("{teamid}/Roster/pdf")]
        public async Task<ActionResult> RosterExportPdf([FromRoute] int teamId)
        {
            var oTeam = await _teamService.GetTeamRoster(teamId);

            string renderedHtml = await _viewRenderer.RenderAsync("Templates/PDF/TeamRoster", oTeam);
            var docTitle = $"{oTeam.School.Name} {oTeam.ScheduleYear.Name} {oTeam.Sport.Name}";
            var fileName = $"{docTitle}.pdf";
            var fileBytes = _pdfService.GeneratePdf(renderedHtml, docTitle);
            //var fileBytes = _pdfService.TeamRosterFile(oRoster, fileName);
            return File(fileBytes, "application/pdf", fileName);
        }

        [HttpGet("{teamId}/PitchCount")]
        public async Task<IActionResult> Pitchcount([FromRoute] int teamId)
        {
            var oTeam = await _teamService.GetTeamDistribution(teamId);
            if (oTeam != null && oTeam.Sport.Name.Contains("baseball", System.StringComparison.OrdinalIgnoreCase))
            {
                ViewData["SportId"] = oTeam.SportId;
                ViewData["TeamName"] = oTeam.Name;
                return View(oTeam);
            }
            Response.StatusCode = 404;
            return View();
        }

        [HttpGet("{teamId}/Stats")]
        public async Task<IActionResult> Stats([FromRoute] int teamId)
        {
            var oTeam = await _teamService.GetTeamDistribution(teamId);
            if (oTeam != null)
            {
                ViewData["SportId"] = oTeam.SportId;
                ViewData["TeamName"] = oTeam.Name;
                return View(oTeam);
            }
            Response.StatusCode = 404;
            return View();
        }

        [HttpGet("{teamId}/CustomCodes")]
        public async Task<IActionResult> CustomCodes(int teamId)
        {
            var team = await _teamService.GetTeamAsync(teamId);
            if (team == null)
            {
                return NotFound();
            }

            ViewData["SportId"] = team.SportId;
            ViewData["TeamName"] = team.Name;
            ViewData["CustomCodes"] = await _teamService.GetTeamCustomCodes(teamId);

            return View(team);
        }
        #endregion
    }
}