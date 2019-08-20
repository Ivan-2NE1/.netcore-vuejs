using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VSAND.Data.Entities;
using VSAND.Data.Repositories;
using VSAND.Data.ViewModels;
using VSAND.Data.ViewModels.Sports;
using VSAND.Services.Cache;
using VSAND.Services.Data.CMS;

namespace VSAND.Services.Data.Sports
{
    public class SportService : ISportService
    {
        private IUnitOfWork _uow;
        private ICache _cache;
        private IConfigService _appxConfig;

        public SportService(IUnitOfWork uow, ICache cache, IConfigService appxConfig)
        {
            _uow = uow ?? throw new ArgumentException("Unit of Work is null");
            _cache = cache ?? throw new ArgumentException("Cache is null");
            _appxConfig = appxConfig ?? throw new ArgumentException("AppxConfig is null");
        }

        public async Task<VsandSport> GetSportAsync(int sportId)
        {
            return await _uow.Sports.GetById(sportId);
        }

        public async Task<VsandSport> GetFullSportCachedAsync(int sportId)
        {
            string sCacheKey = Cache.Keys.FullSport(sportId);

            var cachedSport = await _cache.GetAsync<VsandSport>(sCacheKey);
            if (cachedSport != null)
            {
                return cachedSport;
            }

            // TODO: GetFullSport should have cache that expires when any related piece of sport configuration changes

            var oSport = await _uow.Sports.Single(s => s.SportId == sportId, null,
                new List<string> {
                    "SportEvents.VsandSportEventStat",
                    "EventResults",
                    "GameMeta",
                    "Positions",
                    "PlayerStatCategories.PlayerStats",
                    "TeamStatCategories.TeamStats"
                });

            await _cache.SetAsync(sCacheKey, oSport);

            return oSport;
        }

        public async Task<IEnumerable<VsandSport>> GetListAsync()
        {
            return await _uow.Sports.List(null, x => x.OrderBy(s => s.SportId));
        }

        public async Task<IEnumerable<ListItem<int>>> GetActiveListAsync()
        {
            string sCacheKey = Cache.Keys.ActiveSportList();

            var cachedList = await _cache.GetAsync<IEnumerable<ListItem<int>>>(sCacheKey);
            if (cachedList != null)
            {
                return cachedList;
            }

            var oSports = await _uow.Sports.List(s => s.Enabled, x => x.OrderBy(s => s.Name));
            var oRet = (from s in oSports select new ListItem<int> { id = s.SportId, name = s.Name });

            await _cache.SetAsync(sCacheKey, oRet);

            return oRet;
        }

        public async Task<ServiceResult<VsandSport>> AddSportAsync(VsandSport addSport)
        {
            var oRet = new ServiceResult<VsandSport>();

            // TODO: check for duplicate sports here

            await _uow.Sports.Insert(addSport);
            var bRet = await _uow.Save();

            if (bRet)
            {
                oRet.obj = addSport;
                oRet.Success = true;
                oRet.Id = addSport.SportId;

                // TODO: This is the layer that the cache engine should be invoked for Sports (frequently used)
            }

            return oRet;
        }

        public async Task<ServiceResult<VsandSport>> UpdateSportAsync(VsandSport chgSport)
        {
            var oRet = new ServiceResult<VsandSport>();

            _uow.Sports.Update(chgSport);

            var bRet = await _uow.Save();

            if (bRet)
            {
                oRet.obj = chgSport;
                oRet.Success = true;
                oRet.Id = chgSport.SportId;

                // TODO: This is the layer that the cache engine should be invoked for Sports (frequently used)
                await _cache.RemoveAsync("Sport" + chgSport.SportId);
            }

            return oRet;
        }

        #region Positions
        public async Task<VsandSport> GetPositionsAsync(int sportId)
        {
            var oSport = await _uow.Sports.Single(s => s.SportId == sportId, null, new List<string> {
                    "Positions"
                });

            // preserve sort order
            oSport.Positions = oSport.Positions.OrderBy(p => p.SortOrder).ToList();

            return oSport;
        }

        public async Task<ServiceResult<VsandSportPosition>> UpdatePositionAsync(VsandSportPosition chgPosition)
        {
            var oRet = new ServiceResult<VsandSportPosition>();

            _uow.SportPositions.Update(chgPosition);

            var bRet = await _uow.Save();

            if (bRet)
            {
                oRet.obj = chgPosition;
                oRet.Success = true;
                oRet.Id = chgPosition.SportPositionId;
            }

            return oRet;
        }

        public async Task<ServiceResult<VsandSportPosition>> AddPositionAsync(VsandSportPosition addPosition)
        {
            var oRet = new ServiceResult<VsandSportPosition>();

            await _uow.SportPositions.Insert(addPosition);

            var bRet = await _uow.Save();

            if (bRet)
            {
                oRet.obj = addPosition;
                oRet.Success = true;
                oRet.Id = addPosition.SportPositionId;
            }

            return oRet;
        }
        #endregion

        #region Game Meta
        public async Task<VsandSport> GetGameMetaAsync(int sportId)
        {
            var oSport = await _uow.Sports.Single(s => s.SportId == sportId, null, new List<string> {
                    "GameMeta"
                });

            // maintain sorted order
            oSport.GameMeta = oSport.GameMeta.OrderBy(gm => gm.SortOrder).ToList();

            return oSport;
        }

        public async Task<ServiceResult<VsandSportGameMeta>> AddGameMetaAsync(VsandSportGameMeta addGameMeta)
        {
            var oRet = new ServiceResult<VsandSportGameMeta>();

            await _uow.SportGameMeta.Insert(addGameMeta);

            var bRet = await _uow.Save();

            if (bRet)
            {
                oRet.obj = addGameMeta;
                oRet.Success = true;
                oRet.Id = addGameMeta.SportGameMetaId;
            }

            return oRet;
        }

        public async Task<ServiceResult<VsandSportGameMeta>> UpdateGameMetaAsync(VsandSportGameMeta chgGameMeta)
        {
            var oRet = new ServiceResult<VsandSportGameMeta>();

            _uow.SportGameMeta.Update(chgGameMeta);

            var bRet = await _uow.Save();

            if (bRet)
            {
                oRet.obj = chgGameMeta;
                oRet.Success = true;
                oRet.Id = chgGameMeta.SportGameMetaId;
            }

            return oRet;
        }

        public async Task<ServiceResult<VsandSportGameMeta>> DeleteGameMetaAsync(int gameMetaId)
        {
            var oRet = new ServiceResult<VsandSportGameMeta>();

            VsandSportGameMeta remGameMeta = await _uow.SportGameMeta.GetById(gameMetaId);
            await _uow.SportGameMeta.Delete(remGameMeta.SportGameMetaId);

            var bRet = await _uow.Save();

            if (bRet)
            {
                oRet.obj = remGameMeta;
                oRet.Success = true;
                oRet.Id = remGameMeta.SportGameMetaId;
            }

            return oRet;
        }

        public async Task<bool> UpdateGameMetaOrder(int sportId, List<VsandSportGameMeta> gameMeta)
        {
            bool bRet = await _uow.SportGameMeta.UpdateOrder(gameMeta, gm => gm.SportId == sportId, gm => gm.SportGameMetaId, (gm, order) => gm.SortOrder = order);

            if (!bRet)
            {
                return false;
            }

            return await _uow.Save();
        }
        #endregion

        #region Events
        public async Task<VsandSport> GetEventsAsync(int sportId)
        {
            var oSport = await _uow.Sports.Single(s => s.SportId == sportId, null, new List<string> {
                    "SportEvents.EventStats",
                    "EventResults"
                });

            // preserve sort order
            oSport.SportEvents = oSport.SportEvents.OrderBy(e => e.DefaultSort).ToList();
            oSport.EventResults = oSport.EventResults.OrderBy(er => er.SortOrder).ToList();

            return oSport;
        }

        public async Task<ServiceResult<VsandSportEvent>> AddEventAsync(VsandSportEvent addEvent)
        {
            var oRet = new ServiceResult<VsandSportEvent>();

            await _uow.SportEvents.Insert(addEvent);

            var bRet = await _uow.Save();

            if (bRet)
            {
                oRet.obj = addEvent;
                oRet.Success = true;
                oRet.Id = addEvent.SportEventId;
            }

            return oRet;
        }

        public async Task<ServiceResult<VsandSportEvent>> UpdateEventAsync(VsandSportEvent chgEvent)
        {
            var oRet = new ServiceResult<VsandSportEvent>();

            _uow.SportEvents.Update(chgEvent);

            var bRet = await _uow.Save();

            if (bRet)
            {
                oRet.obj = chgEvent;
                oRet.Success = true;
                oRet.Id = chgEvent.SportEventId;
            }

            return oRet;
        }

        public async Task<ServiceResult<VsandSportEvent>> DeleteEventAsync(int eventId)
        {
            var oRet = new ServiceResult<VsandSportEvent>();

            VsandSportEvent remEvent = await _uow.SportEvents.GetById(eventId);
            await _uow.SportEvents.Delete(remEvent.SportEventId);

            var bRet = await _uow.Save();

            if (bRet)
            {
                oRet.obj = remEvent;
                oRet.Success = true;
                oRet.Id = remEvent.SportEventId;
            }

            return oRet;
        }

        public async Task<bool> UpdateEventOrder(int sportId, List<VsandSportEvent> events)
        {
            bool bRet = await _uow.SportEvents.UpdateOrder(events, e => e.SportId == sportId, e => e.SportEventId, (e, order) => e.DefaultSort = order);

            if (!bRet)
            {
                return false;
            }

            return await _uow.Save();
        }
        #endregion

        #region Event Results
        public async Task<ServiceResult<VsandSportEventResult>> AddEventResultAsync(VsandSportEventResult addEventResult)
        {
            var oRet = new ServiceResult<VsandSportEventResult>();

            await _uow.SportEventResults.Insert(addEventResult);

            var bRet = await _uow.Save();

            if (bRet)
            {
                oRet.obj = addEventResult;
                oRet.Success = true;
                oRet.Id = addEventResult.SportEventResultId;
            }

            return oRet;
        }

        public async Task<ServiceResult<VsandSportEventResult>> UpdateEventResultAsync(VsandSportEventResult chgEventResult)
        {
            var oRet = new ServiceResult<VsandSportEventResult>();

            _uow.SportEventResults.Update(chgEventResult);

            var bRet = await _uow.Save();

            if (bRet)
            {
                oRet.obj = chgEventResult;
                oRet.Success = true;
                oRet.Id = chgEventResult.SportEventResultId;
            }

            return oRet;
        }

        public async Task<ServiceResult<VsandSportEventResult>> DeleteEventResultAsync(int eventResultId)
        {
            var oRet = new ServiceResult<VsandSportEventResult>();

            VsandSportEventResult remEventResult = await _uow.SportEventResults.GetById(eventResultId);
            await _uow.SportEventResults.Delete(remEventResult.SportEventResultId);

            var bRet = await _uow.Save();

            if (bRet)
            {
                oRet.obj = remEventResult;
                oRet.Success = true;
                oRet.Id = remEventResult.SportEventResultId;
            }

            return oRet;
        }

        public async Task<bool> UpdateEventResultOrder(int sportId, List<VsandSportEventResult> eventResults)
        {
            bool bRet = await _uow.SportEventResults.UpdateOrder(eventResults, er => er.SportId == sportId, er => er.SportEventResultId, (er, order) => er.SortOrder = order);

            if (!bRet)
            {
                return false;
            }

            return await _uow.Save();
        }
        #endregion

        #region Team Stat Categories
        public async Task<VsandSport> GetTeamStatsAsync(int sportId)
        {
            var oSport = await _uow.Sports.Single(s => s.SportId == sportId, null, new List<string> {
                "TeamStatCategories.TeamStats"
            });

            // preserve sort order
            oSport.TeamStatCategories = oSport.TeamStatCategories.OrderBy(tsc => tsc.SortOrder).ToList();

            foreach (var teamStatCategory in oSport.TeamStatCategories)
            {
                teamStatCategory.TeamStats = teamStatCategory.TeamStats.OrderBy(tsc => tsc.SortOrder).ToList();
            }

            return oSport;
        }

        public async Task<IEnumerable<TeamStatCategory>> GetTeamStatCategoryAsync(int sportId)
        {
            var oRet = new List<TeamStatCategory>();
            if (sportId <= 0) return oRet;

            var oTeamStatCategories = await _uow.SportTeamStatCategories.List(et => et.SportId == sportId);
            var oActive = oTeamStatCategories.FirstOrDefault();
            if (oActive != null)
            {
                oRet.AddRange(from sy in oTeamStatCategories select new TeamStatCategory(sy));
            }

            return oRet;
        }

        public async Task<ServiceResult<VsandSportTeamStatCategory>> AddTeamStatCategoryAsync(VsandSportTeamStatCategory addTeamStatCategory)
        {
            var oRet = new ServiceResult<VsandSportTeamStatCategory>();

            await _uow.SportTeamStatCategories.Insert(addTeamStatCategory);

            var bRet = await _uow.Save();

            if (bRet)
            {
                oRet.obj = addTeamStatCategory;
                oRet.Success = true;
                oRet.Id = addTeamStatCategory.SportTeamStatCategoryId;
            }

            return oRet;
        }

        public async Task<ServiceResult<VsandSportTeamStatCategory>> UpdateTeamStatCategoryAsync(VsandSportTeamStatCategory chgTeamStatCategory)
        {
            var oRet = new ServiceResult<VsandSportTeamStatCategory>();

            _uow.SportTeamStatCategories.Update(chgTeamStatCategory);

            var bRet = await _uow.Save();

            if (bRet)
            {
                oRet.obj = chgTeamStatCategory;
                oRet.Success = true;
                oRet.Id = chgTeamStatCategory.SportTeamStatCategoryId;
            }

            return oRet;
        }

        public async Task<ServiceResult<VsandSportTeamStatCategory>> DeleteTeamStatCategoryAsync(int teamStatCategoryId)
        {
            var oRet = new ServiceResult<VsandSportTeamStatCategory>();

            VsandSportTeamStatCategory remTeamStatCategory = await _uow.SportTeamStatCategories.GetById(teamStatCategoryId);
            await _uow.SportTeamStatCategories.Delete(remTeamStatCategory.SportTeamStatCategoryId);

            var bRet = await _uow.Save();

            if (bRet)
            {
                oRet.obj = remTeamStatCategory;
                oRet.Success = true;
                oRet.Id = remTeamStatCategory.SportTeamStatCategoryId;
            }

            return oRet;
        }

        public async Task<bool> UpdateTeamStatCategoryOrder(int sportId, List<VsandSportTeamStatCategory> teamStatCategories)
        {
            bool bRet = await _uow.SportTeamStatCategories.UpdateOrder(teamStatCategories, tsc => tsc.SportId == sportId, tsc => tsc.SportTeamStatCategoryId, (tsc, order) => tsc.SortOrder = order);

            if (!bRet)
            {
                return false;
            }

            return await _uow.Save();
        }
        #endregion

        #region Player Stat Categories
        public async Task<VsandSport> GetPlayerStatsAsync(int sportId)
        {
            var oSport = await _uow.Sports.Single(s => s.SportId == sportId, null, new List<string> {
                "PlayerStatCategories.PlayerStats"
            });

            // preserve sort order
            oSport.PlayerStatCategories = oSport.PlayerStatCategories.OrderBy(psc => psc.SortOrder).ToList();

            foreach (var playerStatCategory in oSport.PlayerStatCategories)
            {
                playerStatCategory.PlayerStats = playerStatCategory.PlayerStats.OrderBy(psc => psc.SortOrder).ToList();
            }

            return oSport;
        }

        public async Task<IEnumerable<PlayerStatCategory>> GetPlayerStatCategoryAsync(int sportId)
        {
            var oRet = new List<PlayerStatCategory>();
            if (sportId <= 0) return oRet;

            var oPlayerStatCategories = await _uow.SportPlayerStatCategories.List(et => et.SportId == sportId);
            var oActive = oPlayerStatCategories.FirstOrDefault();
            if (oActive != null)
            {
                oRet.AddRange(from sy in oPlayerStatCategories select new PlayerStatCategory(sy));
            }

            return oRet;
        }

        public async Task<ServiceResult<VsandSportPlayerStatCategory>> AddPlayerStatCategoryAsync(VsandSportPlayerStatCategory addPlayerStatCategory)
        {
            var oRet = new ServiceResult<VsandSportPlayerStatCategory>();

            await _uow.SportPlayerStatCategories.Insert(addPlayerStatCategory);

            var bRet = await _uow.Save();

            if (bRet)
            {
                oRet.obj = addPlayerStatCategory;
                oRet.Success = true;
                oRet.Id = addPlayerStatCategory.SportPlayerStatCategoryId;
            }

            return oRet;
        }

        public async Task<ServiceResult<VsandSportPlayerStatCategory>> UpdatePlayerStatCategoryAsync(VsandSportPlayerStatCategory chgPlayerStatCategory)
        {
            var oRet = new ServiceResult<VsandSportPlayerStatCategory>();

            _uow.SportPlayerStatCategories.Update(chgPlayerStatCategory);

            var bRet = await _uow.Save();

            if (bRet)
            {
                oRet.obj = chgPlayerStatCategory;
                oRet.Success = true;
                oRet.Id = chgPlayerStatCategory.SportPlayerStatCategoryId;
            }

            return oRet;
        }

        public async Task<ServiceResult<VsandSportPlayerStatCategory>> DeletePlayerStatCategoryAsync(int playerStatCategoryId)
        {
            var oRet = new ServiceResult<VsandSportPlayerStatCategory>();

            VsandSportPlayerStatCategory remPlayerStatCategory = await _uow.SportPlayerStatCategories.GetById(playerStatCategoryId);
            await _uow.SportPlayerStatCategories.Delete(remPlayerStatCategory.SportPlayerStatCategoryId);

            var bRet = await _uow.Save();

            if (bRet)
            {
                oRet.obj = remPlayerStatCategory;
                oRet.Success = true;
                oRet.Id = remPlayerStatCategory.SportPlayerStatCategoryId;
            }

            return oRet;
        }

        public async Task<bool> UpdatePlayerStatCategoryOrder(int sportId, List<VsandSportPlayerStatCategory> playerStatCategories)
        {
            bool bRet = await _uow.SportPlayerStatCategories.UpdateOrder(playerStatCategories, psc => psc.SportId == sportId, psc => psc.SportPlayerStatCategoryId, (psc, order) => psc.SortOrder = order);

            if (!bRet)
            {
                return false;
            }

            return await _uow.Save();
        }
        #endregion

        #region Team Stats
        public async Task<IEnumerable<TeamStat>> GetTeamStatAsync(int sportId)
        {
            var oRet = new List<TeamStat>();
            if (sportId <= 0) return oRet;

            var oTeamStats = await _uow.SportTeamStats.List(et => et.SportId == sportId);
            var oActive = oTeamStats.FirstOrDefault();
            if (oActive != null)
            {
                oRet.AddRange(from sy in oTeamStats select new TeamStat(sy));
            }

            return oRet;
        }

        public async Task<ServiceResult<VsandSportTeamStat>> AddTeamStatAsync(VsandSportTeamStat addTeamStat)
        {
            var oRet = new ServiceResult<VsandSportTeamStat>();

            await _uow.SportTeamStats.Insert(addTeamStat);

            var bRet = await _uow.Save();

            if (bRet)
            {
                oRet.obj = addTeamStat;
                oRet.Success = true;
                oRet.Id = addTeamStat.SportTeamStatId;
            }

            return oRet;
        }

        public async Task<ServiceResult<VsandSportTeamStat>> UpdateTeamStatAsync(VsandSportTeamStat chgTeamStat)
        {
            var oRet = new ServiceResult<VsandSportTeamStat>();

            _uow.SportTeamStats.Update(chgTeamStat);

            var bRet = await _uow.Save();

            if (bRet)
            {
                oRet.obj = chgTeamStat;
                oRet.Success = true;
                oRet.Id = chgTeamStat.SportTeamStatId;
            }

            return oRet;
        }

        public async Task<ServiceResult<VsandSportTeamStat>> DeleteTeamStatAsync(int teamStatCategoryId)
        {
            var oRet = new ServiceResult<VsandSportTeamStat>();

            VsandSportTeamStat remTeamStat = await _uow.SportTeamStats.GetById(teamStatCategoryId);
            await _uow.SportTeamStats.Delete(remTeamStat.SportTeamStatId);

            var bRet = await _uow.Save();

            if (bRet)
            {
                oRet.obj = remTeamStat;
                oRet.Success = true;
                oRet.Id = remTeamStat.SportTeamStatId;
            }

            return oRet;
        }

        public async Task<bool> UpdateTeamStatOrder(int teamStatCategoryId, List<VsandSportTeamStat> teamStats)
        {
            bool bRet = await _uow.SportTeamStats.UpdateOrder(teamStats, ps => ps.SportTeamStatCategoryId == teamStatCategoryId, ps => ps.SportTeamStatId, (ps, order) => ps.SortOrder = order);

            if (!bRet)
            {
                return false;
            }

            return await _uow.Save();
        }

        public async Task<bool> MoveTeamStat(int teamStatCategoryId, int teamStatId)
        {
            VsandSportTeamStat teamStat = await _uow.SportTeamStats.GetById(teamStatId);

            teamStat.SportTeamStatCategoryId = teamStatCategoryId;
            _uow.SportTeamStats.Update(teamStat);

            return await _uow.Save();
        }
        #endregion

        #region Player Stats
        public async Task<IEnumerable<PlayerStat>> GetPlayerStatAsync(int sportId)
        {
            var oRet = new List<PlayerStat>();
            if (sportId <= 0) return oRet;

            var oPlayerStats = await _uow.SportPlayerStats.List(et => et.SportId == sportId);
            var oActive = oPlayerStats.FirstOrDefault();
            if (oActive != null)
            {
                oRet.AddRange(from sy in oPlayerStats select new PlayerStat(sy));
            }

            return oRet;
        }

        public async Task<ServiceResult<VsandSportPlayerStat>> AddPlayerStatAsync(VsandSportPlayerStat addPlayerStat)
        {
            var oRet = new ServiceResult<VsandSportPlayerStat>();

            await _uow.SportPlayerStats.Insert(addPlayerStat);

            var bRet = await _uow.Save();

            if (bRet)
            {
                oRet.obj = addPlayerStat;
                oRet.Success = true;
                oRet.Id = addPlayerStat.SportPlayerStatId;
            }

            return oRet;
        }

        public async Task<ServiceResult<VsandSportPlayerStat>> UpdatePlayerStatAsync(VsandSportPlayerStat chgPlayerStat)
        {
            var oRet = new ServiceResult<VsandSportPlayerStat>();

            _uow.SportPlayerStats.Update(chgPlayerStat);

            var bRet = await _uow.Save();

            if (bRet)
            {
                oRet.obj = chgPlayerStat;
                oRet.Success = true;
                oRet.Id = chgPlayerStat.SportPlayerStatId;
            }

            return oRet;
        }

        public async Task<ServiceResult<VsandSportPlayerStat>> DeletePlayerStatAsync(int playerStatCategoryId)
        {
            var oRet = new ServiceResult<VsandSportPlayerStat>();

            VsandSportPlayerStat remPlayerStat = await _uow.SportPlayerStats.GetById(playerStatCategoryId);
            await _uow.SportPlayerStats.Delete(remPlayerStat.SportPlayerStatId);

            var bRet = await _uow.Save();

            if (bRet)
            {
                oRet.obj = remPlayerStat;
                oRet.Success = true;
                oRet.Id = remPlayerStat.SportPlayerStatId;
            }

            return oRet;
        }

        public async Task<bool> UpdatePlayerStatOrder(int playerStatCategoryId, List<VsandSportPlayerStat> playerStats)
        {
            bool bRet = await _uow.SportPlayerStats.UpdateOrder(playerStats, ps => ps.SportPlayerStatCategoryId == playerStatCategoryId, ps => ps.SportPlayerStatId, (ps, order) => ps.SortOrder = order);

            if (!bRet)
            {
                return false;
            }

            return await _uow.Save();
        }

        public async Task<bool> MovePlayerStat(int playerStatCategoryId, int playerStatId)
        {
            VsandSportPlayerStat playerStat = await _uow.SportPlayerStats.GetById(playerStatId);

            playerStat.SportPlayerStatCategoryId = playerStatCategoryId;
            _uow.SportPlayerStats.Update(playerStat);

            return await _uow.Save();
        }
        #endregion

        #region Front-End Display

        public SortedList<int, string> GetSeasons(string activeSeason)
        {
            var oRet = new SortedList<int, string>();

            switch (activeSeason.ToLowerInvariant())
            {
                case "fall":
                    oRet.Add(0, "Fall");
                    oRet.Add(1, "Winter");
                    oRet.Add(2, "Spring");
                    break;
                case "winter":
                    oRet.Add(0, "Winter");
                    oRet.Add(1, "Spring");
                    oRet.Add(2, "Fall");
                    break;
                case "spring":
                    oRet.Add(0, "Spring");
                    oRet.Add(1, "Fall");
                    oRet.Add(2, "Winter");
                    break;
            }

            return oRet;
        }

        public async Task<List<ListItem<string>>> GetFeaturedSportItemsCachedAsync()
        {
            // This has cache depenency on AppConfig where ConfigCat = FrontEnd and ConfigName = FeatureSports
            // Get the list of sport ids that are marked as featured from the CMS Settings
            // Convert those sport ids in the list of slug + name

            // Get the season!
            var season = await _appxConfig.GetConfigValueAsync("FrontEnd", "ActiveSeason");
            if (string.IsNullOrEmpty(season))
            {
                // use the date to determine the season we are in
                season = VSAND.Common.DateHelp.SeasonName();
            }

            string cacheKey = Cache.Keys.FrontEndFeaturedSports(season);

            var featuredSportList = await _cache.GetAsync<List<ListItem<string>>>(cacheKey);
            if (featuredSportList == null || !featuredSportList.Any())
            {
                // get the actual feature sports config value
                string featuredSportIds = await _appxConfig.GetConfigValueAsync("FrontEnd", "FeaturedSportIds" + season);
                if (string.IsNullOrWhiteSpace(featuredSportIds))
                {
                    //TODO: we need to solve for this empty list. Something from the sports table that makes sense for the current date
                }

                List<int> sportIds = (from s in featuredSportIds.Split(new char[] { ',', ';' }) select int.Parse(s)).ToList();

                // get the sport id and names for the items in sportIds
                var oSports = await _uow.Sports.List(s => sportIds.Contains(s.SportId));

                // get the entity slug for the items in sportIds
                var oSlugs = await _uow.EntitySlugs.List(s => s.EntityType == "Sport" && sportIds.Contains(s.EntityId));

                featuredSportList = new List<ListItem<string>>();
                foreach (int sportId in sportIds)
                {
                    var oSlug = oSlugs.FirstOrDefault(s => s.EntityId == sportId);
                    var oSport = oSports.FirstOrDefault(s => s.SportId == sportId);
                    if (oSlug != null && oSport != null)
                    {
                        var featuredSport = new ListItem<string>(oSlug.Slug, oSport.Name);
                        featuredSportList.Add(featuredSport);
                    }
                }

                // Save this back into the cache!
                await _cache.SetAsync(cacheKey, featuredSportList);
            }

            return featuredSportList;
        }

        public async Task<SortedList<string, List<ListItem<string>>>> GetSportNavBySeasonAsync()
        {
            var oRet = new SortedList<string, List<ListItem<string>>>();

            var oSlugs = await _uow.EntitySlugs.List(s => s.EntityType == "Sport");
            var oSports = await _uow.Sports.List(s => s.Enabled == true);
            foreach (var oSport in oSports)
            {
                var oSlug = oSlugs.FirstOrDefault(s => s.EntityId == oSport.SportId);
                if (oSlug != null)
                {
                    string season = oSport.Season;
                    if (!oRet.ContainsKey(season))
                    {
                        oRet.Add(season, new List<ListItem<string>>());
                    }

                    var item = new ListItem<string>(oSlug.Slug, oSport.Name);
                    oRet[season].Add(item);
                }
            }
            return oRet;
        }

        public async Task<Dictionary<string, List<ListItem<string>>>> GetSportNavByGenderAsync()
        {
            var oRet = new Dictionary<string, List<ListItem<string>>>();

            var outputGenders = new List<string> { "boys", "girls" };
            var oSlugs = await _uow.EntitySlugs.List(s => s.EntityType == "Sport");
            var oSports = await _uow.Sports.List(s => s.Enabled == true, s => s.OrderBy(n => n.NeutralName));
            foreach (var oSport in oSports)
            {
                var oSlug = oSlugs.FirstOrDefault(s => s.EntityId == oSport.SportId);
                if (oSlug != null)
                {
                    string gender = oSport.Gender.Trim().ToLower();
                    if (outputGenders.Contains(gender))
                    {
                        if (!oRet.ContainsKey(gender))
                        {
                            oRet.Add(gender, new List<ListItem<string>>());
                        }

                        var item = new ListItem<string>(oSlug.Slug, oSport.NeutralName);
                        oRet[gender].Add(item);
                    }                    
                }
            }
            return oRet;
        }

        public async Task<Dictionary<string, List<ListItem<string>>>> GetSportNavByGenderCachedAsync()
        {
            string cacheKey = Cache.Keys.FrontEndSportsByGender();

            var oRet = await _cache.GetAsync<Dictionary<string, List<ListItem<string>>>>(cacheKey);
            
            if (oRet != null && oRet.Any())
            {
                return oRet;
            }

            oRet = await GetSportNavByGenderAsync();
            if (oRet != null && oRet.Any())
            {
                await _cache.SetAsync(cacheKey, oRet);
            }

            return oRet;
        }

        public async Task<SortedList<string, List<ListItem<string>>>> GetSportNavBySeasonCachedAsync()
        {
            string cacheKey = Cache.Keys.FrontEndSportsBySeason();

            SortedList<string, List<ListItem<string>>> oRet = await _cache.GetAsync<SortedList<string, List<ListItem<string>>>>(cacheKey);

            if (oRet == null || !oRet.Any())
            {
                oRet = await GetSportNavBySeasonAsync();
                await _cache.SetAsync(cacheKey, oRet);
            }

            return oRet;
        }

        public async Task<StandingsView> GetSportsStandingViewAsync(int sportId, int scheduleYearId, string conference)
        {
            var oRet = new StandingsView();

            oRet.Sport = await _uow.Sports.GetById(sportId);
            oRet.ScheduleYear = await _uow.ScheduleYears.GetById(scheduleYearId);

            var sportSlug = await _uow.EntitySlugs.Single(es => es.EntityType == "Sport" && es.EntityId == sportId);
            if (sportSlug != null)
            {
                oRet.SportSlug = sportSlug.Slug;
            }

            var scheduleYearSlug = await _uow.EntitySlugs.Single(es => es.EntityType == "ScheduleYear" && es.EntityId == scheduleYearId);
            if (scheduleYearSlug != null)
            {
                oRet.ScheduleYearSlug = scheduleYearSlug.Slug;
            }

            var customCodes = await _uow.TeamCustomCodes.List(tcc => tcc.CodeName == "Conference" && tcc.Team.SportId == sportId && tcc.Team.ScheduleYearId == scheduleYearId && tcc.Team.School.CoreCoverage == true);
            oRet.Conferences = (from cc in customCodes orderby cc.CodeValue ascending select cc.CodeValue).Distinct().ToList();

            if (!string.IsNullOrWhiteSpace(conference))
            {
                if (!oRet.Conferences.Any(c => c.Equals(conference, StringComparison.OrdinalIgnoreCase)))
                {
                    // A conference name was requested that doesn't exist in the current year.
                    return oRet;
                }

                // Load the conference standing results
                oRet.Conference = conference;

                // Get the teams that meet our filter criteria
                var oTeams = await _uow.Teams.List(t => t.SportId == sportId &&
                    t.ScheduleYearId == scheduleYearId &&
                    t.CustomCodes.Any(tcc => tcc.CodeName == "Conference" && tcc.CodeValue == conference), null,
                    new List<string> { "School", "CustomCodes" });

                oRet.Standings = (from t in oTeams select new TeamRecordInfo(t, true)).ToList();

                // for each team in the list, get their school's slug
                var schoolIds = (from t in oTeams where t.SchoolId.HasValue select t.SchoolId.Value).ToList();
                var schoolSlugs = await _uow.EntitySlugs.List(es => es.EntityType == "School" && schoolIds.Contains(es.EntityId));
                foreach(var slug in schoolSlugs)
                {
                    oRet.SchoolSlugs.Add(slug.EntityId, slug.Slug);
                }
            }

            return oRet;
        }

        /// <summary>
        /// Get the view model that shows for a sport and schedule year the conferences, 
        /// and if a conference is selected, the conference standings.
        /// Value is obtained from cache, or created and cached using GetSportsStandingsViewAsync.
        /// </summary>
        /// <param name="sportId">Required. The unique sport id.</param>
        /// <param name="scheduleYearId">Optional. The schedule year id. If not suppleid, the active schedule year will be used.</param>
        /// <param name="conference">Optional. If not supplied, the standings list is empty, and only the conference list is populated.</param>
        /// <returns></returns>
        public async Task<StandingsView> GetSportStandingsViewCachedAsync(int sportId, int? scheduleYearId, string conference)
        {
            var syid = 0;
            if (!scheduleYearId.HasValue)
            {
                syid = await _uow.ScheduleYears.GetActiveScheduleYearIdAsync();
            }
            else
            {
                syid = scheduleYearId.Value;
            }

            string cacheKey = Cache.Keys.FrontEndSportStandingView(sportId, syid, conference);

            StandingsView oRet = await _cache.GetAsync<StandingsView>(cacheKey);
            if (oRet != null)
            {
                return oRet;
            }

            oRet = await GetSportsStandingViewAsync(sportId, syid, conference);
            if (oRet != null)
            {
                await _cache.SetAsync(cacheKey, oRet);
            }

            return oRet;
        }

        // Powerpoints
        public async Task<PowerpointsView> GetSportsPowerpointsViewAsync(int sportId, int scheduleYearId, string section, string group)
        {
            var oRet = new PowerpointsView();

            oRet.Sport = await _uow.Sports.GetById(sportId);
            oRet.ScheduleYear = await _uow.ScheduleYears.GetById(scheduleYearId);

            var sportSlug = await _uow.EntitySlugs.Single(es => es.EntityType == "Sport" && es.EntityId == scheduleYearId);
            if (sportSlug != null)
            {
                oRet.SportSlug = sportSlug.Slug;
            }

            var scheduleYearSlug = await _uow.EntitySlugs.Single(es => es.EntityType == "ScheduleYear" && es.EntityId == scheduleYearId);
            if (scheduleYearSlug != null)
            {
                oRet.ScheduleYearSlug = scheduleYearSlug.Slug;
            }

            var customCodes = await _uow.TeamCustomCodes.List(tcc => (tcc.CodeName == "Section" || tcc.CodeName == "Group") && tcc.Team.SportId == sportId && tcc.Team.ScheduleYearId == scheduleYearId && tcc.Team.School.CoreCoverage == true);
            //var sectionCodes = customCodes.Where(cc => cc.CodeName.Equals("Section", StringComparison.OrdinalIgnoreCase)).ToList();
            //var groupCodes = customCodes.Where(cc => cc.CodeName.Equals("Group", StringComparison.OrdinalIgnoreCase)).ToList();

            //var classifications = (from s in sectionCodes join g in groupCodes on s.TeamId equals g.TeamId select s.CodeValue + ", " + g.CodeValue).Distinct().ToList();

            var classifications = new List<ListItem<string>>();

            foreach (var sectionCc in customCodes.Where(cc => cc.CodeName == "Section"))
            {
                string classification = sectionCc.CodeValue.Trim();
                string classificationVal = classification;
                var teamId = sectionCc.TeamId;
                var groupCc = customCodes.FirstOrDefault(cc => cc.CodeName == "Group" && cc.TeamId == teamId);
                if (groupCc != null)
                {
                    string groupName = groupCc.CodeValue.Trim();
                    classificationVal = $"{classification}|{groupName}";
                    classification = $"{classification}, {groupName}";
                    
                }

                if (!classifications.Any(c => c.id == classificationVal))
                {
                    classifications.Add(new ListItem<string>(classificationVal, classification));
                }
            }

            oRet.Classifications = classifications.OrderBy(c => c.name).ToList();

            if (!string.IsNullOrWhiteSpace(section))
            {
                if (!oRet.Classifications.Any(c => c.id == $"{section}|{group}"))
                {
                    return oRet;
                }

                List<VsandTeam> oTeams = null;
                if (!string.IsNullOrWhiteSpace(group))
                {
                    oTeams = await _uow.Teams.List(t => t.SportId == sportId &&
                        t.ScheduleYearId == scheduleYearId &&
                        t.CustomCodes.Any(tcc => tcc.CodeName == "Section" && tcc.CodeValue == section) &&
                        t.CustomCodes.Any(tcc => tcc.CodeName == "Group" && tcc.CodeValue == group), null,
                        new List<string> { "School", "CustomCodes" });
                } else
                {
                    oTeams = await _uow.Teams.List(t => t.SportId == sportId &&
                        t.ScheduleYearId == scheduleYearId &&
                        t.CustomCodes.Any(tcc => tcc.CodeName == "Section" && tcc.CodeValue == section), null,
                        new List<string> { "School", "CustomCodes" });
                }

                // Load the conference standing results
                oRet.Classification = $"{section}, {group}";
                oRet.Section = section;
                oRet.Group = group;
               
                oRet.Standings = (from t in oTeams select new TeamRecordInfo(t, true)).ToList();

                // for each team in the list, get their school's slug
                var schoolIds = (from t in oTeams where t.SchoolId.HasValue select t.SchoolId.Value).ToList();
                var schoolSlugs = await _uow.EntitySlugs.List(es => es.EntityType == "School" && schoolIds.Contains(es.EntityId));
                foreach (var slug in schoolSlugs)
                {
                    oRet.SchoolSlugs.Add(slug.EntityId, slug.Slug);
                }
            }

            return oRet;
        }

        /// <summary>
        /// Get the view model that shows for a sport and schedule year the conferences, 
        /// and if a conference is selected, the conference standings.
        /// Value is obtained from cache, or created and cached using GetSportsStandingsViewAsync.
        /// </summary>
        /// <param name="sportId">Required. The unique sport id.</param>
        /// <param name="scheduleYearId">Optional. The schedule year id. If not suppleid, the active schedule year will be used.</param>
        /// <param name="conference">Optional. If not supplied, the standings list is empty, and only the conference list is populated.</param>
        /// <returns></returns>
        public async Task<PowerpointsView> GetSportPowerpointsViewCachedAsync(int sportId, int? scheduleYearId, string section, string group)
        {
            var syid = 0;
            if (!scheduleYearId.HasValue)
            {
                syid = await _uow.ScheduleYears.GetActiveScheduleYearIdAsync();
            }
            else
            {
                syid = scheduleYearId.Value;
            }

            string cacheKey = Cache.Keys.FrontEndPowerpointsView(sportId, syid, section, group);

            PowerpointsView oRet = await _cache.GetAsync<PowerpointsView>(cacheKey);
            if (oRet != null)
            {
                return oRet;
            }

            oRet = await GetSportsPowerpointsViewAsync(sportId, syid, section, group);
            if (oRet != null)
            {
                await _cache.SetAsync(cacheKey, oRet);
            }

            return oRet;
        }


        #endregion

        #region Event Types
        public async Task<List<VsandSportEventType>> GetTopLevelEventTypesAsync(int sportId, int scheduleYearId)
        {
            return await _uow.SportEventTypes.List(et => et.SportId == sportId && et.ScheduleYearId == scheduleYearId && (et.ParentId == null || et.ParentId == 0));
        }

        public async Task<List<VsandSportEventType>> GetChildEventTypes(int parentEventTypeId)
        {
            return await _uow.SportEventTypes.List(et => et.ParentId == parentEventTypeId);
        }

        public async Task<VsandSportEventType> GetEventTypeAsync(int eventTypeId)
        {
            return await _uow.SportEventTypes.Single(et => et.EventTypeId == eventTypeId);
        }

        public async Task<ServiceResult<VsandSportEventType>> AddEventTypeAsync(VsandSportEventType addEventType)
        {
            var oRet = new ServiceResult<VsandSportEventType>();

            await _uow.SportEventTypes.Insert(addEventType);

            var bCreated = await _uow.Save();
            if (bCreated == false)
            {
                oRet.Success = false;
                oRet.Message = "An error occurred while trying to create the event type.";

                return oRet;
            }

            oRet.Success = true;
            oRet.obj = addEventType;
            oRet.Id = addEventType.EventTypeId;

            return oRet;
        }

        public async Task<ServiceResult<VsandSportEventType>> UpdateEventTypeAsync(VsandSportEventType chgEventType)
        {
            var oRet = new ServiceResult<VsandSportEventType>();

            _uow.SportEventTypes.Update(chgEventType);

            var bUpdated = await _uow.Save();
            if (bUpdated == false)
            {
                oRet.Success = false;
                oRet.Message = "An error ocurred while trying to update the event type.";

                return oRet;
            }

            oRet.Success = true;
            oRet.obj = chgEventType;
            oRet.Id = chgEventType.EventTypeId;

            return oRet;
        }

        public async Task<ServiceResult<VsandSportEventType>> DeleteEventTypeAsync(int eventTypeId)
        {
            var oRet = new ServiceResult<VsandSportEventType>();

            await _uow.SportEventTypes.Delete(eventTypeId);

            var bSaved = await _uow.Save();
            if (bSaved == false)
            {
                oRet.Success = false;
                oRet.Message = "An error occurred while trying to update the event type.";

                return oRet;
            }

            oRet.Success = true;
            oRet.Id = eventTypeId;

            return oRet;
        }

        public async Task<List<EventTypeListItem>> GetActiveEventTypeObjects(int sportId, int scheduleYearId)
        {
            return await _uow.ScheduleYearEventTypeListItems.GetActiveEventTypeObjectsAsync(sportId, scheduleYearId);
        }

        public async Task<List<ListItem<string>>> GetActiveEventTypes(int sportId, int scheduleYearId)
        {
            var etItems = await GetActiveEventTypeObjects(sportId, scheduleYearId);
            var oRet = (from et in etItems select new ListItem<string> { id = et.EventTypeListValue, name = et.EventTypeListName }).ToList();
            return oRet;
        }

        public async Task<List<EventTypeListItem>> GetAllEventTypeObjects(int sportId, int scheduleYearId)
        {
            return await _uow.ScheduleYearEventTypeListItems.GetEventTypeObjectsAsync(sportId, scheduleYearId);
        }

        public async Task<List<ListItem<string>>> GetAllEventTypes(int sportId, int scheduleYearId)
        {
            var etItems = await _uow.ScheduleYearEventTypeListItems.GetEventTypeObjectsAsync(sportId, scheduleYearId);
            var oRet = (from et in etItems select new ListItem<string> { id = et.EventTypeListValue, name = et.EventTypeListName }).ToList();
            return oRet;
        }
        #endregion

        #region Event Type Rounds
        public async Task<List<VsandSportEventTypeRound>> GetEventTypeRoundsAsync(int eventTypeId)
        {
            return await _uow.SportEventTypeRounds.List(r => r.EventTypeId == eventTypeId, x => x.OrderBy(r => r.SortOrder));
        }

        public async Task<ServiceResult<VsandSportEventTypeRound>> AddEventTypeRoundAsync(VsandSportEventTypeRound addRound)
        {
            var oRet = new ServiceResult<VsandSportEventTypeRound>();

            await _uow.SportEventTypeRounds.Insert(addRound);

            var bRet = await _uow.Save();

            if (bRet)
            {
                oRet.obj = addRound;
                oRet.Success = true;
                oRet.Id = addRound.RoundId;
            }

            return oRet;
        }

        public async Task<ServiceResult<VsandSportEventTypeRound>> UpdateEventTypeRoundAsync(VsandSportEventTypeRound chgRound)
        {
            var oRet = new ServiceResult<VsandSportEventTypeRound>();

            _uow.SportEventTypeRounds.Update(chgRound);

            var bRet = await _uow.Save();

            if (bRet)
            {
                oRet.obj = chgRound;
                oRet.Success = true;
                oRet.Id = chgRound.RoundId;
            }

            return oRet;
        }

        public async Task<ServiceResult<VsandSportEventTypeRound>> DeleteEventTypeRoundAsync(int roundCategoryId)
        {
            var oRet = new ServiceResult<VsandSportEventTypeRound>();

            VsandSportEventTypeRound remRound = await _uow.SportEventTypeRounds.GetById(roundCategoryId);
            await _uow.SportEventTypeRounds.Delete(remRound.RoundId);

            var bRet = await _uow.Save();

            if (bRet)
            {
                oRet.obj = remRound;
                oRet.Success = true;
                oRet.Id = remRound.RoundId;
            }

            return oRet;
        }

        public async Task<bool> UpdateEventTypeRoundOrder(int eventTypeId, List<VsandSportEventTypeRound> rounds)
        {
            bool bRet = await _uow.SportEventTypeRounds.UpdateOrder(rounds, r => r.EventTypeId == eventTypeId, r => r.RoundId, (r, order) => r.SortOrder = order);

            if (bRet == false)
            {
                return false;
            }

            return await _uow.Save();
        }
        #endregion

        #region Event Type Sections
        public async Task<List<VsandSportEventTypeSection>> GetEventTypeSectionsAsync(int eventTypeId)
        {
            var sections = await _uow.SportEventTypeSections.List(s => s.EventTypeId == eventTypeId, x => x.OrderBy(s => s.SortOrder), new List<string> { "Groups" });

            // preserve sort order
            foreach (var section in sections)
            {
                section.Groups = section.Groups.OrderBy(g => g.SortOrder).ToList();
            }

            return sections;
        }

        public async Task<ServiceResult<VsandSportEventTypeSection>> AddEventTypeSectionAsync(VsandSportEventTypeSection addSection)
        {
            var oRet = new ServiceResult<VsandSportEventTypeSection>();

            await _uow.SportEventTypeSections.Insert(addSection);

            var bRet = await _uow.Save();

            if (bRet)
            {
                oRet.obj = addSection;
                oRet.Success = true;
                oRet.Id = addSection.SectionId;
            }

            return oRet;
        }

        public async Task<ServiceResult<VsandSportEventTypeSection>> UpdateEventTypeSectionAsync(VsandSportEventTypeSection chgSection)
        {
            var oRet = new ServiceResult<VsandSportEventTypeSection>();

            _uow.SportEventTypeSections.Update(chgSection);

            var bRet = await _uow.Save();

            if (bRet)
            {
                oRet.obj = chgSection;
                oRet.Success = true;
                oRet.Id = chgSection.SectionId;
            }

            return oRet;
        }

        public async Task<ServiceResult<VsandSportEventTypeSection>> DeleteEventTypeSectionAsync(int sectionCategoryId)
        {
            var oRet = new ServiceResult<VsandSportEventTypeSection>();

            VsandSportEventTypeSection remSection = await _uow.SportEventTypeSections.GetById(sectionCategoryId);
            await _uow.SportEventTypeSections.Delete(remSection.SectionId);

            var bRet = await _uow.Save();

            if (bRet)
            {
                oRet.obj = remSection;
                oRet.Success = true;
                oRet.Id = remSection.SectionId;
            }

            return oRet;
        }

        public async Task<bool> UpdateEventTypeSectionOrder(int eventTypeId, List<VsandSportEventTypeSection> sections)
        {
            bool bRet = await _uow.SportEventTypeSections.UpdateOrder(sections, s => s.EventTypeId == eventTypeId, s => s.SectionId, (r, order) => r.SortOrder = order);
            if (bRet == false)
            {
                return false;
            }

            return await _uow.Save();
        }
        #endregion

        #region Event Type Groups
        public async Task<ServiceResult<VsandSportEventTypeGroup>> AddEventTypeGroupAsync(VsandSportEventTypeGroup addGroup)
        {
            var oRet = new ServiceResult<VsandSportEventTypeGroup>();

            await _uow.SportEventTypeGroups.Insert(addGroup);

            var bRet = await _uow.Save();

            if (bRet)
            {
                oRet.obj = addGroup;
                oRet.Success = true;
                oRet.Id = addGroup.GroupId;
            }

            return oRet;
        }

        public async Task<ServiceResult<VsandSportEventTypeGroup>> UpdateEventTypeGroupAsync(VsandSportEventTypeGroup chgGroup)
        {
            var oRet = new ServiceResult<VsandSportEventTypeGroup>();

            _uow.SportEventTypeGroups.Update(chgGroup);

            var bRet = await _uow.Save();

            if (bRet)
            {
                oRet.obj = chgGroup;
                oRet.Success = true;
                oRet.Id = chgGroup.GroupId;
            }

            return oRet;
        }

        public async Task<ServiceResult<VsandSportEventTypeGroup>> DeleteEventTypeGroupAsync(int groupCategoryId)
        {
            var oRet = new ServiceResult<VsandSportEventTypeGroup>();

            VsandSportEventTypeGroup remGroup = await _uow.SportEventTypeGroups.GetById(groupCategoryId);
            await _uow.SportEventTypeGroups.Delete(remGroup.GroupId);

            var bRet = await _uow.Save();

            if (bRet)
            {
                oRet.obj = remGroup;
                oRet.Success = true;
                oRet.Id = remGroup.GroupId;
            }

            return oRet;
        }

        public async Task<bool> UpdateEventTypeGroupOrder(int sectionId, List<VsandSportEventTypeGroup> groups)
        {
            bool bRet = await _uow.SportEventTypeGroups.UpdateOrder(groups, g => g.SectionId == sectionId, g => g.GroupId, (r, order) => r.SortOrder = order);
            if (bRet == false)
            {
                return false;
            }

            return await _uow.Save();
        }
        #endregion

        public async Task<IEnumerable<TeamStatistics>> GetTeamStatisticsAsync(int sportId)
        {
            var oRet = new List<TeamStatistics>();
            if (sportId <= 0) return oRet;

            var oTeamStatCategories = await _uow.SportTeamStatCategories.List(et => et.SportId == sportId);
            var oTeamStats = await _uow.SportTeamStats.List(et => et.SportId == sportId);

            foreach(var oCategories in oTeamStatCategories)
            {
                foreach(var oStats in oTeamStats)
                {
                    if(oCategories.SportTeamStatCategoryId == oStats.SportTeamStatCategoryId)
                    {
                        var oTeamStatistic = new TeamStatistics(oCategories, oStats);
                        oRet.Add(oTeamStatistic);
                    }
                }
            }

            return oRet;
        }
    }
}
