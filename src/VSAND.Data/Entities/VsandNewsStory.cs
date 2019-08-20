using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandNewsStory
    {
        public VsandNewsStory()
        {
            VsandNewsPackage = new HashSet<VsandNewsPackage>();
        }

        public int StoryId { get; set; }
        public int NewsId { get; set; }
        public int RevisionId { get; set; }
        public int AssignToId { get; set; }
        public string AssignToName { get; set; }
        public string Status { get; set; }
        public string SubStatus { get; set; }
        public int StatusById { get; set; }
        public string StatusByName { get; set; }
        public DateTime? StatusDate { get; set; }
        public string Story { get; set; }
        public string ObjectData { get; set; }

        public AppxUser AssignTo { get; set; }
        public VsandNews News { get; set; }
        public AppxUser StatusBy { get; set; }
        public ICollection<VsandNewsPackage> VsandNewsPackage { get; set; }
    }
}
