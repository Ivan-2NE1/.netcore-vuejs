using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSAND.Data.Entities;

namespace VSAND.Data.ViewModels.Teams
{
    public class TeamOosRecord
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public int GroupExchange { get; set; } = 0;
        public int Wins { get; set; } = 0;
        public int Losses { get; set; } = 0;
        public int Ties { get; set; } = 0;
        public DateTime? LastUpdated { get; set; }

        public TeamOosRecord()
        {

        }

        public TeamOosRecord(VsandTeam team, List<VsandTeamCustomCode> codes)
        {
            TeamId = team.TeamId;
            TeamName = team.Name.Trim();
            if (string.IsNullOrEmpty(TeamName))
            {
                TeamName = team.School.Name.Trim();
            }

            var groupExchange = codes.FirstOrDefault(tcc => tcc.CodeName.Equals("GroupExchange", StringComparison.OrdinalIgnoreCase));
            if (groupExchange != null)
            {
                GroupExchange = groupExchange.GetValue<int>();
            }

            var recordWins = codes.FirstOrDefault(tcc => tcc.CodeName.Equals("OOSFinalWins", StringComparison.OrdinalIgnoreCase));
            if (recordWins != null)
            {
                Wins = recordWins.GetValue<int>();
            }

            var recordLosses = codes.FirstOrDefault(tcc => tcc.CodeName.Equals("OOSFinalLosses", StringComparison.OrdinalIgnoreCase));
            if (recordLosses != null)
            {
                Losses = recordLosses.GetValue<int>();
            }

            var recordTies = codes.FirstOrDefault(tcc => tcc.CodeName.Equals("OOSFinalTies", StringComparison.OrdinalIgnoreCase));
            if (recordTies != null)
            {
                Ties = recordTies.GetValue<int>();
            }

            var recordUpdated = codes.FirstOrDefault(tcc => tcc.CodeName.Equals("OOSUpdate", StringComparison.OrdinalIgnoreCase));
            if (recordUpdated != null)
            {
                LastUpdated = recordUpdated.GetValue<DateTime?>();
            }
        }
    }
}
