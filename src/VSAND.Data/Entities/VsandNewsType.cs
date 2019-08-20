using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandNewsType
    {
        public VsandNewsType()
        {
            VsandNews = new HashSet<VsandNews>();
        }

        public int NewsTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string TypeName { get; set; }
        public string ControlPath { get; set; }
        public string LegacyFormatter { get; set; }
        public string AtomFormatter { get; set; }

        public ICollection<VsandNews> VsandNews { get; set; }
    }
}
