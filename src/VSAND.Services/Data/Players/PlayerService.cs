using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VSAND.Data.Entities;
using VSAND.Data.Repositories;
using VSAND.Data.ViewModels;
using VSAND.Data.ViewModels.Teams;
using VSAND.Data.ViewModels.Players;
using VSAND.Services.Cache;
using VSAND.Services.StatAgg;

namespace VSAND.Services.Data.Players
{
    public class PlayerService : IPlayerService
    {
        ILogger Log = LogManager.GetCurrentClassLogger();

        private IUnitOfWork _uow;
        private ICache _cache;
        private IPlayerStatAggregation _playerStatAgg;

        public PlayerService(IUnitOfWork uow, ICache cache, IPlayerStatAggregation playerStatAgg)
        {
            _uow = uow ?? throw new ArgumentException("Unit of Work");
            _cache = cache ?? throw new ArgumentException("Cache");
            _playerStatAgg = playerStatAgg ?? throw new ArgumentException("Player Stat Aggregation");
        }

        public async Task<VsandPlayer> GetPlayerAsync(int playerId)
        {
            return await _uow.Players.GetById(playerId);
        }

        public async Task<PlayerView> GetFullPlayerViewCachedAsync(int playerId, int? viewSportId, int? viewScheduleYearId)
        {
            int sportId = 0;
            if (viewSportId.HasValue)
            {
                sportId = viewSportId.Value;
            }

            int scheduleYearId = 0;
            if (viewScheduleYearId.HasValue)
            {
                scheduleYearId = viewScheduleYearId.Value;
            }

            Log.Info($"GetFullPlayerViewCachedAsync requested with SportId: {sportId}, ScheduleYearId: {scheduleYearId}");

            string cacheKey = Cache.Keys.FullPlayerView(playerId, sportId, scheduleYearId);

            var playerView = await _cache.GetAsync<PlayerView>(cacheKey);
            if (playerView != null)
            {
                return playerView;
            }

            var player = await GetPlayerTeamsAsync(playerId);
            if (player != null)
            {
                playerView = new PlayerView();

                playerView.Player = player;

                var allSportIds = (from tr in player.TeamRosters select tr.Team.SportId).Distinct().ToList();
                var allScheduleYearIds = (from tr in player.TeamRosters select tr.Team.ScheduleYearId).Distinct().ToList();
                var allSchoolIds = (from tr in player.TeamRosters where tr.Team.SchoolId.HasValue select tr.Team.SchoolId.Value).Distinct().ToList();

                VsandTeamRoster teamRoster = null;
                List<int> positionIds = new List<int>();

                var teamId = 0;

                if (sportId > 0 && scheduleYearId > 0)
                {
                    teamRoster = (from tr in player.TeamRosters where tr.Team.SportId == sportId && tr.Team.ScheduleYearId == scheduleYearId select tr).FirstOrDefault();
                    if (teamRoster != null)
                    {
                        teamId = teamRoster.TeamId;
                    }
                }

                if (teamId == 0)
                {
                    // Look at their team info and figure out which one is the most recent based on the default event type for it
                    var schedYearId = (from tr in player.TeamRosters orderby tr.Team.ScheduleYear.EndYear descending select tr.Team.ScheduleYear.ScheduleYearId).FirstOrDefault();
                    var sportIds = (from tr in player.TeamRosters where tr.Team.ScheduleYearId == schedYearId select tr.Team.SportId).ToList();
                    // get the sportid of the event type with the latest date
                    var sportEventType = await _uow.SportEventTypes.Single(set => set.ScheduleYearId.HasValue &&
                        set.ScheduleYearId.Value == schedYearId && sportIds.Contains(set.SportId), set => set.OrderByDescending(s => s.EndDate.Value));

                    var latestSportId = 0;
                    if (sportEventType != null)
                    {
                        latestSportId = sportEventType.SportId;
                        if (latestSportId > 0)
                        {
                            teamRoster = (from tr in player.TeamRosters where tr.Team.ScheduleYearId == schedYearId && tr.Team.SportId == latestSportId select tr).FirstOrDefault();
                        }
                    }                    
                }

                if (teamRoster != null)
                {
                    playerView.TeamId = teamRoster.TeamId;

                    positionIds = teamRoster.PositionList;

                    if (positionIds.Any())
                    {
                        var positions = await _uow.SportPositions.List(sp => positionIds.Contains(sp.SportPositionId));
                        // Get the featured stats for these positions
                        foreach (var position in positions)
                        {
                            //TODO: Once the sport positions are in the context, get the list, split it, then use it to query the players stat value
                        }
                    }

                    // check to see if the player has any top 100 stats for the requested team
                    playerView.Top100Stats = await _playerStatAgg.TeamRosterEntryTop100(playerId, teamRoster.TeamId, teamRoster.Team.SportId);
                }

                // Get the slugs related to the sports they played and the schedule years they played
                playerView.Slugs.AddRange(await _uow.EntitySlugs.List(es => es.EntityType == "Sport" && allSportIds.Contains(es.EntityId)));
                playerView.Slugs.AddRange(await _uow.EntitySlugs.List(es => es.EntityType == "ScheduleYear" && allScheduleYearIds.Contains(es.EntityId)));
                playerView.Slugs.AddRange(await _uow.EntitySlugs.List(es => es.EntityType == "School" && allSchoolIds.Contains(es.EntityId)));

                //TODO: GetFullPlayerViewCachedAsync - Reinstate SetCache value for result
                //await _cache.SetAsync(cacheKey, player);
            }

            return playerView;
        }

        public async Task<VsandPlayer> GetPlayerTeamsAsync(int playerId)
        {
            return await _uow.Players.Single(p => p.PlayerId == playerId, null, new List<string> { "TeamRosters.Team.ScheduleYear", "TeamRosters.Team.Sport", "TeamRosters.Team.School" });
        }

        public async Task<PagedResult<PlayerSummary>> Search(string firstName, string lastName, int graduationYear, int schoolId, int pageSize, int pageNumber)
        {
            var oRet = await _uow.Players.Search(firstName, lastName, graduationYear, schoolId, pageSize, pageNumber);
            return oRet;
        }

        public async Task<VsandPlayer> GetPlayerStatsAsync(int playerId, int teamId)
        {
            var oRet = await _uow.Players.GetPlayerStatsAsync(playerId, teamId);
            return oRet;
        }

        public async Task<ServiceResult<VsandPlayer>> AddPlayerAsync(VsandPlayer addPlayer)
        {
            var oRet = new ServiceResult<VsandPlayer>();

            // TODO: does this need auditing?
            await _uow.Players.Insert(addPlayer);

            var bRet = await _uow.Save();
            if (bRet)
            {
                oRet.obj = addPlayer;
                oRet.Success = true;
                oRet.Id = addPlayer.PlayerId;
            }

            return oRet;
        }

        public async Task<ServiceResult<VsandPlayer>> UpdatePlayerAsync(VsandPlayer chgPlayer)
        {
            var oRet = new ServiceResult<VsandPlayer>();

            _uow.Players.Update(chgPlayer);

            var bRet = await _uow.Save();

            if (bRet)
            {
                oRet.obj = chgPlayer;
                oRet.Success = true;
                oRet.Id = chgPlayer.PlayerId;
            }

            return oRet;
        }

        public async Task<ServiceResult<VsandPlayer>> DeletePlayerAsync(int playerId)
        {
            var oRet = new ServiceResult<VsandPlayer>();

            VsandPlayer remPlayer = await GetPlayerAsync(playerId);

            if (remPlayer == null)
            {
                oRet.Success = false;
                oRet.Id = playerId;
                oRet.Message = "There was no player found with Id " + playerId;
            }

            await _uow.Players.Delete(remPlayer.PlayerId);

            var bRet = await _uow.Save();

            if (bRet)
            {
                oRet.obj = remPlayer;
                oRet.Success = true;
                oRet.Id = remPlayer.PlayerId;
            }

            return oRet;
        }

        public async Task<List<VsandPlayer>> GetListAsync(int teamId)
        {
            var oRet = new List<VsandPlayer>();

            var oTeam = await _uow.Teams.GetTeamAsync(teamId);
            var oRoster = oTeam.RosterEntries;

            oRet = oRoster.Select(gr => new VsandPlayer(gr)).ToList();

            return oRet;
        }
    }
}
