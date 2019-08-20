using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandSystemMessage
    {
        public VsandSystemMessage()
        {
            VsandSystemMessageSport = new HashSet<VsandSystemMessageSport>();
        }

        public int MessageId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string CreatedBy { get; set; }
        public int CreatedById { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public int ModifiedById { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime? StartDisplayDate { get; set; }
        public DateTime? EndDisplayDate { get; set; }
        public int? ScheduleYearId { get; set; }
        public int? DisplayMethod { get; set; }
        public string DisplayArea { get; set; }

        public ICollection<VsandSystemMessageSport> VsandSystemMessageSport { get; set; }
    }
}
