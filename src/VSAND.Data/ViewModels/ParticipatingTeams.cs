using VSAND.Data.Entities;
using NLog;

namespace VSAND.Data.ViewModels
{
    public class ParticipatingTeam
    {
        private readonly Logger Log = LogManager.GetCurrentClassLogger();

        public int SchoolId { get; set; } = 0;
        public int TeamId { get; set; } = 0;
        public string TeamName { get; set; } = "";
        public string State { get; set; } = "";
        public string Abbreviation { get; set; } = "";
        public bool HomeTeam { get; set; } = false;
        public double Score { get; set; } = 0;

        public ParticipatingTeam()
        {

        }

        public ParticipatingTeam(int iTeamId, bool bHomeTeam, double dScore)
        {
            this.TeamId = iTeamId;
            this.HomeTeam = bHomeTeam;
            this.Score = dScore;
        }

        public ParticipatingTeam(int iTeamId, string sTeamName, bool bHomeTeam, double dScore)
        {
            this.TeamId = iTeamId;
            this.TeamName = sTeamName;
            this.State = GetTeamState();
            this.HomeTeam = bHomeTeam;
            this.Score = dScore;
        }
        
        public ParticipatingTeam(VsandGameReportTeam oTeam)
        {
            if (oTeam != null)
            {
                TeamId = oTeam.TeamId;
                TeamName = oTeam.TeamName;

                if (oTeam.Team != null && oTeam.Team.SchoolId.HasValue)
                {
                    SchoolId = oTeam.Team.SchoolId.Value;
                }

                if (oTeam.Team != null)
                {                    
                    if (oTeam.Team.Name != null && !string.IsNullOrEmpty(oTeam.Team.Name))
                    {
                        TeamName = oTeam.Team.Name;
                    }
                    
                    if (string.IsNullOrEmpty(TeamName))
                    {
                        if (oTeam.Team.School != null)
                        {                            
                            TeamName = oTeam.Team.School.Name;
                        } else
                        {
                            Log.Warn($"School obj is null for {oTeam.TeamId}");
                        }                        
                    }
                } else
                {
                    Log.Warn($"Team obj is null for {oTeam.TeamId}");
                }

                State = GetTeamState();
                HomeTeam = oTeam.HomeTeam;
                Score = oTeam.FinalScore;
                if (oTeam.Abbreviation != null)
                {
                    Abbreviation = oTeam.Abbreviation;
                }                    
            }
        }

        public string GetTeamState()
        {
            string[] aTeam = TeamName.Split(new char[] { '(', ')' });
            string sState = "";
            if (aTeam.Length > 1)
                sState = aTeam[1].Replace(".", "").ToUpper();
            return sState;
        }

        public string TeamNameUnformatted()
        {
            if (string.IsNullOrEmpty(TeamName.Trim()))
                return "";
            else
            {
                string[] aTeam = TeamName.Split(new char[] { '(', ')' });
                return aTeam[0].Trim();
            }
        }

        public string TeamNameFormatted()
        {
            string sName = TeamName;
            if (!string.IsNullOrEmpty(State) && TeamId.Equals("0"))
            {
                if (State.Length >= 2)
                    sName = TeamNameUnformatted() + " (" + State[0].ToString().ToUpper() + State[1].ToString().ToLower() + ".)";
                else
                    sName = TeamNameUnformatted() + " (" + State + ")";
            }
            return sName;
        }
    }
}
