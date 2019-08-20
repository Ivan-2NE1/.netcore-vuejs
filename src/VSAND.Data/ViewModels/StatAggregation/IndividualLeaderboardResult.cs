using System;
using System.Collections.Generic;
using System.Text;
using VSAND.Data.Entities;

namespace VSAND.Data.ViewModels.StatAggregation
{
    public class IndividualLeaderboardResult
    {
        public VsandSport Sport { get; set; }
        public VsandScheduleYear ScheduleYear { get; set; }
        public VsandSportPlayerStatCategory Category { get; set; }
        public List<VsandSportPlayerStatCategory> Categories { get; set; }
        public List<VsandSportPlayerStat> Stats { get; set; }
        public List<AggStatPlayer> Players { get; set; }
        public int TotalResults { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int OrderById { get; set; }
        public string OrderDir { get; set; }

        public IndividualLeaderboardResult()
        {
            Players = new List<AggStatPlayer>();
        }
    }
}
