using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandRoundupType
    {
        public VsandRoundupType()
        {
            VsandRoundup = new HashSet<VsandRoundup>();
        }

        public int RoundupTypeId { get; set; }
        public string Name { get; set; }
        public int? SportId { get; set; }

        public VsandSport Sport { get; set; }
        public ICollection<VsandRoundup> VsandRoundup { get; set; }
    }
}
