using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandPlannerNote
    {
        public int NoteId { get; set; }
        public DateTime PlannerDate { get; set; }
        public int CategoryId { get; set; }
        public int? SportId { get; set; }
        public int AddedBy { get; set; }
        public DateTime AddedDate { get; set; }
        public string Note { get; set; }
        public int? PublicationId { get; set; }

        public VsandPlannerCategory Category { get; set; }
        public VsandPublication Publication { get; set; }
        public VsandSport Sport { get; set; }
    }
}
