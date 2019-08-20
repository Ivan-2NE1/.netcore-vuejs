using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandGameReportEventPlayer
    {
        public VsandGameReportEventPlayer()
        {
            GameReportEventPlayerStats = new HashSet<VsandGameReportEventPlayerStat>();
        }

        public int EventPlayerId { get; set; }
        public int GameReportTeamId { get; set; }
        public int EventResultId { get; set; }
        public int? PlayerId { get; set; }
        public bool? Winner { get; set; }
        public double? Score { get; set; }

        public VsandGameReportEventResult EventResult { get; set; }
        public VsandGameReportTeam GameReportTeam { get; set; }
        public VsandPlayer Player { get; set; }
        public ICollection<VsandGameReportEventPlayerStat> GameReportEventPlayerStats { get; set; }
    }
}
