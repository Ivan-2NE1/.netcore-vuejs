using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandGameReportTeam
    {
        public VsandGameReportTeam()
        {
            EventPlayers = new HashSet<VsandGameReportEventPlayer>();
            EventPlayerGroups = new HashSet<VsandGameReportEventPlayerGroup>();
            VsandGameReportPairingTeam = new HashSet<VsandGameReportPairingTeam>();
            PeriodScores = new HashSet<VsandGameReportPeriodScore>();
            GameRoster = new HashSet<VsandGameReportRoster>();
            TeamStats = new HashSet<VsandGameReportTeamStat>();
        }

        public int GameReportTeamId { get; set; }
        public int GameReportId { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public string Abbreviation { get; set; }
        public bool HomeTeam { get; set; }
        public int? TeamWins { get; set; }
        public int? TeamLosses { get; set; }
        public int? TeamTies { get; set; }
        public double FinalScore { get; set; }
        public bool WinningTeam { get; set; }
        public bool Forfeit { get; set; }

        public VsandGameReport GameReport { get; set; }
        public VsandTeam Team { get; set; }
        public ICollection<VsandGameReportEventPlayer> EventPlayers { get; set; }
        public ICollection<VsandGameReportEventPlayerGroup> EventPlayerGroups { get; set; }
        public ICollection<VsandGameReportPairingTeam> VsandGameReportPairingTeam { get; set; }
        public ICollection<VsandGameReportPeriodScore> PeriodScores { get; set; }
        public ICollection<VsandGameReportRoster> GameRoster { get; set; }
        public ICollection<VsandGameReportTeamStat> TeamStats { get; set; }
    }
}
