using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VSAND.Data.Entities;
using VSAND.Data.ViewModels;
using VSAND.Data.ViewModels.Sports;
using VSAND.Services.Data.Sports;

namespace VSAND.Backend.Areas.SiteApi
{
    [Route("SiteApi/[controller]")]
    [ApiController]
    public class SportsController : ControllerBase
    {
        private ISportService _sports = null;
        public SportsController(ISportService sportSvc)
        {
            _sports = sportSvc;
        }

        #region Sports
        [HttpGet(Name = "Get a List of All Sports")]
        public async Task<IEnumerable<VsandSport>> GetListAsync()
        {
            return await _sports.GetListAsync();
        }

        // GET: SiteApi/Sports/list
        [HttpGet("list", Name = "Get List of Active Sports")]
        public async Task<IEnumerable<ListItem<int>>> GetActiveListAsync()
        {
            return await _sports.GetActiveListAsync();
        }

        // POST: SiteApi/Sports
        [HttpPost]
        public async Task<ApiResult<VsandSport>> Post(VsandSport viewModel)
        {
            var result = await _sports.AddSportAsync(viewModel);
            return new ApiResult<VsandSport>(result);
        }

        // PUT: SiteApi/Sports/5
        [HttpPut("{sportId}")]
        public async Task<ApiResult<VsandSport>> Put(int sportId, VsandSport viewModel)
        {
            if (sportId != viewModel.SportId)
            {
                return null;
            }

            var result = await _sports.UpdateSportAsync(viewModel);
            return new ApiResult<VsandSport>(result);
        }
        #endregion

        #region Positions
        // POST: SiteApi/Sports/5/Position
        [HttpPost("{sportId}/position")]
        public async Task<ApiResult<VsandSportPosition>> AddPosition(int sportId, VsandSportPosition viewModel)
        {
            var result = await _sports.AddPositionAsync(viewModel);
            return new ApiResult<VsandSportPosition>(result);
        }

        // PUT: SiteApi/Sports/5/Position/12
        [HttpPut("{sportId}/position/{sportPositionid}")]
        public async Task<ApiResult<VsandSportPosition>> UpdatePosition(int sportId, int sportPositionId, VsandSportPosition viewModel)
        {
            if (sportId != viewModel.SportId)
            {
                return null;
            }

            if (sportPositionId != viewModel.SportPositionId)
            {
                return null;
            }

            var result = await _sports.UpdatePositionAsync(viewModel);
            return new ApiResult<VsandSportPosition>(result);
        }
        #endregion

        #region Game Meta
        // GET: SiteApi/Sports/Meta/5
        [HttpGet("meta/{sportid}")]
        public async Task<string> GetSportAndMetaConfiguration(int sportId)
        {
            // https://github.com/aspnet/AspNetCore/issues/2285
            // this is an internal serialization problem which (may) be resolved in ASP.NET Core 3.0

            var result = await _sports.GetGameMetaAsync(sportId);
            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
        }

        // POST: SiteApi/Sports/5/GameMeta
        [HttpPost("{sportId}/GameMeta")]
        public async Task<ApiResult<VsandSportGameMeta>> AddGameMeta(int sportId, VsandSportGameMeta viewModel)
        {
            if (viewModel.SportId != sportId)
            {
                return null;
            }

            var result = await _sports.AddGameMetaAsync(viewModel);
            return new ApiResult<VsandSportGameMeta>(result);
        }

        // PUT: SiteApi/Sports/5/GameMeta/12
        [HttpPut("{sportId}/GameMeta/{sportGameMetaId}")]
        public async Task<ApiResult<VsandSportGameMeta>> UpdateGameMeta(int sportId, int sportGameMetaId, VsandSportGameMeta viewModel)
        {
            if (sportId != viewModel.SportId)
            {
                return null;
            }

            if (sportGameMetaId != viewModel.SportGameMetaId)
            {
                return null;
            }

            var result = await _sports.UpdateGameMetaAsync(viewModel);
            return new ApiResult<VsandSportGameMeta>(result);
        }

        // DELETE: SiteApi/Sports/5/GameMeta/12
        [HttpDelete("{sportId}/GameMeta/{sportGameMetaId}")]
        public async Task<ApiResult<VsandSportGameMeta>> DeleteGameMeta(int sportGameMetaId)
        {
            ServiceResult<VsandSportGameMeta> result = await _sports.DeleteGameMetaAsync(sportGameMetaId);
            return new ApiResult<VsandSportGameMeta>(result);
        }

        // PUT: SiteApi/Sports/5/GameMeta/SortOrder/Update
        [HttpPut("{sportId}/GameMeta/SortOrder/Update")]
        public async Task<bool> UpdateGameMetaOrder(int sportId, [FromBody] List<VsandSportGameMeta> gameMeta)
        {
            return await _sports.UpdateGameMetaOrder(sportId, gameMeta);
        }
        #endregion

        #region Events
        // GET: SiteApi/Sports/Events/5
        [HttpGet("startingweightclasslist/{sportid}")]
        public async Task<List<ListItem<string>>> GetStartingWeightClassList(int sportId)
        {
            // https://github.com/aspnet/AspNetCore/issues/2285
            // this is an internal serialization problem which (may) be resolved in ASP.NET Core 3.0

            var result = await _sports.GetEventsAsync(sportId);
            return (from e in result.SportEvents select new ListItem<string> { id = e.Abbreviation, name = e.Name }).ToList();            
        }

        // POST: SiteApi/Sports/5/Events
        [HttpPost("{sportId}/Events")]
        public async Task<ApiResult<VsandSportEvent>> AddEvent(int sportId, VsandSportEvent viewModel)
        {
            var result = await _sports.AddEventAsync(viewModel);
            return new ApiResult<VsandSportEvent>(result);
        }

        // PUT: SiteApi/Sports/5/Events/12
        [HttpPut("{sportId}/Events/{eventId}")]
        public async Task<ApiResult<VsandSportEvent>> UpdateEvent(int sportId, int eventId, VsandSportEvent viewModel)
        {
            if (sportId != viewModel.SportId)
            {
                return null;
            }

            if (eventId != viewModel.SportEventId)
            {
                return null;
            }

            var result = await _sports.UpdateEventAsync(viewModel);
            return new ApiResult<VsandSportEvent>(result);
        }

        // DELETE: SiteApi/Sports/5/Events/12
        [HttpDelete("{sportId}/Events/{eventId}")]
        public async Task<ApiResult<VsandSportEvent>> DeleteEvent(int eventId)
        {
            ServiceResult<VsandSportEvent> result = await _sports.DeleteEventAsync(eventId);
            return new ApiResult<VsandSportEvent>(result);
        }

        // PUT: SiteApi/Sports/5/Events/SortOrder/Update
        [HttpPut("{sportId}/Events/SortOrder/Update")]
        public async Task<bool> UpdateEventOrder(int sportId, [FromBody] List<VsandSportEvent> events)
        {
            return await _sports.UpdateEventOrder(sportId, events);
        }
        #endregion

        #region Event Result Types
        // POST: SiteApi/Sports/5/EventResults
        [HttpPost("{sportId}/EventResults")]
        public async Task<ApiResult<VsandSportEventResult>> AddEventResultType(int sportId, VsandSportEventResult viewModel)
        {
            var result = await _sports.AddEventResultAsync(viewModel);
            return new ApiResult<VsandSportEventResult>(result);
        }

        // PUT: SiteApi/Sports/5/EventResults/12
        [HttpPut("{sportId}/EventResults/{eventResultId}")]
        public async Task<ApiResult<VsandSportEventResult>> UpdateEventResultType(int sportId, int eventResultId, VsandSportEventResult viewModel)
        {
            if (sportId != viewModel.SportId)
            {
                return null;
            }

            if (eventResultId != viewModel.SportEventResultId)
            {
                return null;
            }

            var result = await _sports.UpdateEventResultAsync(viewModel);
            return new ApiResult<VsandSportEventResult>(result);
        }

        // DELETE: SiteApi/Sports/5/EventResults/12
        [HttpDelete("{sportId}/EventResults/{eventResultId}")]
        public async Task<ApiResult<VsandSportEventResult>> DeleteEventResultType(int eventResultId)
        {
            ServiceResult<VsandSportEventResult> result = await _sports.DeleteEventResultAsync(eventResultId);
            return new ApiResult<VsandSportEventResult>(result);
        }

        // PUT: SiteApi/Sports/5/EventResults/SortOrder/Update
        [HttpPut("{sportId}/EventResults/SortOrder/Update")]
        public async Task<bool> UpdateEventResultTypeOrder(int sportId, [FromBody] List<VsandSportEventResult> events)
        {
            return await _sports.UpdateEventResultOrder(sportId, events);
        }
        #endregion

        #region Team Stat Categories
        //Get:  SiteApi/Sports/5/TeamStatCategories
        [HttpGet("{sportId}/TeamStatCategories")]
        public async Task<IEnumerable<TeamStatCategory>> GetTeamStatCategory(int sportId)
        {
            var result = await _sports.GetTeamStatCategoryAsync(sportId);
            return result;
        }

        // POST: SiteApi/Sports/5/TeamStatCategories
        [HttpPost("{sportId}/TeamStatCategories")]
        public async Task<ApiResult<VsandSportTeamStatCategory>> AddTeamStatCategory(int sportId, VsandSportTeamStatCategory viewModel)
        {
            var result = await _sports.AddTeamStatCategoryAsync(viewModel);
            return new ApiResult<VsandSportTeamStatCategory>(result);
        }

        // PUT: SiteApi/Sports/5/TeamStatCategories/12
        [HttpPut("{sportId}/TeamStatCategories/{teamStatCategoryId}")]
        public async Task<ApiResult<VsandSportTeamStatCategory>> UpdateTeamStatCategory(int sportId, int teamStatCategoryId, VsandSportTeamStatCategory viewModel)
        {
            if (sportId != viewModel.SportId)
            {
                return null;
            }

            if (teamStatCategoryId != viewModel.SportTeamStatCategoryId)
            {
                return null;
            }

            var result = await _sports.UpdateTeamStatCategoryAsync(viewModel);
            return new ApiResult<VsandSportTeamStatCategory>(result);
        }

        // DELETE: SiteApi/Sports/5/TeamStatCategories/12
        [HttpDelete("{sportId}/TeamStatCategories/{teamStatCategoryId}")]
        public async Task<ApiResult<VsandSportTeamStatCategory>> DeleteTeamStatCategory(int teamStatCategoryId)
        {
            ServiceResult<VsandSportTeamStatCategory> result = await _sports.DeleteTeamStatCategoryAsync(teamStatCategoryId);
            return new ApiResult<VsandSportTeamStatCategory>(result);
        }

        // PUT: SiteApi/Sports/5/TeamStatCategories/SortOrder/Update
        [HttpPut("{sportId}/TeamStatCategories/SortOrder/Update")]
        public async Task<bool> UpdateTeamStatCategoryOrder(int sportId, [FromBody] List<VsandSportTeamStatCategory> teamStatCategories)
        {
            return await _sports.UpdateTeamStatCategoryOrder(sportId, teamStatCategories);
        }
        #endregion

        #region Player Stat Categories
        //Get:  SiteApi/Sports/5/PlayerStatCategories
        [HttpGet("{sportId}/PlayerStatCategories")]
        public async Task<IEnumerable<PlayerStatCategory>> GetPlayerStatCategory(int sportId)
        {
            var result = await _sports.GetPlayerStatCategoryAsync(sportId);
            return result;
        }
        // POST: SiteApi/Sports/5/PlayerStatCategories
        [HttpPost("{sportId}/PlayerStatCategories")]
        public async Task<ApiResult<VsandSportPlayerStatCategory>> AddPlayerStatCategory(int sportId, VsandSportPlayerStatCategory viewModel)
        {
            var result = await _sports.AddPlayerStatCategoryAsync(viewModel);
            return new ApiResult<VsandSportPlayerStatCategory>(result);
        }

        // PUT: SiteApi/Sports/5/PlayerStatCategories/12
        [HttpPut("{sportId}/PlayerStatCategories/{playerStatCategoryId}")]
        public async Task<ApiResult<VsandSportPlayerStatCategory>> UpdatePlayerStatCategory(int sportId, int playerStatCategoryId, VsandSportPlayerStatCategory viewModel)
        {
            if (sportId != viewModel.SportId)
            {
                return null;
            }

            if (playerStatCategoryId != viewModel.SportPlayerStatCategoryId)
            {
                return null;
            }

            var result = await _sports.UpdatePlayerStatCategoryAsync(viewModel);
            return new ApiResult<VsandSportPlayerStatCategory>(result);
        }

        // DELETE: SiteApi/Sports/5/PlayerStatCategories/12
        [HttpDelete("{sportId}/PlayerStatCategories/{playerStatCategoryId}")]
        public async Task<ApiResult<VsandSportPlayerStatCategory>> DeletePlayerStatCategory(int playerStatCategoryId)
        {
            ServiceResult<VsandSportPlayerStatCategory> result = await _sports.DeletePlayerStatCategoryAsync(playerStatCategoryId);
            return new ApiResult<VsandSportPlayerStatCategory>(result);
        }

        // PUT: SiteApi/Sports/5/PlayerStatCategories/SortOrder/Update
        [HttpPut("{sportId}/PlayerStatCategories/SortOrder/Update")]
        public async Task<bool> UpdatePlayerStatCategoryOrder(int sportId, [FromBody] List<VsandSportPlayerStatCategory> playerStatCategories)
        {
            return await _sports.UpdatePlayerStatCategoryOrder(sportId, playerStatCategories);
        }
        #endregion

        #region Team Stats
        //Get:  SiteApi/Sports/5/TeamStats
        [HttpGet("{sportId}/TeamStats")]
        public async Task<IEnumerable<TeamStat>> GetTeamStat(int sportId)
        {
            var result = await _sports.GetTeamStatAsync(sportId);
            return result;
        }

        // POST: SiteApi/Sports/5/TeamStats/12
        [HttpPost("{sportId}/TeamStats")]
        public async Task<ApiResult<VsandSportTeamStat>> AddTeamStat(int sportId, VsandSportTeamStat viewModel)
        {
            if (sportId != viewModel.SportId)
            {
                return null;
            }

            var result = await _sports.AddTeamStatAsync(viewModel);
            return new ApiResult<VsandSportTeamStat>(result);
        }

        // PUT: SiteApi/Sports/5/TeamStats/12
        [HttpPut("{sportId}/TeamStats/{teamStatCategoryId}")]
        public async Task<ApiResult<VsandSportTeamStat>> UpdateTeamStat(int sportId, int teamStatCategoryId, VsandSportTeamStat viewModel)
        {
            if (sportId != viewModel.SportId)
            {
                return null;
            }

            if (teamStatCategoryId != viewModel.SportTeamStatCategoryId)
            {
                return null;
            }

            var result = await _sports.UpdateTeamStatAsync(viewModel);
            return new ApiResult<VsandSportTeamStat>(result);
        }

        // DELETE: SiteApi/Sports/5/TeamStats/12
        [HttpDelete("{sportId}/TeamStats/{teamStatCategoryId}")]
        public async Task<ApiResult<VsandSportTeamStat>> DeleteTeamStat(int teamStatCategoryId)
        {
            ServiceResult<VsandSportTeamStat> result = await _sports.DeleteTeamStatAsync(teamStatCategoryId);
            return new ApiResult<VsandSportTeamStat>(result);
        }

        // PUT: SiteApi/Sports/5/TeamStats/SortOrder/Update
        [HttpPut("{sportId}/TeamStats/SortOrder/Update/{teamStatCategoryId}")]
        public async Task<bool> UpdateTeamStatOrder(int teamStatCategoryId, [FromBody] List<VsandSportTeamStat> teamStats)
        {
            return await _sports.UpdateTeamStatOrder(teamStatCategoryId, teamStats);
        }

        // PUT: SiteApi/Sports/5/TeamStats/Update/Move
        [HttpPut("{sportId}/TeamStats/Update/Move")]
        public async Task<bool> MoveTeamStat([FromQuery] int teamStatCategoryId, [FromQuery] int teamStatId)
        {
            return await _sports.MoveTeamStat(teamStatCategoryId, teamStatId);
        }
        #endregion

        #region Player Stats
        //Get:  SiteApi/Sports/5/PlayerStats
        [HttpGet("{sportId}/PlayerStats")]
        public async Task<IEnumerable<PlayerStat>> GetPlayerStat(int sportId)
        {
            var result = await _sports.GetPlayerStatAsync(sportId);
            return result;
        }

        // POST: SiteApi/Sports/5/PlayerStats/12
        [HttpPost("{sportId}/PlayerStats")]
        public async Task<ApiResult<VsandSportPlayerStat>> AddPlayerStat(int sportId, VsandSportPlayerStat viewModel)
        {
            if (sportId != viewModel.SportId)
            {
                return null;
            }

            var result = await _sports.AddPlayerStatAsync(viewModel);
            return new ApiResult<VsandSportPlayerStat>(result);
        }

        // PUT: SiteApi/Sports/5/PlayerStats/12
        [HttpPut("{sportId}/PlayerStats/{playerStatCategoryId}")]
        public async Task<ApiResult<VsandSportPlayerStat>> UpdatePlayerStat(int sportId, int playerStatCategoryId, VsandSportPlayerStat viewModel)
        {
            if (sportId != viewModel.SportId)
            {
                return null;
            }

            if (playerStatCategoryId != viewModel.SportPlayerStatCategoryId)
            {
                return null;
            }

            var result = await _sports.UpdatePlayerStatAsync(viewModel);
            return new ApiResult<VsandSportPlayerStat>(result);
        }

        // DELETE: SiteApi/Sports/5/PlayerStats/12
        [HttpDelete("{sportId}/PlayerStats/{playerStatCategoryId}")]
        public async Task<ApiResult<VsandSportPlayerStat>> DeletePlayerStat(int playerStatCategoryId)
        {
            ServiceResult<VsandSportPlayerStat> result = await _sports.DeletePlayerStatAsync(playerStatCategoryId);
            return new ApiResult<VsandSportPlayerStat>(result);
        }

        // PUT: SiteApi/Sports/5/PlayerStats/SortOrder/Update
        [HttpPut("{sportId}/PlayerStats/SortOrder/Update/{playerStatCategoryId}")]
        public async Task<bool> UpdatePlayerStatOrder(int playerStatCategoryId, [FromBody] List<VsandSportPlayerStat> playerStats)
        {
            return await _sports.UpdatePlayerStatOrder(playerStatCategoryId, playerStats);
        }

        // PUT: SiteApi/Sports/5/PlayerStats/Update/Move
        [HttpPut("{sportId}/PlayerStats/Update/Move")]
        public async Task<bool> MovePlayerStat([FromQuery] int playerStatCategoryId, [FromQuery] int playerStatId)
        {
            return await _sports.MovePlayerStat(playerStatCategoryId, playerStatId);
        }
        #endregion

        //Get:  SiteApi/Sports/5/TeamStatistics
        [HttpGet("{sportId}/TeamStatistics")]
        public async Task<IEnumerable<TeamStatistics>> GetTeamStatistics(int sportId)
        {
            var result = await _sports.GetTeamStatisticsAsync(sportId);
            return result;
        }
    }
}
