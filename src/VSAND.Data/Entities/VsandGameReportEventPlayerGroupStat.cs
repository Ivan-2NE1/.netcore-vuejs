namespace VSAND.Data.Entities
{
    public partial class VsandGameReportEventPlayerGroupStat
    {
        public int PlayerGroupStatId { get; set; }
        public int PlayerGroupId { get; set; }
        public int StatId { get; set; }
        public double? StatValue { get; set; }

        public VsandGameReportEventPlayerGroup EventPlayerGroup { get; set; }
        public VsandSportEventStat SportEventStat { get; set; }
    }
}
