using System.Collections.Generic;
using System.Linq;
using VSAND.Data.Entities;

namespace VSAND.Data.ViewModels
{
    public class LeagueRule
    {
        public int LeagueRuleId { get; set; }
        public int SportId { get; set; }
        public int ScheduleYearId { get; set; }
        public string Conference { get; set; }
        public string Division { get; set; }
        public string RuleType { get; set; }

        public List<ListItem<string>> RuleItems { get; set; }

        public LeagueRule()
        {

        }

        public LeagueRule(VsandLeagueRule entity)
        {
            LeagueRuleId = entity.LeagueRuleId;
            SportId = entity.SportId;
            ScheduleYearId = entity.ScheduleYearId;
            Conference = entity.Conference;
            Division = entity.Division;
            RuleType = entity.RuleType;
            RuleItems = entity.RuleItems.Select(ri => new ListItem<string>
            {
                name = $"{ri.Conference} - {ri.Division}",
                id = $"{ri.Conference}|{ri.Division}"
            }).ToList();
        }

        public VsandLeagueRule ToEntityModel()
        {
            // this entity model method does not yield RuleItems, as those are better handled in the Service layer
            // they must be added / removed with respect to the RuleItems that already exist in the database

            return new VsandLeagueRule
            {
                LeagueRuleId = this.LeagueRuleId,
                SportId = this.SportId,
                ScheduleYearId = this.ScheduleYearId,
                Conference = this.Conference,
                Division = this.Division,
                RuleType = this.RuleType
            };
        }
    }
}
