using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandSportEventTypeGroup
    {
        public VsandSportEventTypeGroup()
        {
            VsandGameReport = new HashSet<VsandGameReport>();
        }

        public int GroupId { get; set; }
        public int SectionId { get; set; }
        public string Name { get; set; }
        public int? SortOrder { get; set; }

        public VsandSportEventTypeSection Section { get; set; }
        public ICollection<VsandGameReport> VsandGameReport { get; set; }
    }
}
