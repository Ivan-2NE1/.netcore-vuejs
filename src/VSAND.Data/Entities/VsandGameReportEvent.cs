using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandGameReportEvent
    {
        public VsandGameReportEvent()
        {
            Results = new HashSet<VsandGameReportEventResult>();
        }

        public int EventId { get; set; }
        public int GameReportId { get; set; }
        public int SportEventId { get; set; }
        public int? SortOrder { get; set; }
        public int? RoundNumber { get; set; }
        public string RoundName { get; set; }

        public VsandGameReport GameReport { get; set; }
        public VsandSportEvent SportEvent { get; set; }
        public ICollection<VsandGameReportEventResult> Results { get; set; }
    }
}
