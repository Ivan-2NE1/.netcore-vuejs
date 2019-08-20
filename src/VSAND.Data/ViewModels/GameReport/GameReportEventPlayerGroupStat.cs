using System;
using System.Collections.Generic;
using System.Text;
using VSAND.Data.Entities;

namespace VSAND.Data.ViewModels.GameReport
{
    public class GameReportEventPlayerGroupStat
    {
        public int PlayerGroupStatId { get; set; }
        public int PlayerGroupId { get; set; }
        public int StatId { get; set; }
        public double? StatValue { get; set; }

        public GameReportEventPlayerGroupStat()
        {

        }

        public GameReportEventPlayerGroupStat(VsandGameReportEventPlayerGroupStat stat)
        {
            this.PlayerGroupStatId = stat.PlayerGroupStatId;
            this.PlayerGroupId = stat.PlayerGroupId;
            this.StatId = stat.StatId;
            this.StatValue = stat.StatValue;
        }
    }
}
