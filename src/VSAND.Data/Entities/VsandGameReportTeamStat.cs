using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandGameReportTeamStat
    {
        public int TeamStatId { get; set; }
        public int GameReportTeamId { get; set; }
        public int StatId { get; set; }
        public double? StatValue { get; set; }

        public VsandGameReportTeam GameReportTeam { get; set; }
        public VsandSportTeamStat SportTeamStat { get; set; }
    }
}
