using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandSportEventTypeRound
    {
        public VsandSportEventTypeRound()
        {
            VsandGameReport = new HashSet<VsandGameReport>();
        }

        public int RoundId { get; set; }
        public int EventTypeId { get; set; }
        public string Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? SortOrder { get; set; }
        public int? CustomFormId { get; set; }
        public bool? ActAsHeading { get; set; }
        public string ParticipatingTeamsFilter { get; set; }

        public VsandSportEventType SportEventType { get; set; }
        public ICollection<VsandGameReport> VsandGameReport { get; set; }
    }
}
