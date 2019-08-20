using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace VSAND.Data.ViewModels
{
    public class ScheduleYearProvisioningSummary
    {        
        public int SchoolId { get; set; }
        public bool CoreCoverage { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public bool Validated { get; set; }
        [DisplayName("Previous Season Game Count")]
        public int PreviousSeasonGameCount { get; set; }
        [DisplayName("Previous Season Roster Count")]
        public int PreviousSeasonRosterCount { get; set; }
        [DisplayName("Current Season Team Id")]
        public int CurrentSeasonTeamId { get; set; }
        [DisplayName("Current Season Game Count")]
        public int CurrentSeasonGameCount { get; set; }
        [DisplayName("Current Season Roster Count")]
        public int CurrentSeasonRosterCount { get; set; }
    }
}
