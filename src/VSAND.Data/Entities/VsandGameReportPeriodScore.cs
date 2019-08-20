using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandGameReportPeriodScore
    {
        public int PeriodScoreId { get; set; }
        public int GameReportTeamId { get; set; }
        public int PeriodNumber { get; set; }
        public bool IsOtperiod { get; set; }
        public double Score { get; set; }
        public string ScoreSpecial { get; set; }
        public bool IsSoperiod { get; set; }

        public VsandGameReportTeam GameReportTeam { get; set; }
    }
}
