namespace VSAND.Data.Entities
{
    public partial class VsandGameReportRoster
    {
        public int GameReportRosterId { get; set; }
        public int GameReportTeamId { get; set; }
        public int PlayerId { get; set; }
        public int? PositionId { get; set; }
        public bool Starter { get; set; }
        public int? RosterOrder { get; set; }
        public bool? PlayerOfRecord { get; set; }
        public int? RecordWins { get; set; }
        public int? RecordLosses { get; set; }
        public int? RecordTies { get; set; }

        public VsandGameReportTeam GameReportTeam { get; set; }
        public VsandPlayer Player { get; set; }
        public VsandSportPosition Position { get; set; }
    }
}
