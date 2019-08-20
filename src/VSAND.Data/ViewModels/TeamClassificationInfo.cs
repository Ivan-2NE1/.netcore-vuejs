using System.Linq;
using VSAND.Data.Entities;

namespace VSAND.Data.ViewModels
{
    public class TeamClassificationInfo
    {
        public int TeamId { get; set; } = 0;
        public string TeamName { get; set; } = "";
        public string Section { get; set; } = "";
        public string Group { get; set; } = "";

        public TeamClassificationInfo()
        {
        }

        public TeamClassificationInfo(VsandTeam oTeam)
        {
            TeamId = oTeam.TeamId;
            TeamName = oTeam.Name;
            VsandTeamCustomCode oSection = oTeam.CustomCodes.FirstOrDefault(cc => cc.CodeName == "Section");
            if (oSection != null)
            {
                Section = oSection.CodeValue;
            }
            VsandTeamCustomCode oGroup = oTeam.CustomCodes.FirstOrDefault(cc => cc.CodeName == "Group");
            if (oGroup != null)
            {
                Group = oGroup.CodeValue;
            }
        }
    }


}
