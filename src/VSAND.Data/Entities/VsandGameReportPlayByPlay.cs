using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandGameReportPlayByPlay
    {
        public VsandGameReportPlayByPlay()
        {
            VsandPublicationStoryPlayByPlay = new HashSet<VsandPublicationStoryPlayByPlay>();
        }

        public int PlayByPlayId { get; set; }
        public int GameReportId { get; set; }
        public int? PeriodNumber { get; set; }
        public int? SortOrder { get; set; }
        public string ObjectData { get; set; }
        public string FormattedText { get; set; }

        public VsandGameReport GameReport { get; set; }
        public ICollection<VsandPublicationStoryPlayByPlay> VsandPublicationStoryPlayByPlay { get; set; }
    }
}
