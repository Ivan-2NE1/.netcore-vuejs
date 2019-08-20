using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandLeagueRuleItem
    {
        public int LeagueRuleId { get; set; }
        public string Conference { get; set; }
        public string Division { get; set; }

        public VsandLeagueRule LeagueRule { get; set; }
    }
}
