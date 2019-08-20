using System;
using System.Collections.Generic;
using System.Text;
using VSAND.Data.Entities;

namespace VSAND.Data.ViewModels.GameReport
{
    public class GameReportEventPlayerStat
    {
        public int EventPlayerStatId { get; set; }
        public int EventPlayerId { get; set; }
        public int StatId { get; set; }
        public double StatValue { get; set; }

        public GameReportEventPlayerStat()
        {

        }

        public GameReportEventPlayerStat(VsandGameReportEventPlayerStat greps)
        {
            this.EventPlayerStatId = greps.EventPlayerStatId;
            this.EventPlayerId = greps.EventPlayerId;
            this.StatId = greps.StatId;
            this.StatValue = greps.StatValue;
        }
    }
}
