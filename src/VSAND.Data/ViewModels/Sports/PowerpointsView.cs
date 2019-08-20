using System;
using System.Collections.Generic;
using System.Text;
using VSAND.Data.Entities;

namespace VSAND.Data.ViewModels.Sports
{
    public class PowerpointsView
    {
        public VsandSport Sport { get; set; }
        public VsandScheduleYear ScheduleYear { get; set; }
        public string SportSlug { get; set; }
        public string ScheduleYearSlug { get; set; }
        public List<ListItem<string>> Classifications { get; set; }
        public string Classification { get; set; }
        public string Section { get; set; }
        public string Group { get; set; }
        public List<TeamRecordInfo> Standings { get; set; }
        public SortedList<int, string> SchoolSlugs { get; set; }
        public PowerpointsView()
        {
            SchoolSlugs = new SortedList<int, string>();
        }
    }
}
