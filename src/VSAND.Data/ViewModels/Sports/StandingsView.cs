using System;
using System.Collections.Generic;
using System.Text;
using VSAND.Data.Entities;

namespace VSAND.Data.ViewModels.Sports
{
    public class StandingsView
    {
        public VsandSport Sport { get; set; }
        public VsandScheduleYear ScheduleYear { get; set; }
        public string SportSlug { get; set; }
        public string ScheduleYearSlug { get; set; }
        public List<string> Conferences { get; set; }
        public string Conference { get; set; }
        public List<TeamRecordInfo> Standings { get; set; }
        public SortedList<int, string> SchoolSlugs { get; set; }
        public StandingsView()
        {
            SchoolSlugs = new SortedList<int, string>();
        }

    }
}
