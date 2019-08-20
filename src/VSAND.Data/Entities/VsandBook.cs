using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandBook
    {
        public VsandBook()
        {
            VsandBookMember = new HashSet<VsandBookMember>();
            VsandBookNote = new HashSet<VsandBookNote>();
            VsandBookSubscription = new HashSet<VsandBookSubscription>();
        }

        public int BookId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int SportId { get; set; }
        public int ScheduleYearId { get; set; }
        public int CreatedById { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Deleted { get; set; }
        public int? PreviousBookId { get; set; }

        public AppxUser CreatedByNavigation { get; set; }
        public VsandScheduleYear ScheduleYear { get; set; }
        public VsandSport Sport { get; set; }
        public ICollection<VsandBookMember> VsandBookMember { get; set; }
        public ICollection<VsandBookNote> VsandBookNote { get; set; }
        public ICollection<VsandBookSubscription> VsandBookSubscription { get; set; }
    }
}
