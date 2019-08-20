using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandLeagueRule
    {
        public VsandLeagueRule()
        {
            RuleItems = new HashSet<VsandLeagueRuleItem>();            
        }

        public int LeagueRuleId { get; set; }
        public int SportId { get; set; }
        public int ScheduleYearId { get; set; }
        public string Conference { get; set; }
        public string Division { get; set; }
        public string RuleType { get; set; }

        public VsandSport Sport { get; set; }
        public VsandScheduleYear ScheduleYear { get; set; }

        public ICollection<VsandLeagueRuleItem> RuleItems { get; set; }
    }
}
