using System;
using System.Collections.Generic;
using System.Text;

namespace VSAND.Data.ViewModels.GameReport
{
    public class GameReportTeamStat
    {
        public int GameReportTeamId { get; set; }
        public int TeamStatId { get; set; }
        public int StatId { get; set; }
        public double? StatValue { get; set; }

        public GameReportTeamStat()
        {

        }

        public GameReportTeamStat(VSAND.Data.Entities.VsandGameReportTeamStat ts)
        {
            this.GameReportTeamId = ts.GameReportTeamId;
            this.TeamStatId = ts.TeamStatId;
            this.StatId = ts.StatId;
            this.StatValue = ts.StatValue;            
        }
    }
}
