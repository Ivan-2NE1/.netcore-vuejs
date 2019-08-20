using System.Collections.Generic;
using System.Linq;
using VSAND.Data.Entities;

namespace VSAND.Data.ViewModels
{
    public class TeamComplianceInfo
    {
        public int TeamId { get; set; } = 0;
        public string TeamName { get; set; } = "";
        public int EventCount { get; set; } = 0;
        public int RosterCount { get; set; } = 0;
        public int UnvalidatedCount { get; set; } = 0;

        public TeamComplianceInfo()
        {
        }

        public TeamComplianceInfo(VsandTeam oTeam)
        {
            TeamId = oTeam.TeamId;
            TeamName = oTeam.Name;
            RosterCount = oTeam.RosterEntries.Count;

            // TODO: make event count a parameter
            // int iEventCount = VSAND.Helper.TeamSchedule.GetTeamScheduleCount(oTeam.TeamId);
            EventCount = 0; // iEventCount;

            ICollection<VsandTeamRoster> oRoster = oTeam.RosterEntries;
            int iUnvalidated = oRoster.Where(tr => tr.Player.Validated == false).Count();
            UnvalidatedCount = iUnvalidated;
        }
    }
}
