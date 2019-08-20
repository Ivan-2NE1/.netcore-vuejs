using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandGameReportEventPlayerStat
    {
        public int EventPlayerStatId { get; set; }
        public int EventPlayerId { get; set; }
        public int StatId { get; set; }
        public double StatValue { get; set; }

        public VsandGameReportEventPlayer EventPlayer { get; set; }
        public VsandSportEventStat SportEventStat { get; set; }
    }
}
