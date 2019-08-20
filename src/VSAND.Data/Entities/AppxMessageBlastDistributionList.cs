using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class AppxMessageBlastDistributionList
    {
        public int DistributionListId { get; set; }
        public string Name { get; set; }
        public int ListSourceId { get; set; }
        public string Filter { get; set; }
    }
}
