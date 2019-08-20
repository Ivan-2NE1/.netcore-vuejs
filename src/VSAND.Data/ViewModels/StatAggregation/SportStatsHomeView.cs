using System;
using System.Collections.Generic;
using System.Text;
using VSAND.Data.Entities;

namespace VSAND.Data.ViewModels.StatAggregation
{
    public class SportStatsHomeView
    {
        public List<IndividualLeaderboardResult> Featuredstats { get; set; }
        public List<VsandSportPlayerStatCategory> PlayerStatCategories { get; set; }
        public List<VsandSportTeamStatCategory> TeamStatCategories { get; set; }
        public VsandSport Sport { get; set; }
        public SportStatsHomeView()
        {
            Featuredstats = new List<IndividualLeaderboardResult>();
        }
    }
}
