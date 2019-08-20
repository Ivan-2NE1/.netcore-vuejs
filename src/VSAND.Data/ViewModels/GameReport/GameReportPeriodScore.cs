using System;
using System.Collections.Generic;
using System.Text;

namespace VSAND.Data.ViewModels.GameReport
{
    public class GameReportPeriodScore
    {
        public int PeriodScoreId { get; set; }
        public int GameReportTeamId { get; set; }
        public int PeriodNumber { get; set; }
        public double Score { get; set; }
        public string ScoreSpecial { get; set; }
        public bool IsShootOutPeriod { get; set; }
        public bool IsOvertimePeriod { get; set; }

        public GameReportPeriodScore()
        {

        }

        public GameReportPeriodScore(VSAND.Data.Entities.VsandGameReportPeriodScore oScore)
        {
            this.PeriodScoreId = oScore.PeriodScoreId;
            this.GameReportTeamId = oScore.GameReportTeamId;
            this.PeriodNumber = oScore.PeriodNumber;
            this.Score = oScore.Score;
            this.ScoreSpecial = oScore.ScoreSpecial;
            this.IsShootOutPeriod = oScore.IsSoperiod;
            this.IsOvertimePeriod = oScore.IsOtperiod;
        }
    }
}
