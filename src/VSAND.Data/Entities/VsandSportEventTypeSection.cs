using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandSportEventTypeSection
    {
        public VsandSportEventTypeSection()
        {
            GameReports = new HashSet<VsandGameReport>();
            Groups = new HashSet<VsandSportEventTypeGroup>();
        }

        public int SectionId { get; set; }
        public int EventTypeId { get; set; }
        public string Name { get; set; }
        public int? SortOrder { get; set; }

        public VsandSportEventType EventType { get; set; }
        public ICollection<VsandGameReport> GameReports { get; set; }
        public ICollection<VsandSportEventTypeGroup> Groups { get; set; }
    }
}
