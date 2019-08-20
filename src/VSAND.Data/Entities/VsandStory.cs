using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandStory
    {
        public VsandStory()
        {
            VsandGamePackage = new HashSet<VsandGamePackage>();
        }

        public int StoryId { get; set; }
        public int PublicationStoryId { get; set; }
        public int RevisionId { get; set; }
        public string Title { get; set; }
        public string ByLine { get; set; }
        public string Story { get; set; }
        public int AssignToId { get; set; }
        public string AssignToName { get; set; }
        public string Status { get; set; }
        public string SubStatus { get; set; }
        public int StatusById { get; set; }
        public string StatusBy { get; set; }
        public DateTime? StatusDate { get; set; }
        public string SourceLine { get; set; }

        public AppxUser AssignTo { get; set; }
        public VsandPublicationStory PublicationStory { get; set; }
        public AppxUser StatusByNavigation { get; set; }
        public ICollection<VsandGamePackage> VsandGamePackage { get; set; }
    }
}
