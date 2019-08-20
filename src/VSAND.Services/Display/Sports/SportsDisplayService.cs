using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSAND.Data.Repositories;
using VSAND.Data.ViewModels.StatAggregation;
using VSAND.Services.Cache;
using VSAND.Services.Data.Sports;
using VSAND.Services.StatAgg;

namespace VSAND.Services.Display.Sports
{
    public class SportsDisplayService : ISportsDisplayService
    {
        private readonly ICache _cache;
        private readonly IUnitOfWork _uow;
        private readonly IPlayerStatAggregation _stagAgg;
        private readonly ISportService _sports;

        public SportsDisplayService(ICache cache, IUnitOfWork uow, IPlayerStatAggregation statAgg, ISportService sports)
        {
            _cache = cache;
            _uow = uow;
            _stagAgg = statAgg;
            _sports = sports;
        }

        public async Task<SportStatsHomeView> GetSportStatsHomeView(int sportId)
        {
            var cacheKey = Cache.Keys.FrontEndDisplaySportStatsHomeView(sportId);

            var oView = await _cache.GetAsync<SportStatsHomeView>(cacheKey);
            if (oView != null)
            {
                return oView;
            }

            // Build & cache our view
            oView = new SportStatsHomeView();
            
            var scheduleYear = await _uow.ScheduleYears.Single(sy => sy.Active.HasValue && sy.Active.Value);
            int scheduleYearId = 0;
            if (scheduleYear != null)
            {
                // big problem?
                scheduleYearId = scheduleYear.ScheduleYearId;
            }

            oView.Sport = await _sports.GetSportAsync(sportId);

            List<int> featuredStatIds = new List<int>();
            // Check to see if there is a list of featured stats for this sport
            var featuredStatIdConfig = await _uow.AppxConfigs.Single(c => c.ConfigCat == "FrontEnd" && c.ConfigName == $"Sport{sportId}FeaturedPlayerStats");
            if (featuredStatIdConfig != null)
            {
                string featuredIds = featuredStatIdConfig.ConfigVal.Trim();
                featuredStatIds = (from s in featuredIds.Split(',').ToList() select int.Parse(s)).ToList();
            }
            foreach (int statId in featuredStatIds)
            {
                var featureStat = await _stagAgg.IndividualLeaderBoardForStat(sportId, scheduleYearId, statId, "DESC", 1, 5);
                if (featureStat != null && featureStat.Players.Any())
                {
                    oView.Featuredstats.Add(featureStat);
                }                
            }

            oView.PlayerStatCategories = await _uow.SportPlayerStatCategories.List(spsc => spsc.SportId == sportId && spsc.PlayerStats.Any(ps => ps.Enabled), spsc => spsc.OrderBy(c => c.SortOrder));
            oView.TeamStatCategories = await _uow.SportTeamStatCategories.List(tsc => tsc.SportId == sportId && tsc.TeamStats.Any(ts => ts.Enabled), tsc => tsc.OrderBy(c => c.SortOrder));

            await _cache.SetAsync(cacheKey, oView);

            return oView;
        }
    }
}
