using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using VSAND.Backend.Controllers;
using VSAND.Data.ViewModels;
using VSAND.Data.Entities;
using VSAND.Data.ViewModels.GameReport;
using VSAND.Services.Data.GameReports;
using VSAND.Services.Data.Manage.Users;

namespace VSAND.Backend.Areas.SiteApi
{
    [ApiController]
    [Route("siteapi/[controller]")]
    public class GamesController : BaseApiController
    {
        private IGameReportService _gameService = null;
        public GamesController(IUserService userService, IGameReportService gameService) : base(userService)
        {
            _gameService = gameService;
        }

        // GET: api/Games
        [HttpGet("teamgames")]
        public async Task<IEnumerable<TeamGameSummary>> TeamGames([FromQuery] int teamId)
        {
            return await _gameService.TeamGames(teamId);
        }

        // GET: api/Games
        [HttpGet("teamrecordinfo")]
        public async Task<IEnumerable<TeamRecordInfo>> TeamRecordInfo([FromQuery] int teamId, bool UseCached)
        {
            UseCached = true;
            return await _gameService.TeamRecordInfo(teamId, UseCached);
        }

        // GET: api/Games/5
        [HttpGet("{id}", Name = "GetGame")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Games
        [HttpPost]
        public async Task<ApiResult<AddGameReport>> Post([FromBody] AddGameReport gameReport)
        {
            var result = await _gameService.AddGameReportAsync(ApplicationUser.AppxUser, gameReport);
            return new ApiResult<AddGameReport>(result);
        }

        // POST: siteapi/Games/Schedule
        [HttpPost("scheduleGame")]
        public async Task<ApiResult<AddScheduledGame>> ScheduleGame([FromBody] AddScheduledGame scheduleGame)
        {
            var result = await _gameService.AddScheduledGameAsync(ApplicationUser.AppxUser, scheduleGame);
            return new ApiResult<AddScheduledGame>(result);
        }

        // PUT: api/Games/updateGameReport/5
        [HttpPut("updateGameReport/{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/games/deleteGameReport/5
        [HttpDelete("deleteGameReport/{gameReportId}")]
        public async Task<ApiResult<VsandGameReport>> Delete(int gameReportId)
        {
            ServiceResult<VsandGameReport> result = await _gameService.DeleteGameReportAsync(gameReportId);
            return new ApiResult<VsandGameReport>(result);
        }

        // POST: siteapi/games/saveteamstat
        [HttpPost("saveteamstat")]
        public async Task<ApiResult<GameReportTeamStat>> SaveTeamStat([FromBody] GameReportTeamStat grts)
        {
            var oResult = await _gameService.SaveTeamStatAsync(ApplicationUser.AppxUser, grts);
            return new ApiResult<GameReportTeamStat>(oResult);
        }

        // POST: siteapi/games/saveplayerstat
        [HttpPost("saveplayerstat")]
        public async Task<ApiResult<GameReportPlayerStat>> SavePlayerStat([FromBody] GameReportPlayerStat grps)
        {
            var oResult = await _gameService.SavePlayerStatAsync(ApplicationUser.AppxUser, grps);
            return new ApiResult<GameReportPlayerStat>(oResult);
        }

        // POST: siteapi/games/initplayer
        [HttpGet("initplayer")]
        public async Task<ServiceResult<List<GameReportPlayerStat>>> InitPlayer([FromQuery] int gameId, [FromQuery] int teamId, [FromQuery] int playerId)
        {
            var result = await _gameService.InitPlayer(ApplicationUser.AppxUser, gameId, teamId, playerId);
            return result;
        }
    }
}
