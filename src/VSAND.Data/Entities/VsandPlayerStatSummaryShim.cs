using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandPlayerStatSummaryShim
    {
        public int PlayerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Team { get; set; }
        public double? Total { get; set; }
        public double? Average { get; set; }
        public double? Minimum { get; set; }
        public double? Maximum { get; set; }
        public int? GameCount { get; set; }
    }
}
