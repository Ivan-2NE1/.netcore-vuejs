using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandStatQuery
    {
        public int StatQueryId { get; set; }
        public string Name { get; set; }
        public int SportId { get; set; }
        public string OutputTitle { get; set; }
        public int CreatedById { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedById { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string Handler { get; set; }
        public string QueryData { get; set; }
    }
}
