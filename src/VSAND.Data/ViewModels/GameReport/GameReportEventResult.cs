using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSAND.Data.Entities;

namespace VSAND.Data.ViewModels.GameReport
{
    public class GameReportEventResult
    {
        public int EventResultId { get; set; }
        public int EventId { get; set; }
        public int? SortOrder { get; set; }
        public string ResultType { get; set; }
        public string Duration { get; set; }
        public string Overtime { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<GameReportEventPlayer> Players { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<GameReportEventPlayerGroup> Groups { get; set; }

        public GameReportEventResult()
        {

        }

        public GameReportEventResult(VsandGameReportEventResult grer)
        {
            this.EventResultId = grer.EventResultId;
            this.EventId = grer.EventId;
            this.SortOrder = grer.SortOrder;
            this.ResultType = grer.ResultType;
            this.Duration = grer.Duration;
            this.Overtime = grer.Overtime;

            if (grer.EventPlayers != null && grer.EventPlayers.Any())
            {
                this.Players = (from p in grer.EventPlayers select new GameReportEventPlayer(p)).ToList();
            }

            if (grer.EventPlayerGroups != null && grer.EventPlayerGroups.Any())
            {
                this.Groups = (from p in grer.EventPlayerGroups select new GameReportEventPlayerGroup(p)).ToList();
            }
        }
    }
}
