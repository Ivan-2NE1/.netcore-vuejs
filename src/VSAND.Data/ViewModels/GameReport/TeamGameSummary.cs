using System;
using System.Collections.Generic;
using System.Linq;
using VSAND.Data.Entities;

namespace VSAND.Data.ViewModels.GameReport
{
    public class TeamGameSummary
    {
        public int GameReportId { get; }
        public DateTime GameDate { get; }
        public string ResultName { get; }
        public string ResultScore { get; }
        public bool Forfeit { get; }
        public int OpponentId { get; }
        public string Opponent { get; }
        public string OpponentConference { get; } = "";
        public string OpponentDivision { get; } = "";
        public string OpponentLeague { get; } = "";
        public string TeamConference { get; } = "";
        public string TeamDivision { get; } = "";
        public string GameType { get; }
        public bool OppIsOos { get; }
        public bool PowerPointsEligible { get; }
        public int RecordWins { get; set; } = 0;
        public int RecordLosses { get; set; } = 0;
        public int RecordTies { get; set; } = 0;
        public int ConferenceWins { get; set; } = 0;
        public int ConferenceLosses { get; set; } = 0;
        public int ConferenceTies { get; set; } = 0;
        public string OverallRecord
        {
            get
            {
                if (!Deleted && Final)
                {
                    return $"{RecordWins}-{RecordLosses}" + (RecordTies > 0 ? $"-{RecordTies}" : "");
                }
                return "";
            }
        }
        public string ConferenceRecord
        {
            get
            {
                if (!Deleted && Final)
                {
                    return $"{ConferenceWins}-{ConferenceLosses}" + (ConferenceTies > 0 ? $"-{ConferenceTies}" : "");
                }
                return "";
            }
        }
        public bool Deleted { get; }
        public bool ConferenceGame { get; set; } = false;
        public bool Final { get; }

        public TeamGameSummary()
        {

        }

        public TeamGameSummary(int refTeamId, VsandGameReport gr)
        {
            this.GameReportId = gr.GameReportId;
            this.GameDate = gr.GameDate;
            this.Deleted = gr.Deleted;
            this.PowerPointsEligible = gr.PPEligible;
            this.Forfeit = gr.Teams.Any(grt => grt.Forfeit);
            this.Final = gr.Final;

            bool lowScoreWins = gr.Sport.EnableLowScoreWins.HasValue ? gr.Sport.EnableLowScoreWins.Value : false;

            var teamResult = gr.Teams.FirstOrDefault(grt => grt.TeamId == refTeamId);
            VsandGameReportTeam oppResult = null;
            if (gr.Teams.Count == 1)
            {
                // there is no opponent
                Opponent = "TBA";
                OpponentId = 0;
                OpponentConference = "";
                OpponentDivision = "";
            }
            else
            {
                oppResult = gr.Teams.FirstOrDefault(grt => grt.TeamId != refTeamId);

                if (oppResult != null)
                {
                    OpponentId = oppResult.TeamId;
                    Opponent = oppResult.Team.Name;
                    OpponentConference = oppResult.Team.CustomCodes.FirstOrDefault(tcc => tcc.CodeName == "Conference")?.CodeValue ?? "";
                    OpponentDivision = oppResult.Team.CustomCodes.FirstOrDefault(tcc => tcc.CodeName == "Division")?.CodeValue ?? "";
                    OpponentLeague = OpponentConference;
                    if (OpponentDivision != "")
                    {
                        OpponentLeague = $"{OpponentConference} - {OpponentDivision}";
                    }

                    OppIsOos = !oppResult.Team.School.CoreCoverage;
                }
            }

            if (Final && oppResult != null)
            {
                bool teamScoreGreater = false;
                if (teamResult != null && oppResult != null)
                {
                    if (teamResult.FinalScore == oppResult.FinalScore)
                    {
                        ResultName = "T";
                    }
                    else
                    {
                        teamScoreGreater = teamResult.FinalScore > oppResult.FinalScore;
                        if (lowScoreWins && !teamScoreGreater || teamScoreGreater)
                        {
                            ResultName = "W";
                        }
                        else
                        {
                            ResultName = "L";
                        }
                    }

                    // Result score is the ref team score - opp team score
                    ResultScore = $"{teamResult.FinalScore}-{oppResult.FinalScore}";
                }

                TeamConference = teamResult.Team.CustomCodes.FirstOrDefault(tcc => tcc.CodeName == "Conference")?.CodeValue ?? "";
                TeamDivision = teamResult.Team.CustomCodes.FirstOrDefault(tcc => tcc.CodeName == "Division")?.CodeValue ?? "";

                GameType = gr.EventType.Name;
                if (gr.Section != null)
                {
                    GameType += " " + gr.Section.Name;

                    if (gr.Group != null)
                    {
                        GameType += ", " + gr.Group.Name;
                    }
                }
            }
        }
    }
}
