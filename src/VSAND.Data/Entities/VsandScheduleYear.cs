using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandScheduleYear
    {
        public VsandScheduleYear()
        {
            VsandBook = new HashSet<VsandBook>();
            VsandBookNote = new HashSet<VsandBookNote>();
            VsandGameReport = new HashSet<VsandGameReport>();
            VsandScheduleLoadFile = new HashSet<VsandScheduleLoadFile>();
            VsandSportEventType = new HashSet<VsandSportEventType>();
            VsandSportSeason = new HashSet<VsandSportSeason>();
            VsandTeam = new HashSet<VsandTeam>();
            PowerPointsConfigs = new HashSet<VsandPowerPointsConfig>();
            LeagueRules = new HashSet<VsandLeagueRule>();
        }

        public int ScheduleYearId { get; set; }
        public string Name { get; set; }
        public int? EndYear { get; set; }
        public bool? Active { get; set; }

        public ICollection<VsandBook> VsandBook { get; set; }
        public ICollection<VsandBookNote> VsandBookNote { get; set; }
        public ICollection<VsandGameReport> VsandGameReport { get; set; }
        public ICollection<VsandScheduleLoadFile> VsandScheduleLoadFile { get; set; }
        public ICollection<VsandSportEventType> VsandSportEventType { get; set; }
        public ICollection<VsandSportSeason> VsandSportSeason { get; set; }
        public ICollection<VsandTeam> VsandTeam { get; set; }
        public ICollection<VsandPowerPointsConfig> PowerPointsConfigs { get; set; }
        public ICollection<VsandLeagueRule> LeagueRules { get; set; }
    }
}
