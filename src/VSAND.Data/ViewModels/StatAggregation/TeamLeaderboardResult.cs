using System;
using System.Collections.Generic;
using System.Text;
using VSAND.Data.Entities;

namespace VSAND.Data.ViewModels.StatAggregation
{
    public class TeamLeaderboardResult
    {
        public VsandSport Sport { get; set; }
        public VsandScheduleYear ScheduleYear { get; set; }
        public VsandSportTeamStatCategory Category { get; set; }
        public List<VsandSportTeamStatCategory> Categories { get; set; }
        public List<VsandSportTeamStat> Stats { get; set; }
        public List<AggStatTeam> Teams { get; set; }
        public int TotalResults { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int OrderById { get; set; }
        public string OrderDir { get; set; }

        public TeamLeaderboardResult()
        {
            Teams = new List<AggStatTeam>();
        }
    }
}
