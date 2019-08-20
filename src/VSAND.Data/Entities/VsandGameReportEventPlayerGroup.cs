using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandGameReportEventPlayerGroup
    {
        public VsandGameReportEventPlayerGroup()
        {
            EventPlayerGroupPlayers = new HashSet<VsandGameReportEventPlayerGroupPlayer>();
            GameReportEventPlayerGroupStats = new HashSet<VsandGameReportEventPlayerGroupStat>();
        }

        public int PlayerGroupId { get; set; }
        public int GameReportTeamId { get; set; }
        public int EventResultId { get; set; }
        public bool? Winner { get; set; }

        public VsandGameReportEventResult EventResult { get; set; }
        public VsandGameReportTeam GameReportTeam { get; set; }
        public ICollection<VsandGameReportEventPlayerGroupPlayer> EventPlayerGroupPlayers { get; set; }
        public ICollection<VsandGameReportEventPlayerGroupStat> GameReportEventPlayerGroupStats { get; set; }
    }
}
