using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VSAND.Frontend.Models;
using VSAND.Services.Data.GameReports;

namespace VSAND.Frontend.Areas.SiteApi.Controllers
{
    [ApiController]
    [Route("siteapi/[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly IGameReportService _gameService;

        public GamesController(IGameReportService gameService)
        {
            _gameService = gameService;
        }

        // GET: siteapi/auth/isadmin
        [HttpGet("schedule")]
        public async Task<ScheduleModel> ScheduledGames([FromQuery] DateTime? viewStart, [FromQuery] int? sportId, [FromQuery] int? schoolId, [FromQuery] int? scheduleYearId)
        {
            if (!viewStart.HasValue)
            {
                viewStart = DateTime.Now;
            }
            var oGames = await _gameService.ScheduleScoreboard(viewStart, sportId, schoolId, scheduleYearId);
            var oRet = new ScheduleModel(viewStart.Value, oGames);
            return oRet;
        }
    }
}