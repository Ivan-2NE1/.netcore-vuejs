using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandSportEventStat
    {
        public VsandSportEventStat()
        {
            EventPlayerGroupStats = new HashSet<VsandGameReportEventPlayerGroupStat>();
            EventPlayerStats = new HashSet<VsandGameReportEventPlayerStat>();
        }

        public int SportEventStatId { get; set; }
        public int SportEventId { get; set; }
        public int SportId { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public string DataType { get; set; }
        public int SortOrder { get; set; }
        public bool Enabled { get; set; }
        public bool? Calculated { get; set; }

        public VsandSport Sport { get; set; }
        public VsandSportEvent SportEvent { get; set; }
        public ICollection<VsandGameReportEventPlayerGroupStat> EventPlayerGroupStats { get; set; }
        public ICollection<VsandGameReportEventPlayerStat> EventPlayerStats { get; set; }
    }
}
