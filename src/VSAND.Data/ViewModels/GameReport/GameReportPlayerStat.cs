using System;
using System.Collections.Generic;
using System.Text;
using VSAND.Data.Entities;

namespace VSAND.Data.ViewModels
{
    public class GameReportPlayerStat
    {
        public int PlayerStatId { get; set; }
        public int GameReportId { get; set; }
        public int PlayerId { get; set; }
        public int StatId { get; set; }
        public double? StatValue { get; set; }

        public GameReportPlayerStat()
        {

        }

        public GameReportPlayerStat(VsandGameReportPlayerStat stat)
        {
            this.PlayerStatId = stat.PlayerStatId;
            this.GameReportId = stat.GameReportId;
            this.PlayerId = stat.PlayerId;
            this.StatId = stat.StatId;
            this.StatValue = stat.StatValue;
        }
    }
}
