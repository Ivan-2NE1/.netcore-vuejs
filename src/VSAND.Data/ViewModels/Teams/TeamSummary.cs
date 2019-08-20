using System;
using System.Collections.Generic;
using System.Text;

namespace VSAND.Data.ViewModels.Teams
{
    public class TeamSummary
    {
        public int TeamId { get; set; }
        public int SchoolId { get; set; }
        public int SportId { get; set; }
        public int ScheduleYearId { get; set; }
        public string Name { get; set; }
        public string Sport { get; set; }
        public string ScheduleYear { get; set; }

        public TeamSummary()
        {

        }
    }
}
