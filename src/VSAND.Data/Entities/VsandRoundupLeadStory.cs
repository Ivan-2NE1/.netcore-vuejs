using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandRoundupLeadStory
    {
        public int LeadStoryId { get; set; }
        public int RoundupId { get; set; }
        public int RevisionNumber { get; set; }
        public string ByLine { get; set; }
        public string Article { get; set; }
        public int CreatedById { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string SourceLine { get; set; }

        public AppxUser CreatedByNavigation { get; set; }
        public VsandRoundup Roundup { get; set; }
    }
}
