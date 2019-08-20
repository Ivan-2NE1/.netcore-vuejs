using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandNews
    {
        public VsandNews()
        {
            VsandNewsPackage = new HashSet<VsandNewsPackage>();
            VsandNewsStory = new HashSet<VsandNewsStory>();
        }

        public int NewsId { get; set; }
        public string Slug { get; set; }
        public string Title { get; set; }
        public string ByLine { get; set; }
        public int? SportId { get; set; }
        public int? NewsTypeId { get; set; }
        public int? PublicationId { get; set; }
        public DateTime? PubDate { get; set; }
        public DateTime? OnlineDate { get; set; }
        public bool? SendOnline { get; set; }
        public int CreatedById { get; set; }
        public string CreatedByName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int AssignToId { get; set; }
        public string AssignToName { get; set; }
        public DateTime? AssignToDate { get; set; }
        public string Status { get; set; }
        public string SubStatus { get; set; }
        public int StatusById { get; set; }
        public string StatusByName { get; set; }
        public DateTime? StatusDate { get; set; }
        public bool? Archived { get; set; }

        public AppxUser AssignTo { get; set; }
        public AppxUser CreatedBy { get; set; }
        public VsandNewsType NewsType { get; set; }
        public VsandPublication Publication { get; set; }
        public VsandSport Sport { get; set; }
        public AppxUser StatusBy { get; set; }
        public ICollection<VsandNewsPackage> VsandNewsPackage { get; set; }
        public ICollection<VsandNewsStory> VsandNewsStory { get; set; }
    }
}
