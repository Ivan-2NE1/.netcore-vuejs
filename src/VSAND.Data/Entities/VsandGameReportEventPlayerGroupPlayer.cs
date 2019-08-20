namespace VSAND.Data.Entities
{
    public partial class VsandGameReportEventPlayerGroupPlayer
    {
        public int PlayerGroupPlayerId { get; set; }
        public int PlayerGroupId { get; set; }
        public int? PlayerId { get; set; }
        public int? SortOrder { get; set; }

        public VsandPlayer Player { get; set; }
        public VsandGameReportEventPlayerGroup PlayerGroup { get; set; }
    }
}
