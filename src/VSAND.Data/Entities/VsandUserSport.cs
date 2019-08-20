using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandUserSport
    {
        public int UserSportId { get; set; }
        public int AdminId { get; set; }
        public int SportId { get; set; }

        public AppxUser Admin { get; set; }
        public VsandSport Sport { get; set; }
    }
}
