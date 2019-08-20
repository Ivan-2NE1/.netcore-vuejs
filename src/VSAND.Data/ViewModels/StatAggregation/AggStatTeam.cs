using System;
using System.Collections.Generic;
using System.Text;
using VSAND.Data.ViewModels.Teams;

namespace VSAND.Data.ViewModels.StatAggregation
{
    public class AggStatTeam
    {
        public int TeamId { get; set; }
        public int SchoolId { get; set; }
        public int SportId { get; set; }
        public int ScheduleYearId { get; set; }

        public string Name { get; set; }
        public string SchoolSlug { get; set; }
        public string SportSlug { get; set; }
        public string ScheduleYearSlug { get; set; }
        public TeamRecord Record { get; set; }

        public string City { get; set; }
        public string State { get; set; }
        
        public int GamesPlayed { get; set; }
        public int Rank { get; set; }
        public Dictionary<int, double> StatValues { get; set; }

        public AggStatTeam()
        {
            StatValues = new Dictionary<int, double>();
        }
    }
}
