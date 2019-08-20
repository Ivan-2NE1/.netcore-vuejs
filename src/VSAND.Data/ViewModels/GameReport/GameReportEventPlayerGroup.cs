using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using VSAND.Data.Entities;

namespace VSAND.Data.ViewModels.GameReport
{
    public class GameReportEventPlayerGroup
    {
        public int PlayerGroupId { get; set; }
        public int GameReportTeamId { get; set; }
        public int EventResultId { get; set; }
        public bool? Winner { get; set; }
        public List<GameReportEventPlayerGroupPlayer> Players { get; set; }
        public List<GameReportEventPlayerGroupStat> Stats { get; set; }

        public GameReportEventPlayerGroup()
        {

        }

        public GameReportEventPlayerGroup(VsandGameReportEventPlayerGroup grepg)
        {
            this.PlayerGroupId = grepg.PlayerGroupId;
            this.GameReportTeamId = grepg.GameReportTeamId;
            this.EventResultId = grepg.EventResultId;
            this.Winner = grepg.Winner;

            if (grepg.EventPlayerGroupPlayers != null && grepg.EventPlayerGroupPlayers.Any())
            {
                this.Players = (from p in grepg.EventPlayerGroupPlayers orderby p.SortOrder ascending select new GameReportEventPlayerGroupPlayer(p)).ToList();
            }

            if (grepg.EventPlayerGroupPlayers != null && grepg.GameReportEventPlayerGroupStats.Any())
            {
                this.Stats = (from s in grepg.GameReportEventPlayerGroupStats select new GameReportEventPlayerGroupStat(s)).ToList();
            }
        }
    }
}
