namespace VSAND.Data.Entities
{
    public partial class VsandGameReportMeta
    {
        public int GameReportMetaId { get; set; }
        public int GameReportId { get; set; }
        public int SportGameMetaId { get; set; }
        public string MetaValue { get; set; }

        public VsandGameReport GameReport { get; set; }
        public VsandSportGameMeta SportGameMeta { get; set; }
    }
}
