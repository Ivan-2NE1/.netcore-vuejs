using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandPlannerLayout
    {
        public int LayoutId { get; set; }
        public int Month { get; set; }
        public int WeekDay { get; set; }
        public int CategoryId { get; set; }
        public int SortOrder { get; set; }
        public bool Deleted { get; set; }

        public VsandPlannerCategory Category { get; set; }
    }
}
