using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSAND.Data.ViewModels
{
    public class AddGameReport
    {
        public int SportId { get; set; }
        public int refTeamId { get; set; }
        public int EventTypeId { get; set; }
        public int RoundId { get; set; }
        public int SectionId { get; set; }
        public int GroupId { get; set; }
        public DateTime GameDate { get; set; }
        public int ScheduleYearId { get; set; }
        public string LocationName { get; set; } = "";
        public string LocationCity { get; set; } = "";
        public string LocationState { get; set; } = "";
        public string Source { get; set; } = "Web";
        public string Notes { get; set; } = "";
        public List<ParticipatingTeam> Teams { get; set; }
        public List<GameReportMeta> Meta { get; set; }

        public AddGameReport()
        {
            Teams = new List<ParticipatingTeam>();
            Meta = new List<GameReportMeta>();
        }

        public void Initialize()
        {
            if (Teams == null)
            {
                Teams = new List<ParticipatingTeam>();
            }
           
            var oHomeShim = new ParticipatingTeam(0, "", true, 0);
            var oAwayShim = new ParticipatingTeam(0, "", false, 0);
            if (Teams.Count == 0)
            {
                Teams.Add(oHomeShim);
                Teams.Add(oAwayShim);
            } else
            {
                if (Teams.Count < 2)
                {
                    if (Teams.Any(t => t.HomeTeam))
                    {
                        Teams.Add(oAwayShim);
                    } else
                    {
                        Teams.Add(oHomeShim);
                    }
                }
            }            
        }
    }
}
