using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandNewsPackage
    {
        public int NewsPackageId { get; set; }
        public int NewsId { get; set; }
        public int NewsStoryId { get; set; }
        public string FormattedStory { get; set; }
        public string FileName { get; set; }
        public int CreatedById { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Status { get; set; }

        public AppxUser CreatedByNavigation { get; set; }
        public VsandNews News { get; set; }
        public VsandNewsStory NewsStory { get; set; }
    }
}
