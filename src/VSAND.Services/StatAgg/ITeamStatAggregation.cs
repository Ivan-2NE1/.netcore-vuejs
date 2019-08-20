using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VSAND.Data.ViewModels.StatAggregation;

namespace VSAND.Services.StatAgg
{
    public interface ITeamStatAggregation
    {
        Task<TeamLeaderboardResult> TeamLeaderBoard(int sportId, int scheduleYearId, int statCategoryId, int gamesPlayed, int orderBy, string orderDir, int pageNumber, int pageSize);
    }
}
