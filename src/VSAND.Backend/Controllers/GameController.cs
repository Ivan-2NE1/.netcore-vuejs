using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VSAND.Services.Data.GameReports;

namespace VSAND.Backend.Controllers
{
    public class GameController : Controller
    {
        private IGameReportService _gameService = null;
        public GameController(IGameReportService gameService)
        {
            _gameService = gameService;
        }

        [Route("/game/{id:int}")]
        public async Task<IActionResult> Index(int id)
        {
            var oModel = await _gameService.GetGameReport(id);
            return View(oModel);
        }

        /// <summary>
        /// Report a new game
        /// </summary>
        /// <returns></returns>
        [ActionName("Report")]
        public async Task<IActionResult> Report([FromQuery] int? sportId, [FromQuery] int? scheduleYearId, [FromQuery] int? teamId)
        {
            var oAddGame = await _gameService.GetAddGameReport(sportId, scheduleYearId, teamId);
            return View(oAddGame);
        }

        [ActionName("Schedule")]
        public async Task<IActionResult> Schedule([FromQuery] int? sportId, [FromQuery] int? scheduleYearId, [FromQuery] int? teamId)
        {
            var oAddScheduleGame = await _gameService.GetAddScheduledGame(sportId, scheduleYearId, teamId);
            return View(oAddScheduleGame);
        }

        [Route("/game/nav/{id:int}")]
        public async Task<IActionResult> Nav(int id)
        {
            var oModel = await _gameService.GetGameReport(id);
            return PartialView("~/Views/Game/_GameNav.cshtml", oModel);
        }

        [Route("/game/scoring/{id:int}")]
        public async Task<IActionResult> Scoring(int id)
        {
            var oModel = await _gameService.GetGameReportScoring(id);
            return View(oModel);
        }

        [Route("/game/plays/{id:int}")]
        public async Task<IActionResult> PlayByPlay(int id)
        {
            var oModel = await _gameService.GetGameReport(id);
            return View(oModel);
        }

        [Route("/game/events/{id:int}")]
        public async Task<IActionResult> Events(int id)
        {
            var oModel = await _gameService.GetGameReportEvents(id);
            return View(oModel);
        }

        [Route("/game/teamstats/{id:int}")]
        public async Task<IActionResult> TeamStats(int id, int teamId)
        {
            var oModel = await _gameService.GetGameReportTeamStats(id);
            return View(oModel);
        }

        [Route("/game/playerstats/{id:int}/{teamid:int}")]
        public async Task<IActionResult> PlayerStats(int id, int teamId)
        {
            var oModel = await _gameService.GetGameReportPlayerStats(id, teamId);
            ViewData["GameReportTeamId"] = teamId;
            return View(oModel);
        }

        [Route("/game/review/{id:int}")]
        public async Task<IActionResult> Review(int id)
        {
            var oModel = await _gameService.GetGameReport(id);
            return View(oModel);
        }

        /// <summary>
        /// Single game report presented in "one view" format
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("/game/oneview/{id:int}")]
        public async Task<IActionResult> OneView(int id)
        {
            var oModel = await _gameService.GetFullGameReport(id);
            //TODO: Build One View for game report
            return View(oModel);
        }
    }
}