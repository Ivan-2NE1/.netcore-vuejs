namespace VSAND.Data.Entities
{
    public partial class VsandPublicationStoryPlayByPlay
    {
        public int PubStoryPlayByPlayId { get; set; }
        public int PublicationStoryId { get; set; }
        public int PlayByPlayId { get; set; }
        public int SortOrder { get; set; }
        public string FormattedText { get; set; }

        public VsandGameReportPlayByPlay GameReportPlayByPlay { get; set; }
        public VsandPublicationStory PublicationStory { get; set; }
    }
}
