using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandPlannerDayBudget
    {
        public int BudgetId { get; set; }
        public DateTime PlannerDate { get; set; }
        public double? Columns { get; set; }
        public int? PublicationId { get; set; }

        public VsandPublication Publication { get; set; }
    }
}
