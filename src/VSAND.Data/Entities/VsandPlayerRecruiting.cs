using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandPlayerRecruiting
    {
        public int RecruitingId { get; set; }
        public int PlayerId { get; set; }
        public string State { get; set; }
        public int UniversityId { get; set; }
        public string UniversityName { get; set; }
        public int InterestLevel { get; set; }
        public int SportId { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedById { get; set; }
        public string CreatedBy { get; set; }

        public VsandPlayer Player { get; set; }
        public VsandSport Sport { get; set; }
    }
}
