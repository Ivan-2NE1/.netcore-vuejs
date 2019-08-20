using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandTeamRosterCustomCode
    {
        public int CustomCodeId { get; set; }
        public string CodeName { get; set; }
        public string CodeValue { get; set; }
        public int RosterId { get; set; }
        public int? SportId { get; set; }
        public int? ScheduleYearId { get; set; }

        public VsandTeamRoster Roster { get; set; }
    }
}
