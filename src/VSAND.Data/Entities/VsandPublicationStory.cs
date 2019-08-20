using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandPublicationStory
    {
        public VsandPublicationStory()
        {
            VsandPublicationStoryNote = new HashSet<VsandPublicationStoryNote>();
            VsandPublicationStoryPlayByPlay = new HashSet<VsandPublicationStoryPlayByPlay>();
            VsandStory = new HashSet<VsandStory>();
        }

        public int PublicationStoryId { get; set; }
        public int PublicationId { get; set; }
        public int GameReportId { get; set; }
        public int? CreatedById { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? AssignToId { get; set; }
        public string AssignToName { get; set; }
        public DateTime AssignToDate { get; set; }
        public string Status { get; set; }
        public string SubStatus { get; set; }
        public int? StatusById { get; set; }
        public string StatusBy { get; set; }
        public DateTime? StatusDate { get; set; }

        public AppxUser AssignedToUser { get; set; }
        public AppxUser CreatedBy { get; set; }
        public VsandGameReport GameReport { get; set; }
        public VsandPublication Publication { get; set; }
        public AppxUser StatusByNavigation { get; set; }
        public ICollection<VsandPublicationStoryNote> VsandPublicationStoryNote { get; set; }
        public ICollection<VsandPublicationStoryPlayByPlay> VsandPublicationStoryPlayByPlay { get; set; }
        public ICollection<VsandStory> VsandStory { get; set; }
    }
}
