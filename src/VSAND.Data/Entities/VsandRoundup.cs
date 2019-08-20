using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandRoundup
    {
        public VsandRoundup()
        {
            VsandRoundupLeadStory = new HashSet<VsandRoundupLeadStory>();
            VsandRoundupMember = new HashSet<VsandRoundupMember>();
        }

        public int RoundupId { get; set; }
        public int? EditionId { get; set; }
        public string Title1 { get; set; }
        public string Title2 { get; set; }
        public string Title3 { get; set; }
        public int SportId { get; set; }
        public int? RoundupFormat { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool Archived { get; set; }
        public int? LeadGameReportId { get; set; }
        public string LeadStory { get; set; }

        public VsandEdition Edition { get; set; }
        public VsandRoundupType RoundupFormatNavigation { get; set; }
        public VsandSport Sport { get; set; }
        public ICollection<VsandRoundupLeadStory> VsandRoundupLeadStory { get; set; }
        public ICollection<VsandRoundupMember> VsandRoundupMember { get; set; }
    }
}
