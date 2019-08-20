using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using VSAND.Data.Entities;

namespace VSAND.Data.ViewModels.GameReport
{
    public class GameReport
    {
        public int GameReportId { get; set; }
        public string Name { get; set; }
        public DateTime GameDate { get; set; }
        public DateTime ReportedDate { get; set; }
        public bool Final { get; set; }
        public int EventTypeId { get; set; }
        public int? RoundId { get; set; }
        public int? SectionId { get; set; }
        public int? GroupId { get; set; }
        public int SportId { get; set; }
        public int ScheduleYearId { get; set; }
        public string LocationName { get; set; }
        public string LocationCity { get; set; }
        public string LocationState { get; set; }
        public bool Deleted { get; set; }
        public bool Exhibition { get; set; }
        public string AddNote { get; set; } = "";
        public EventTypeListItem EventTypeItem { get; set; }
        public Sports.Sport Sport { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<GameReportNote> Notes { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<GameReportTeam> Teams { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<ParticipatingTeam> ParticipatingTeams { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<GameReportMeta> Meta { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<GameReportEvent> Events { get; set; }

        public GameReport()
        {

        }

        public GameReport(VsandGameReport oGame)
        {
            GameReportId = oGame.GameReportId;
            Name = oGame.Name;
            GameDate = oGame.GameDate;
            Final = oGame.Final;
            SportId = oGame.SportId;
            ScheduleYearId = oGame.ScheduleYearId;
            ReportedDate = oGame.ReportedDate;
            LocationName = oGame.LocationName;
            LocationCity = oGame.LocationCity;
            LocationState = oGame.LocationState;
            Deleted = oGame.Deleted;
            Exhibition = oGame.Exhibition;
            EventTypeId = oGame.GameTypeId;
            RoundId = oGame.RoundId;
            SectionId = oGame.SectionId;
            GroupId = oGame.GroupId;

            EventTypeItem = new EventTypeListItem(oGame.GameTypeId, "", oGame.RoundId ?? 0, "", oGame.SectionId ?? 0, "", oGame.GroupId ?? 0, "");

            List<VsandSportEvent> sportEvents = null;
            if (oGame.Sport != null)
            {
                Sport = new Sports.Sport(oGame.Sport);
                if (oGame.Sport.SportEvents != null && oGame.Sport.SportEvents.Any())
                {
                    sportEvents = oGame.Sport.SportEvents.ToList();
                }
            }

            if (oGame.Meta != null && oGame.Meta.Any())
            {
                Meta = (from gm in oGame.Meta select new GameReportMeta { SportGameMetaId = gm.SportGameMetaId, MetaValue = gm.MetaValue }).ToList();
            }

            if (oGame.Events != null && oGame.Events.Any())
            {
                Events = (from e in oGame.Events orderby e.SortOrder ascending select new GameReportEvent(e, sportEvents)).ToList();
            }

            if (oGame.Notes != null && oGame.Notes.Any())
            {
                Notes = (from n in oGame.Notes orderby n.NoteDate descending select new GameReportNote(n)).ToList();
            }

            if (oGame.Teams != null && oGame.Teams.Any()) {
                ParticipatingTeams = (from t in oGame.Teams select new ParticipatingTeam(t)).ToList();
                Teams = (from t in oGame.Teams select new GameReportTeam(t)).ToList();

                if (oGame.PlayerStats != null && oGame.PlayerStats.Any())
                {
                    foreach(var team in Teams)
                    {
                        if (team.RosterEntries != null && team.RosterEntries.Any())
                        {
                            var teamRoster = team.RosterEntries;
                            // we can use this to filter the player stats to this team
                            var playerIds = (from tr in teamRoster select tr.PlayerId).ToList();
                            var playerStats = (from ps in oGame.PlayerStats where playerIds.Contains(ps.PlayerId) select new GameReportPlayerStat(ps)).ToList();
                            team.PlayerStats = playerStats;
                        }
                    }
                }
            }
        }
    }
}
