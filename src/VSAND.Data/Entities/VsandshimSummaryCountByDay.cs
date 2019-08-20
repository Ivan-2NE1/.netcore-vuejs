using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandshimSummaryCountByDay
    {
        public DateTime SummaryDate { get; set; }
        public int SummaryCount { get; set; }
    }
}
