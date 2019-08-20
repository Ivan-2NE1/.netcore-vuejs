using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandTeamScheduleTeam
    {
        public int TsteamId { get; set; }
        public int ScheduleId { get; set; }
        public int TeamId { get; set; }
        public string Name { get; set; }
        public bool HomeTeam { get; set; }
        public int SortOrder { get; set; }

        public VsandTeamSchedule Schedule { get; set; }
        public VsandTeam Team { get; set; }
    }
}
