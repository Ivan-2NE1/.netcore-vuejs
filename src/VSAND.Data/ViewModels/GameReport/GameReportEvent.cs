using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSAND.Data.Entities;

namespace VSAND.Data.ViewModels.GameReport
{
    public class GameReportEvent
    {
        public int EventId { get; set; }
        public int GameReportId { get; set; }
        public int SportEventId { get; set; }
        public int? SortOrder { get; set; }
        public int? RoundNumber { get; set; }
        public string RoundName { get; set; }
        public string EventName { get; set; }
        public string ResultHandler { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<GameReportEventResult> Results { get; set; }

        public GameReportEvent()
        {

        }

        public GameReportEvent(VsandGameReportEvent gre, List<VsandSportEvent> sportEvents)
        {
            this.EventId = gre.EventId;
            this.GameReportId = gre.EventId;
            this.SportEventId = gre.SportEventId;
            this.SortOrder = gre.SortOrder;
            this.RoundNumber = gre.RoundNumber;
            this.RoundName = gre.RoundName;

            if (gre.Results != null && gre.Results.Any())
            {
                this.Results = (from r in gre.Results orderby r.SortOrder ascending select new GameReportEventResult(r)).ToList();
            }

            if (sportEvents != null && sportEvents.Any())
            {
                var sportEvent = sportEvents.FirstOrDefault(se => se.SportEventId == gre.SportEventId);
                if (sportEvent != null)
                {
                    this.EventName = sportEvent.Name;

                    // we can clone some of this info to the event object so we don't have to always look it up
                    switch (sportEvent.ResultType.Trim().ToLowerInvariant())
                    {
                        case "individual":
                            this.ResultHandler = "event-result-individual";
                            break;
                        case "4player":
                            this.ResultHandler = "event-result-relay";
                            break;
                        case "custom":
                            // check the defined handler path and translate
                            switch (sportEvent.ResultHandler.Trim().ToLowerInvariant())
                            {
                                case "~/vsand/uc/sportevent/vsandtennissingles.ascx":
                                    this.ResultHandler = "event-result-tennis-singles";
                                    break;
                                case "~/vsand/uc/sportevent/vsandtennisdoubles.ascx":
                                    this.ResultHandler = "event-result-tennis-doubles";
                                    break;
                                case "~/vsand/uc/sportevent/vsandgolfmatchplay.ascx":
                                    this.ResultHandler = "event-result-golf-match-play";
                                    break;
                                case "~/vsand/uc/sportevent/vsandwrestlingbout.ascx":
                                    this.ResultHandler = "event-result-wrestling";
                                    break;
                                default:
                                    this.ResultHandler = "event-result-individual";
                                    break;
                            }
                            break;
                        default:
                            this.ResultHandler = "event-result-individual";
                            break;
                    }
                }
            }
        }

    }
}
