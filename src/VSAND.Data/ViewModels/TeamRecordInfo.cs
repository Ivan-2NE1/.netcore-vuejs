using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using VSAND.Common;
using VSAND.Data.Entities;
using VSAND.Data.ViewModels.GameReport;

namespace VSAND.Data.ViewModels
{
    public class TeamRecordInfo : IComparable<TeamRecordInfo>
    {
        private readonly Logger Log = LogManager.GetCurrentClassLogger();

        private int _maxGames = 0;

        public int TeamId { get; set; } = 0;
        public string TeamName { get; set; } = "";
        public int SchoolId { get; set; } = 0;
        public string County { get; set; } = "";
        public string Section { get; set; } = "";
        public string Group { get; set; } = "";
        public string Conference { get; set; } = "";
        public string Division { get; set; } = "";
        public int NorthingNumber { get; set; } = 0;
        public string SuperSection { get; set; } = "";
        public string SuperSectionAbbr { get; set; } = "";
        public int Wins { get; set; } = 0;
        public int Losses { get; set; } = 0;
        public int Ties { get; set; } = 0;
        public int ConferenceWins { get; set; } = 0;
        public int ConferenceLosses { get; set; } = 0;
        public int ConferenceTies { get; set; } = 0;
        public int DivisionWins { get; set; } = 0;
        public int DivisionLosses { get; set; } = 0;
        public int DivisionTies { get; set; } = 0;
        public int LeagueWins { get; set; } = 0;
        public int LeagueLosses { get; set; } = 0;
        public int LeagueTies { get; set; } = 0;
        public int HomeWins { get; set; } = 0;
        public int HomeLosses { get; set; } = 0;
        public int HomeTies { get; set; } = 0;
        public int RoadWins { get; set; } = 0;
        public int RoadLosses { get; set; } = 0;
        public int RoadTies { get; set; } = 0;
        public double WinningPercentage { get; set; } = 0;
        public int InStateGames { get; set; } = 0;
        public double InStatePercentage { get; set; } = 0;
        public double PointsFor { get; set; } = 0;
        public double PointsAgainst { get; set; } = 0;
        public double PowerPoints { get; set; } = 0;
        public int PowerPointsGames { get; set; } = 0;
        public int PowerPointsRank { get; set; } = 0;
        public double BornPowerIndex { get; set; } = 0;
        public int BornPowerIndexRank { get; set; } = 0;
        public int UPRRank { get; set; } = 0;
        public int SuperSectionSeed { get; set; } = 0;
        public int Seed16Rank { get; set; } = 0;
        public int Seed8Rank { get; set; } = 0;
        public DateTime PPCalcDate { get; set; } = DateHelp.SqlMinDate;
        public double Differential { get; set; } = 0;
        public int RecordLimit { get; set; }
        public DateTime? LastReport { get; set; }
        public DateTime FirstGameDate { get; set; } = DateHelp.SqlMinDate;
        public DateTime LastGameDate { get; set; } = DateHelp.SqlMaxDate;
        public List<VsandGameReport> oGameHistory { get; set; } = null;
        public List<GameReportTeam> GameReportEntries { get; set; }
        public VsandSchool School { get; set; }

        public string OverallRecord
        {
            get {
                string sRet = Wins + "-" + Losses;
                if (Ties > 0)
                {
                    sRet = sRet + "-" + Ties;
                }
                return sRet;
            }
        }

        public string ConferenceRecord
        {
            get {
                string sRet = ConferenceWins + "-" + ConferenceLosses;
                if (ConferenceTies > 0)
                {
                    sRet = sRet + "-" + ConferenceTies;
                }
                return sRet;
            }
        }

        public string DivisionRecord
        {
            get {
                string sRet = DivisionWins + "-" + DivisionLosses;
                if (DivisionTies > 0)
                {
                    sRet = sRet + "-" + DivisionTies;
                }
                return sRet;
            }
        }

        public string LeagueRecord
        {
            get {
                string sRet = LeagueWins + "-" + LeagueLosses;
                if (LeagueTies > 0)
                {
                    sRet = sRet + "-" + LeagueTies;
                }
                return sRet;
            }
        }

        public string HomeRecord
        {
            get {
                string sRet = HomeWins + "-" + HomeLosses;
                if (HomeTies > 0)
                {
                    sRet = sRet + "-" + HomeTies;
                }
                return sRet;
            }
        }

        public string RoadRecord
        {
            get {
                string sRet = RoadWins + "-" + RoadLosses;
                if (RoadTies > 0)
                {
                    sRet = sRet + "-" + RoadTies;
                }
                return sRet;
            }
        }

        public double PowerPointsAverage
        {
            get {
                if (PowerPoints > 0 && PowerPointsGames > 0)
                {
                    return Math.Round(PowerPoints / PowerPointsGames, 2, MidpointRounding.AwayFromZero);
                }
                return 0;
            }
        }


        public double UnitedPowerRank
        {
            get {
                if (PowerPointsRank > 0 && BornPowerIndexRank > 0)
                {
                    return Math.Round((BornPowerIndexRank * 0.6) + (PowerPointsRank * 0.4), 2);
                }
                return 0;
            }
        }

        public TeamRecordInfo()
        {
        }

        public TeamRecordInfo(VsandTeam oTeam)
        {
            if (oTeam != null)
            {
                PopTeamData(oTeam, false);
            }
        }

        public TeamRecordInfo(VsandTeam oTeam, bool useCached)
        {
            if (oTeam != null)
            {
                PopTeamData(oTeam, useCached);
            }
        }

        public TeamRecordInfo(VsandTeam oTeam, DateTime startDate, DateTime EndDate)
        {
            FirstGameDate = startDate;
            LastGameDate = new DateTime(EndDate.Year, EndDate.Month, EndDate.Day, 23, 59, 59);
            if (oTeam != null)
            {
                PopTeamData(oTeam, false);
            }
        }

        public TeamRecordInfo(VsandTeam oTeam, DateTime startDate, DateTime EndDate, int MaxGames)
        {
            FirstGameDate = startDate;
            LastGameDate = new DateTime(EndDate.Year, EndDate.Month, EndDate.Day, 23, 59, 59);
            _maxGames = MaxGames;
            if (oTeam != null)
            {
                PopTeamData(oTeam, false);
            }
        }

        public TeamRecordInfo(VsandTeam oTeam, DateTime startDate, DateTime EndDate, bool UseCached)
        {
            FirstGameDate = startDate;
            LastGameDate = new DateTime(EndDate.Year, EndDate.Month, EndDate.Day, 23, 59, 59);
            if (oTeam != null)
            {
                PopTeamData(oTeam, UseCached);
            }
        }

        public TeamRecordInfo(VsandTeam oTeam, DateTime startDate, DateTime EndDate, int MaxGames, bool UseCached)
        {
            FirstGameDate = startDate;
            LastGameDate = new DateTime(EndDate.Year, EndDate.Month, EndDate.Day, 23, 59, 59);
            _maxGames = MaxGames;
            if (oTeam != null)
            {
                PopTeamData(oTeam, UseCached);
            }
        }

        // TODO: deal with commented out function calls in this method
        private void PopTeamData(VsandTeam oTeam, bool useCached)
        {
            TeamId = oTeam.TeamId;
            TeamName = oTeam.Name;
            SchoolId = oTeam.SchoolId.Value;
            //CustomCodes = oTeam.CustomCodes;
            GameReportEntries = (from gr in oTeam.GameReportEntries select new GameReport.GameReportTeam(gr)).ToList();

            if (oTeam.School != null)
            {
                if (oTeam.School.County != null)
                {
                    County = oTeam.School.County.Name;
                }
            }
            List<VsandTeamCustomCode> oCC = oTeam.CustomCodes.ToList();
            VsandTeamCustomCode oSection = oCC.FirstOrDefault(cc => cc.CodeName == "Section");
            if (oSection != null)
            {
                Section = oSection.CodeValue;
            }
            VsandTeamCustomCode oGroup = oCC.FirstOrDefault(cc => cc.CodeName == "Group");
            if (oGroup != null)
            {
                Group = oGroup.CodeValue;
            }
            VsandTeamCustomCode oConf = oCC.FirstOrDefault(cc => cc.CodeName == "Conference");
            if (oConf != null)
            {
                Conference = oConf.CodeValue;
            }
            VsandTeamCustomCode oDiv = oCC.FirstOrDefault(cc => cc.CodeName == "Division");
            if (oDiv != null)
            {
                Division = oDiv.CodeValue;
            }

            if (useCached)
            {
                Wins = SetCachedValue(oCC, "Record-Overall-Wins");
                Losses = SetCachedValue(oCC, "Record-Overall-Losses");
                Ties = SetCachedValue(oCC, "Record-Overall-Ties");
                ConferenceWins = SetCachedValue(oCC, "Record-Conference-Wins");
                ConferenceLosses = SetCachedValue(oCC, "Record-Conference-Losses");
                ConferenceTies = SetCachedValue(oCC, "Record-Conference-Ties");
                DivisionWins = SetCachedValue(oCC, "Record-Division-Wins");
                DivisionLosses = SetCachedValue(oCC, "Record-Division-Losses");
                DivisionTies = SetCachedValue(oCC, "Record-Division-Ties");
                if (Wins > 0 | Losses > 0)
                {
                    WinningPercentage = Math.Round((Wins / (double)(Wins + Losses + Ties)), 3);
                }
                if (oTeam.School != null)
                {
                    //VSAND.Helper.Team.CalculateRecord(oTeam.TeamId, oGameHistory, Wins, Losses, Ties, ConferenceWins, ConferenceLosses, ConferenceTies, DivisionWins, DivisionLosses, DivisionTies, LeagueWins, LeagueLosses, LeagueTies, HomeWins, HomeLosses, HomeTies, RoadWins, RoadLosses, RoadTies, InStateGames, oTeam.School.State, PointsFor, PointsAgainst, "");
                }
                
                if (Wins > 0 | Ties > 0 | Losses > 0)
                {
                    InStatePercentage = (InStateGames / (double)(Wins + Losses + Ties));
                }
                
                if (oTeam.ScheduleYearId < 12)
                {
                    // -- Before the 2016-17 SY, the League record was the division record
                    LeagueWins = DivisionWins;
                    LeagueLosses = DivisionLosses;
                    LeagueTies = DivisionTies;
                }
                else
                {
                    LeagueWins = SetCachedValue(oCC, "Record-League-Wins");
                    LeagueLosses = SetCachedValue(oCC, "Record-League-Losses");
                    LeagueTies = SetCachedValue(oCC, "Record-League-Ties");
                }
                HomeWins = SetCachedValue(oCC, "Record-Home-Wins");
                HomeLosses = SetCachedValue(oCC, "Record-Home-Losses");
                HomeTies = SetCachedValue(oCC, "Record-Home-Ties");
                RoadWins = SetCachedValue(oCC, "Record-Road-Wins");
                RoadLosses = SetCachedValue(oCC, "Record-Road-Losses");
                RoadTies = SetCachedValue(oCC, "Record-Road-Ties");
                PointsFor = SetCachedValue(oCC, "PointsFor");
                PointsAgainst = SetCachedValue(oCC, "PointsAgainst");

                if (oTeam.Sport != null)
                {
                    if (oTeam.Sport.EnablePowerPoints.Value)
                    {
                        PowerPoints = SetCachedValue(oCC, "PowerPoints");
                        // -- Deprecated 2018108 Per C. Faytok - no longer required by any sports
                        // TieBreak = SetCachedValue(oCC, "PowerPointsTieBreak")

                        // Dim oPpCalcDate As VsandTeamCustomCode = oCC.FirstOrDefault(Function(cc As VsandTeamCustomCode) cc.CodeName = "PowerPointsDate")
                        // If oPpCalcDate IsNot Nothing Then
                        // DateTime.TryParse(oPpCalcDate.CodeValue, PPCalcDate)
                        // End If

                        // If PPCalcDate <= DateHelp.SqlMaxDate Then
                        // -- Powerpoints are not calculated
                        if (oGameHistory == null)
                        {
                            // oGameHistory = GameReport.GetTeamReportHistory(TeamId);
                        }

                        if (_maxGames > 0)
                        {
                            // PowerPoints = VSAND.Helper.PowerPoints.Calculate(oTeam.TeamId, oGameHistory, FirstGameDate, LastGameDate, _maxGames, oTeam.Sport.PowerPointsDataType);
                        }
                        else
                        {
                            // PowerPoints = VSAND.Helper.PowerPoints.Calculate(oTeam.TeamId, oGameHistory, FirstGameDate, LastGameDate, oTeam.Sport.PowerPointsDataType);
                        }

                        // -- Current just running the alternate calculation to cache out the results
                        // VSAND.Helper.PowerPoints.Calculate(oTeam.TeamId, oGameHistory)

                        // Me.PowerPoints = VSAND.Helper.PowerPoints.Calculate(oTeam.TeamId, oGameHistory)
                        // -- Deprecated 2018108 Per C. Faytok - no longer required by any sports
                        // TieBreak = VSAND.Helper.PowerPoints.TieBreak(oTeam.TeamId, oGameHistory, FirstGameDate, LastGameDate, oTeam.Sport.PowerPointsDataType)
                        PPCalcDate = DateTime.Now;
                    }
                }
            }
            else
            {
                if (oTeam.GameReportEntries.Any())
                {
                    oGameHistory = new List<VsandGameReport>();
                    foreach (VsandGameReportTeam oGrt in oTeam.GameReportEntries)
                    {
                        if (oGrt.GameReport != null)
                        {
                            if (!oGrt.GameReport.Exhibition)
                            {
                                //oGameHistory.Add(oGrt.GameReport);
                            }
                        }
                    }
                }
                else
                {
                    var oGames = new List<VsandGameReport>(); // VSAND.Helper.GameReport.GetTeamReportHistory(oTeam.TeamId, FirstGameDate, LastGameDate);
                    oGameHistory = oGames.Where(gr => gr.Exhibition == false).ToList();
                }

                VsandPowerPointsConfig oPpConfig = null;

                if (_maxGames > 0)
                {
                    // -- Baseball allows ties, but they are not counted for powerpoints, so if this list is truncated here, 
                    // -- the proper number of games does not always flow through
                    // -- Broke this logic into two steps, where we limit by game date first, 
                    // -- and then if there are NO powerpoints, we apply maxgames to the list,
                    // -- otherwise the gamedate filtered list can be sent to the calcuation
                    oGameHistory = oGameHistory.Where(gr => gr.Deleted == false && gr.Exhibition == false).OrderBy(gr => gr.GameDate).ToList(); // .Take(_maxGames).ToList

                    bool bPP = false;
                    if (oTeam.Sport != null)
                    {
                        if (oTeam.Sport.EnablePowerPoints.HasValue)
                        {
                            bPP = oTeam.Sport.EnablePowerPoints.Value;
                        }

                        if (bPP)
                        {
                            // oPpConfig = VSAND.Helper.PowerPoints.GetPPConfig(oTeam.SportId, oTeam.ScheduleYearId);
                        }
                    }

                    if (!bPP)
                    {
                        // -- Restrict the game history using max games
                        oGameHistory = oGameHistory.Take(_maxGames).ToList();
                    }
                    else
                    {
                        // 20150927 Found this logic reversed, was checking If Not oPpConfig.IncludeTieGames
                        if (oPpConfig.IncludeTieGames)
                        {
                            oGameHistory = oGameHistory.Take(_maxGames).ToList();
                        }
                        else
                        {
                            oGameHistory = oGameHistory.Where(gr => gr.Teams.Any((grt => grt.FinalScore > gr.Teams.OrderBy(grtfs => grtfs.FinalScore).FirstOrDefault().FinalScore))).Take(_maxGames).ToList();
                        }
                    }
                }

                if (oTeam.School != null)
                {
                    // VSAND.Helper.Team.CalculateRecord(oTeam.TeamId, oGameHistory, Wins, Losses, Ties, ConferenceWins, ConferenceLosses, ConferenceTies, DivisionWins, DivisionLosses, DivisionTies, LeagueWins, LeagueLosses, LeagueTies, HomeWins, HomeLosses, HomeTies, RoadWins, RoadLosses, RoadTies, InStateGames, oTeam.School.State, PointsFor, PointsAgainst, "");
                }
                if (Wins > 0 | Losses > 0)
                {
                    WinningPercentage = (Wins / (double)(Wins + Losses));
                }
                if (Wins > 0 | Ties > 0 | Losses > 0)
                {
                    InStatePercentage = (InStateGames / (double)(Wins + Losses + Ties));
                }
                if (oTeam.Sport != null)
                {
                    if (oTeam.Sport.EnablePowerPoints.Value)
                    {
                        int iPpMaxGames = _maxGames;
                        DateTime dPpStart = FirstGameDate;
                        DateTime dPpEnd = LastGameDate;

                        if (oPpConfig == null)
                        {
                            // oPpConfig = VSAND.Helper.PowerPoints.GetPPConfig(oTeam.SportId, oTeam.ScheduleYearId);
                        }
                        if (oPpConfig != null)
                        {
                            if (iPpMaxGames == 0)
                            {
                                iPpMaxGames = oPpConfig.IncludeGamesCount;
                            }
                            if (dPpStart == DateHelp.SqlMinDate)
                            {
                                dPpStart = oPpConfig.StartDate;
                            }
                            DateTime dMaxDate = DateHelp.SqlMaxDate;
                            if (dPpEnd.Year == dMaxDate.Year && dPpEnd.Month == dMaxDate.Month && dPpEnd.Day == dMaxDate.Day)
                            {
                                dPpEnd = oPpConfig.EndDate;
                            }
                        }

                        if (iPpMaxGames > 0)
                        {
                            // PowerPoints = VSAND.Helper.PowerPoints.SummarizeCalculate(oTeam.TeamId, oGameHistory, dPpStart, dPpEnd, iPpMaxGames, oTeam.Sport.PowerPointsDataType, PowerPointsGames);
                        }
                        else
                        {
                            // PowerPoints = VSAND.Helper.PowerPoints.SummarizeCalculate(oTeam.TeamId, oGameHistory, dPpStart, dPpEnd, 999, oTeam.Sport.PowerPointsDataType, PowerPointsGames);
                        }

                        // -- Current just running the alternate calculation to cache out the results
                        // VSAND.Helper.PowerPoints.Calculate(oTeam.TeamId, oGameHistory)

                        // Me.PowerPoints = VSAND.Helper.PowerPoints.Calculate(oTeam.TeamId, oGameHistory)
                        // -- Deprecated 2018108 Per C. Faytok - no longer required by any sports
                        // TieBreak = VSAND.Helper.PowerPoints.TieBreak(oTeam.TeamId, oGameHistory, dPpStart, dPpEnd, oTeam.Sport.PowerPointsDataType)
                        PPCalcDate = DateTime.Now;
                    }
                    if (oTeam.Sport.EnableDifferential.Value)
                    {
                        // Differential = VSAND.Helper.PowerPoints.Calculate(oTeam.TeamId, oGameHistory, FirstGameDate, LastGameDate, oTeam.Sport.DifferentialDataType);
                    }
                }
                VsandGameReport oLastGame = oGameHistory.OrderByDescending(gr => gr.GameDate).FirstOrDefault(gr => gr.Deleted == false && gr.Exhibition == false);
                if (oLastGame != null)
                {
                    LastReport = oLastGame.GameDate;
                }
            }
        }

        private int SetCachedValue(List<VsandTeamCustomCode> oCC, string codeName)
        {
            int iRet = 0;
            VsandTeamCustomCode oCode = oCC.FirstOrDefault(cc => cc.CodeName.Equals(codeName, StringComparison.OrdinalIgnoreCase));
            if (oCode != null)
            {
                int.TryParse(oCode.CodeValue, out iRet);
            }
            return iRet;
        }

        public Dictionary<string, int> ToDictionary()
        {
            Dictionary<string, int> oRet = new Dictionary<string, int>
            {
                { "Record-Overall-Wins", Wins },
                { "Record-Overall-Losses", Losses },
                { "Record-Overall-Ties", Ties },
                { "Record-Conference-Wins", ConferenceWins },
                { "Record-Conference-Losses", ConferenceLosses },
                { "Record-Conference-Ties", ConferenceTies },
                { "Record-Division-Wins", DivisionWins },
                { "Record-Division-Losses", DivisionLosses },
                { "Record-Division-Ties", DivisionTies },
                { "Record-Home-Wins", HomeWins },
                { "Record-Home-Losses", HomeLosses },
                { "Record-Home-Ties", HomeTies },
                { "Record-Road-Wins", RoadWins },
                { "Record-Road-Losses", RoadLosses },
                { "Record-Road-Ties", RoadTies },
                { "Record-League-Wins", LeagueWins },
                { "Record-League-Losses", LeagueLosses },
                { "Record-League-Ties", LeagueTies },
                { "PointsFor", (int)PointsFor },
                { "PointsAgainst", (int)PointsAgainst }
            };

            return oRet;
        }

        public int CompareTo(TeamRecordInfo other)
        {
            if (TeamId == other.TeamId)
            {
                return 0;
            }

            // -- The simplest comparison, upr to upr
            int iRet = UnitedPowerRank.CompareTo(other.UnitedPowerRank);
            if (!iRet.Equals(0))
            {
                return iRet;
            }

            // -- Tie-Break 1: Did they play each other head to head?
            var oHeadToHead = oGameHistory.FirstOrDefault(gr => gr.Teams.Any(grt => grt.Team.TeamId == other.TeamId)
                                                                 && !gr.Teams.FirstOrDefault().FinalScore.Equals(gr.Teams.LastOrDefault().FinalScore));
            if (oHeadToHead != null)
            {
                // -- There is a clear winner here
                if (oHeadToHead.Teams.OrderByDescending(grt => grt.FinalScore).FirstOrDefault().Team.TeamId == TeamId)
                {
                    iRet = -1;
                }
                else
                {
                    iRet = 1;
                }
                return iRet;
            }

            // -- Tie-Break 2: Compare records based on common opponents
            TeamRecordInfo oTeam = new TeamRecordInfo();
            TeamRecordInfo oOtherTeam = new TeamRecordInfo();

            foreach (var oGr in oGameHistory)
            {
                var oppId = oGr.Teams.FirstOrDefault(grt => grt.Team.TeamId != TeamId).Team.TeamId;

                var oOppGr = other.oGameHistory.FirstOrDefault(gr => gr.Teams.Any(grt => grt.Team.TeamId == oppId));
                if (oOppGr != null)
                {
                    // -- They have this opponent in common

                    // -- Get the team 1 result
                    var oTeam1Results = oGr.Teams.OrderByDescending(grt => grt.FinalScore).ToList();
                    if (oTeam1Results.FirstOrDefault().FinalScore.Equals(oTeam1Results.LastOrDefault().FinalScore))
                    {
                        // -- Tie
                        oTeam.Ties += 1;
                    }
                    else if (oTeam1Results.FirstOrDefault().Team.TeamId == TeamId)
                    {
                        oTeam.Wins += 1;
                    }
                    else
                    {
                        oTeam.Losses += 1;
                    }

                    // -- Get the team 2 result
                    var oTeam2Results = oOppGr.Teams.OrderByDescending(grt => grt.FinalScore).ToList();
                    if (oTeam2Results.FirstOrDefault().FinalScore.Equals(oTeam2Results.LastOrDefault().FinalScore))
                    {
                        // -- Tie
                        oOtherTeam.Ties += 1;
                    }
                    else if (oTeam2Results.FirstOrDefault().Team.TeamId == other.TeamId)
                    {
                        oOtherTeam.Wins += 1;
                    }
                    else
                    {
                        oOtherTeam.Losses += 1;
                    }
                }
            }

            if (oTeam.Wins > oOtherTeam.Wins || oTeam.Ties > oOtherTeam.Ties || oTeam.Losses < oOtherTeam.Losses)
            {
                iRet = -1;
            }
            else if (oOtherTeam.Wins > oTeam.Wins || oOtherTeam.Ties > oTeam.Ties || oOtherTeam.Losses < oTeam.Losses)
            {
                iRet = 1;
            }

            if (!iRet.Equals(0))
            {
                return iRet;
            }

            // -- Tie-Break 3: Born Rank
            iRet = BornPowerIndex.CompareTo(other.BornPowerIndex);
            if (!iRet.Equals(0))
            {
                return iRet * -1;
            }

            // -- Tie-Break 4: Power points average
            iRet = PowerPointsAverage.CompareTo(other.PowerPointsAverage) * -1;
            return iRet;
        }
    }
}
