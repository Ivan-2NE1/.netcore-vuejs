using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using VSAND.Data.Entities;
using VSAND.Data.ViewModels.GameReport;
using VSAND.Data.ViewModels.Sports;

namespace VSAND.Data.ViewModels
{
    public class GameReportSummary
    {
        public int GameReportId { get; set; }
        public string Name { get; set; }
        public DateTime GameDate { get; set; }
        public DateTime ReportedDate { get; set; }
        public int SportId { get; set; }
        public int CountyId { get; set; }
        public string SportName { get; set; }
        public bool HasFinalScore { get; set; } = false;
        public bool HasPeriodScores { get; set; } = false;
        public bool HasHomeRoster { get; set; } = false;
        public bool HasAwayRoster { get; set; } = false;
        public bool HasHomeTeamStats { get; set; } = false;
        public bool HasAwayTeamStats { get; set; } = false;
        public bool HasHomePlayerStats { get; set; } = false;
        public bool HasAwayPlayerStats { get; set; } = false;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<int> Publications { get; set; }
        public Sport Sport { get; set; }
        public IEnumerable<GameReportTeam> Teams { get; }
   

        public GameReportSummary()
        {

        }

        public GameReportSummary(VsandGameReport gameReport)
        {
            GameReportId = gameReport.GameReportId;
            Name = gameReport.Name;
            GameDate = gameReport.GameDate;
            ReportedDate = gameReport.ReportedDate;
            SportId = gameReport.SportId;
            SportName = gameReport.Sport.Name;
            CountyId = gameReport.CountyId;
            HasFinalScore = gameReport.Final;
            Sport = new Sport(gameReport.Sport);

            if (gameReport.Teams != null && gameReport.Teams.Any())
            {
                Teams = (from t in gameReport.Teams select new GameReportTeam(t)).ToList();
                //TODO: This is not very efficient, but i cloned the publication list up to the game for easier javascript filtering
                // Pull any publication ids up from the team level to the game level for easier reference later on
                Publications = (from t in Teams where t.Publications != null from p in t.Publications select p).Distinct().ToList();
            }
        }
    }
}
