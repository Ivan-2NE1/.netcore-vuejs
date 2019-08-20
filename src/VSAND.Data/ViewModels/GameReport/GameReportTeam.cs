using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using VSAND.Data.Entities;

namespace VSAND.Data.ViewModels.GameReport
{
    public class GameReportTeam
    {
        public int GameReportTeamId { get; set; }
        public int TeamId { get; set; }
        public int SchoolId { get; set; }
        public string Name { get; set; }
        public string Abbreviaton { get; set; }
        public bool HomeTeam { get; set; }
        public double FinalScore { get; set; }
        public bool WinningTeam { get; set; }
        public bool Forfeit { get; set; }
        public string Conference { get; set; }
        public List<int> Publications { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<GameReportTeamStat> TeamStats { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<GameReportPeriodScore> PeriodScores { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<RosterEntry> RosterEntries{ get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<GameReportRosterEntry> GameRosterEntries { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<GameReportPlayerStat> PlayerStats { get; set; }

        public GameReportTeam()
        {

        }

        public GameReportTeam(VsandGameReportTeam team)
        {
            GameReportTeamId = team.GameReportTeamId;
            TeamId = team.TeamId;
            SchoolId = team.Team?.SchoolId ?? 0;
            Name = team.TeamName;
            Abbreviaton = team.Abbreviation;
            HomeTeam = team.HomeTeam;
            FinalScore = team.FinalScore;
            WinningTeam = team.WinningTeam;
            Forfeit = team.Forfeit;
            if (team.Team != null)
            {
                if (team.Team.CustomCodes.Any(cc => cc.CodeName.Equals("Conference")))
                {
                    Conference = team.Team.CustomCodes.FirstOrDefault(cc => cc.CodeName.Equals("Conference")).CodeValue;
                }

                if (team.Team.School != null)
                {
                    var oSchool = team.Team.School;
                    if (oSchool.VsandPublicationSchool.Any())
                    {
                        Publications = (from ps in oSchool.VsandPublicationSchool select ps.PublicationId).ToList();
                    }
                }

                if (team.PeriodScores != null && team.PeriodScores.Any())
                {
                    this.PeriodScores = (from ps in team.PeriodScores orderby ps.PeriodNumber ascending select new GameReportPeriodScore(ps)).ToList();
                }

                if (team.TeamStats != null && team.TeamStats.Any())
                {
                    this.TeamStats = (from ts in team.TeamStats select new GameReportTeamStat(ts)).ToList();
                }

                if (team.Team.RosterEntries != null && team.Team.RosterEntries.Any())
                {
                    this.RosterEntries = (from r in team.Team.RosterEntries orderby r.JerseyNumber ascending select new RosterEntry(r)).ToList();
                }

                if (team.GameRoster != null && team.GameRoster.Any())
                {
                    this.GameRosterEntries = (from grre in team.GameRoster select new GameReportRosterEntry(grre)).ToList();
                }
            }
        }
    }
}
