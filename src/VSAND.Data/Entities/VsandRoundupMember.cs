using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandRoundupMember
    {
        public int RoundupMemberId { get; set; }
        public int RoundupId { get; set; }
        public int GamePackageId { get; set; }
        public int SortOrder { get; set; }
        public bool? IsLead { get; set; }
        public bool? UseStory { get; set; }

        public VsandGamePackage GamePackage { get; set; }
        public VsandRoundup Roundup { get; set; }
    }
}
