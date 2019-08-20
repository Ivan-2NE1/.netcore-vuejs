using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandGameReportPlayerStat
    {
        public int PlayerStatId { get; set; }
        public int GameReportId { get; set; }
        public int PlayerId { get; set; }
        public int StatId { get; set; }
        public double? StatValue { get; set; }

        public VsandGameReport GameReport { get; set; }
        public VsandPlayer Player { get; set; }
        public VsandSportPlayerStat SportPlayerStat { get; set; }
    }
}
