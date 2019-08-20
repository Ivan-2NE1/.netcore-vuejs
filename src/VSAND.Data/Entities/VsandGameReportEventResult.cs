using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandGameReportEventResult
    {
        public VsandGameReportEventResult()
        {
            EventPlayers = new HashSet<VsandGameReportEventPlayer>();
            EventPlayerGroups = new HashSet<VsandGameReportEventPlayerGroup>();
        }

        public int EventResultId { get; set; }
        public int EventId { get; set; }
        public int? SortOrder { get; set; }
        public string ResultType { get; set; }
        public string Duration { get; set; }
        public string Overtime { get; set; }

        public VsandGameReportEvent GameReportEvent { get; set; }
        public ICollection<VsandGameReportEventPlayer> EventPlayers { get; set; }
        public ICollection<VsandGameReportEventPlayerGroup> EventPlayerGroups { get; set; }
    }
}
