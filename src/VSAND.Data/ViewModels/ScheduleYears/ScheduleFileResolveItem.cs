using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSAND.Data.ViewModels.ScheduleYears
{
    public class ScheduleFileResolveItem
    {
        public string Name { get; set; }
        public int? SchoolId { get; set; }
        public int? TeamId { get; set; }
        public int? ResolveToSchoolId { get; set; }
        public string RenameTo { get; set; } = "";
        public string State { get; set; } = "";
        public string City { get; set; } = "";
        public bool PrivateSchool { get; set; } = false;
        public int CountyId { get; set; } = 0;
        public List<string> Opponents { get; set; }
        public List<ListItem<int>> ResolveToChoices { get; set; }
        public string ResolveMethod { get; set; } = "";
        public bool ResolveMethodAccept { get; set; } = false;
        public bool Processing { get; set; } = false;

        public ScheduleFileResolveItem()
        {
            Opponents = new List<string>();
            ResolveToChoices = new List<ListItem<int>>();
        }

        public ScheduleFileResolveItem(List<VSAND.Data.Entities.VsandScheduleLoadFileParse> allRows, string refName, int? refSchoolId, int? refTeamId)
        {
            Opponents = new List<string>();
            ResolveToChoices = new List<ListItem<int>>();

            Name = refName.Trim();
            SchoolId = refSchoolId;
            TeamId = refTeamId;
            RenameTo = refName.Trim();

            // get the unique list of opponents to try to assist with resolving
            var opps1 = (from fr in allRows where fr.TeamSchoolName.Equals(refName, StringComparison.OrdinalIgnoreCase) select fr.OpponentSchoolName).ToList();
            var opps2 = (from fr in allRows where fr.OpponentSchoolName.Equals(refName, StringComparison.OrdinalIgnoreCase) select fr.TeamSchoolName).ToList();
            var opps = new List<string>();
            if (opps1.Any())
            {
                opps.AddRange(opps1);
            }
            if (opps2.Any())
            {
                opps.AddRange(opps2);
            }
            Opponents = opps.Distinct().OrderBy(r => r).ToList();
        }
    }
}
