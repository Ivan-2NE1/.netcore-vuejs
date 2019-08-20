using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandPlannerCategory
    {
        public VsandPlannerCategory()
        {
            VsandPlannerLayout = new HashSet<VsandPlannerLayout>();
            VsandPlannerNote = new HashSet<VsandPlannerNote>();
        }

        public int CategoryId { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }
        public bool Enabled { get; set; }
        public int? PublicationId { get; set; }
        public string TitleBgColor { get; set; }
        public string TitleColor { get; set; }
        public string BodyBgColor { get; set; }
        public string BodyColor { get; set; }

        public VsandPublication Publication { get; set; }
        public ICollection<VsandPlannerLayout> VsandPlannerLayout { get; set; }
        public ICollection<VsandPlannerNote> VsandPlannerNote { get; set; }
    }
}
