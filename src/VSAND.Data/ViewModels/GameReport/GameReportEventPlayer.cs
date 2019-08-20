using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using VSAND.Data.Entities;
using Newtonsoft.Json;

namespace VSAND.Data.ViewModels.GameReport
{
    public class GameReportEventPlayer
    {
        public int EventPlayerId { get; set; }
        public int GameReportTeamId { get; set; }
        public int EventResultId { get; set; }
        public int? PlayerId { get; set; }
        public bool? Winner { get; set; }
        public double? Score { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<GameReportEventPlayerStat> Stats { get; set; }

        public GameReportEventPlayer()
        {

        }

        public GameReportEventPlayer(VsandGameReportEventPlayer grep)
        {
            this.EventPlayerId = grep.EventPlayerId;
            this.GameReportTeamId = grep.GameReportTeamId;
            this.EventResultId = grep.EventResultId;
            this.PlayerId = grep.PlayerId;
            this.Winner = grep.Winner;
            this.Score = grep.Score;

            if (grep.GameReportEventPlayerStats != null && grep.GameReportEventPlayerStats.Any())
            {
                this.Stats = (from s in grep.GameReportEventPlayerStats select new GameReportEventPlayerStat(s)).ToList();
            }
        }
    }
}
