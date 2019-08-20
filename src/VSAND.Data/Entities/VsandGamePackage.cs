using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandGamePackage
    {
        public VsandGamePackage()
        {
            VsandRoundupMember = new HashSet<VsandRoundupMember>();
        }

        public int GamePackageId { get; set; }
        public int PublicationStoryId { get; set; }
        public int StoryId { get; set; }
        public int GameReportId { get; set; }
        public int PublicationId { get; set; }
        public string GameReportData { get; set; }
        public string FormattedStory { get; set; }
        public string FileName { get; set; }
        public int CreatedById { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Status { get; set; }

        public AppxUser CreatedByNavigation { get; set; }
        public VsandGameReport GameReport { get; set; }
        public VsandPublication Publication { get; set; }
        public VsandStory Story { get; set; }
        public ICollection<VsandRoundupMember> VsandRoundupMember { get; set; }
    }
}
