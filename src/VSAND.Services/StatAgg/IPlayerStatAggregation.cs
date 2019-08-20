using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VSAND.Data.ViewModels.StatAggregation;

namespace VSAND.Services.StatAgg
{
    public interface IPlayerStatAggregation
    {
        Task<IndividualLeaderboardResult> IndividualLeaderBoard(int sportId, int scheduleYearId, int statCategoryId, int gamesPlayed, int orderBy, string orderDir, int pageNumber, int pageSize);
        Task<IndividualLeaderboardResult> IndividualLeaderBoardForStat(int sportId, int scheduleYearId, int orderBy, string orderDir, int pageNumber, int pageSize);
        Task<List<AggregatedStatItem>> TeamRosterEntryTop100(int playerId, int teamId, int sportId);
        Task<List<AggregatedStatItem>> TeamRosterentrySpecificStatValues(int playerId, int teamId, List<int> statIds);
    }
}
