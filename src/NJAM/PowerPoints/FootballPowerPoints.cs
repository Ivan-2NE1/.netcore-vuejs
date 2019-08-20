using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VSAND.Common;
using VSAND.Data.Entities;
using VSAND.Data.Repositories;
using VSAND.Data.ViewModels;
using VSAND.Interfaces;

namespace NJAM.PowerPoints
{
    public class FootballPowerPoints : IType, IPowerPoints, IConvertible
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public int TeamId { get; set; } = 0;
        public List<VsandGameReport> GameReports { get; set; }

        private DateTime _StartDate = DateHelp.SqlMinDate;
        public DateTime StartDate
        {
            get {
                return new DateTime(_StartDate.Year, _StartDate.Month, _StartDate.Day, 0, 0, 0);
            }
            set {
                _StartDate = value;
            }
        }

        private DateTime _EndDate = DateHelp.SqlMaxDate;
        public DateTime EndDate
        {
            get {
                return new DateTime(_EndDate.Year, _EndDate.Month, _EndDate.Day, 23, 59, 59);
            }
            set {
                _EndDate = value;
            }
        }

        public int IncludeGames { get; set; } = 8;
        public int EligibleGamesCount { get; set; } = 0;
        public double PowerPoints { get; set; } = 0;
        public int ResidualGameCount { get; set; }

        private readonly IUnitOfWork _uow;

        public FootballPowerPoints(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public FootballPowerPoints(IUnitOfWork uow, int TeamId, List<VsandGameReport> oGameReports, DateTime dStart, DateTime dEnd)
        {
            this._uow = uow;
            this.TeamId = TeamId;
            this.GameReports = oGameReports;
            this.StartDate = dStart;
            this.EndDate = dEnd;
        }

        public FootballPowerPoints(IUnitOfWork uow, int TeamId, List<VsandGameReport> oGameReports, DateTime dStart, DateTime dEnd, int maxGames)
        {
            this._uow = uow;
            this.TeamId = TeamId;
            this.GameReports = oGameReports;
            this.StartDate = dStart;
            this.EndDate = dEnd;
            this.IncludeGames = maxGames;
        }

        public string InputMask()
        {
            return "";
        }

        public decimal ToDecimal()
        {
            double dPowerPoints = 0;
            int iIncludeGames = this.IncludeGames;

            // C. Power Point Calculation Procedure: 
            // 1. Quality Points - Each school will receive the following "Quality Points" for a win or tie from the first eight games: 
            // Win = 6 points; Tie = 3 points; Lose = 0 points 

            // 2. Group Points - Each school will receive "Group Points" from a team they defeated or tied from the first eight games: 
            // Group IV = 4 points 
            // Group III = 3 points 
            // Group II = 2 points 
            // Group I = 1 points 
            // Note: Once the "Football Group Classification" is determined, 
            // there will be no adjustments to this classification when playing teams 
            // (public or non-public) from other sections. When playing teams from out 
            // of state, the group size will be determined based on the enrollment range 
            // of the winning teams section. 

            // 3. Residual Points - Each team will receive residual points from one of the categories below based on the result of the game. 
            // A. Each school will receive "Residual Points" from a team they defeated. For each win or tie your opponent has from the first eight games, you will receive the following (see examples 1 && 2): 
            // Win = 3 points; Tie = 1.5 points; Lose = 0 points 
            // B. Each school will receive "Residual Points" from a team they tied. For each win or tie your opponent has from the first eight games, you will receive the following (see examples 3 && 4): 
            // Win = 1.5 points; Tie = .75 points (See note below); Lose =0 points 
            // Note: No points will be rewarded when calculating "Residual Points" when your game is the opponent's only tie, since you already have been awarded points for that tie. 
            // C. Each school will receive "Residual Points" from a team they lost to. For each win your opponent has that defeated you (not including your game) from the first eight games, you will receive the following (see example 5): 
            // Win = 1 point; Tie = 0 points; Lose = 0 points 

            // Example(1(win))
            // Team A defeated Team B (Group IV) 
            // Team A record 6-2 
            // Team B record 7-1 5 
            // Team A would earn a total of 31 points for the Team B win. (6 points for the win, 4 group points, 21 residual points) 

            // Example(2(win))
            // Team A defeated Team B (Group IV) 
            // Team A record 6-2 
            // Team B record 6-1-1 
            // Team A would earn a total of 29.5 points for the Team B win. (6 points for the win, 4 group points, 19.5 residual points) 

            // Example(3(tie))
            // Team A ties Team B (Group IV) 
            // Team A record 6-1-1 
            // Team B record 5-2-1 
            // Team A would earn a total of 14.5 points for the Team B tie. (3 points for the tie, 4 group points, 7.5 residual points) 

            // Example(4(tie))
            // Team A ties Team B (Group IV) 
            // Team A record 6-1-1 
            // Team B record 5-1-2 
            // Team A would earn a total of 15.25 points for the Team B tie. (3 points for the tie, 4 group points, 8.25 residual points) 

            // Example(5(loss))
            // Non-Public (III) Team A defeated Public Team B 
            // Team B record 6-2 
            // Team A record 7-1 
            // Team B would earn a total of 6 points from the Team A defeat. (6 residual points)

            List<PowerPointsGame> oPowerpointsGames = new List<PowerPointsGame>();

            List<BonusPoints> oBonusPoints = new List<BonusPoints>
            {
                new BonusPoints(12, "NJSFC", "United Red", 2),
                new BonusPoints(12, "NJSFC", "United White", (double)1.5),
                new BonusPoints(13, "NJSFC", "United Red", 1),
                new BonusPoints(13, "NJSFC", "United White", 1)
            };

            // -- Bonus points are different now for SY 13
            bool bTeamUnitedRed = false;
            bool bTeamUnitedWhite = false;
            bool bOppUnitedRed = false;
            bool bOppUnitedWhite = false;

            int scheduleYearId = 0;

            List<VsandGameReport> oEligibleGames = this.GameReports
                .Where(gr => gr.Deleted == false && gr.Final == true && gr.PPEligible == true && gr.Teams.Count == 2)
                .OrderBy(gt => gt.GameDate)
                .Take(iIncludeGames)
                .ToList();

            double dLowPpVal = 999;
            double dRunQuality = 0;
            double dRunGroup = 0;
            double dRunResidual = 0;

            EligibleGamesCount = oEligibleGames.Count;

            int iGameCount = 0;
            foreach (VsandGameReport oGR in oEligibleGames)
            {
                VsandGameReport oGameReport = oGR;
                if (scheduleYearId == 0)
                {
                    scheduleYearId = oGR.ScheduleYear.ScheduleYearId;

                    if (scheduleYearId < 14)
                    {
                        ResidualGameCount = IncludeGames;
                    }
                    else
                    {
                        ResidualGameCount = 7;
                    }
                }

                var oGameTeams = oGR.Teams;

                iGameCount = iGameCount + 1;

                double iQualityPoints = 0;
                double iGroupPoints = 0;
                double dResidualPoints = 0;

                int iGameReportId = oGameReport.GameReportId;
                VsandGameReportTeam oTeam = oGameTeams.FirstOrDefault(gt => gt.TeamId == TeamId);
                double dTeamScore = oTeam.FinalScore;

                string sTeamConf = "";
                var oTeamConf = oTeam.Team.CustomCodes.FirstOrDefault(tc => tc.CodeName.Equals("Conference", StringComparison.OrdinalIgnoreCase));
                if (oTeamConf != null)
                {
                    sTeamConf = oTeamConf.CodeValue;
                }

                string sTeamMultiplier = "";
                var oTeamMultiplier = oTeam.Team.CustomCodes.FirstOrDefault(tc => tc.CodeName.Equals("Non-Public Multiplier", StringComparison.OrdinalIgnoreCase));
                if (oTeamMultiplier != null)
                {
                    sTeamMultiplier = oTeamMultiplier.CodeValue.Trim();
                }

                string sTeamDivision = "";
                var oTeamDivision = oTeam.Team.CustomCodes.FirstOrDefault(tc => tc.CodeName.Equals("Division", StringComparison.OrdinalIgnoreCase));
                if (oTeamDivision != null)
                {
                    sTeamDivision = oTeamDivision.CodeValue;
                }

                bTeamUnitedRed = oBonusPoints.Any(bp => bp.ScheduleYearId == oGameReport.ScheduleYearId && bp.Conference.Equals(sTeamConf, StringComparison.OrdinalIgnoreCase) && bp.Division.Equals(sTeamDivision, StringComparison.OrdinalIgnoreCase) && bp.Division.Contains("Red"));
                bTeamUnitedWhite = oBonusPoints.Any(bp => bp.ScheduleYearId == oGameReport.ScheduleYearId && bp.Conference.Equals(sTeamConf, StringComparison.OrdinalIgnoreCase) && bp.Division.Equals(sTeamDivision, StringComparison.OrdinalIgnoreCase) && bp.Division.Contains("White"));

                VsandGameReportTeam oOpp = oGameTeams.FirstOrDefault(gt => gt.TeamId != TeamId);
                double dOppScore = oOpp.FinalScore;

                bool bTeamforfeit = oTeam.Forfeit;

                int iOppTeamId = oOpp.TeamId;
                string sOppGroup = "";
                int iGroupVal = 0;
                int iOppWins = 0;
                int iOppLosses = 0;
                int iOppTies = 0;
                bool bOutOfState = false;
                string sOppName = "";
                bool bPrivate = false;
                bool bOppPrivate = false;

                int iOppRecordWins = 0;
                int iOppRecordLosses = 0;
                int iOppRecordTies = 0;

                bool bVs9th = true;

                bool bBonusTeam = false;
                double dBonusMultiplier = 1;

                // string sTeamSection = "";
                if (oTeam.Team.School.PrivateSchool.HasValue && oTeam.Team.School.PrivateSchool.Value)
                {
                    bPrivate = true;
                }

                string sBonusCategory = "";
                double dBonusWinValue = 0;
                double dBonusTieValue = 0;
                double dBonusLossValue = 0;
                string sBonusCategoryAbbr = "";

                if (oOpp.Team != null)
                {
                    sOppName = oOpp.Team.Name;
                    if (oOpp.Team.School.State != "NJ")
                    {
                        bOutOfState = true;
                    }
                    else if (oOpp.Team.School.PrivateSchool.HasValue && oOpp.Team.School.PrivateSchool.Value)
                    {
                        bOppPrivate = true;
                    }

                    string sOppConf = "";
                    var oOppConf = oOpp.Team.CustomCodes.FirstOrDefault(tc => tc.CodeName.Equals("Conference", StringComparison.OrdinalIgnoreCase));
                    if (oOppConf != null)
                    {
                        sOppConf = oOppConf.CodeValue;
                    }

                    string sOppDivision = "";
                    var oOppDivision = oOpp.Team.CustomCodes.FirstOrDefault(tc => tc.CodeName.Equals("Division", StringComparison.OrdinalIgnoreCase));
                    if (oOppDivision != null)
                    {
                        sOppDivision = oOppDivision.CodeValue;
                    }

                    if (scheduleYearId <= 13)
                    {
                        var oBonus = oBonusPoints.FirstOrDefault(bp => bp.ScheduleYearId == oTeam.Team.ScheduleYear.ScheduleYearId && bp.Conference.Equals(sOppConf, StringComparison.OrdinalIgnoreCase) && bp.Division.Equals(sOppDivision, StringComparison.OrdinalIgnoreCase));
                        if (oBonus != null)
                        {
                            bBonusTeam = true;
                            dBonusMultiplier = oBonus.Multiplier;
                        }
                    }

                    // -- 20181008 Per Chris Faytok this multiplier value can be applied for any non-A/B/C team vs an A/B/C team
                    // -- even though the regs say that it is for Public teams only
                    // -- 20181022 Turns out that was wrong and non-public don't ever get the multipler
                    if (scheduleYearId >= 14 && !bPrivate)
                    {
                        var oMultiplier = oOpp.Team.CustomCodes.FirstOrDefault(tc => tc.CodeName.Equals("Non-Public Multiplier", StringComparison.OrdinalIgnoreCase));
                        if (oMultiplier != null)
                        {
                            bBonusTeam = true;
                            dBonusMultiplier = 1;
                            sBonusCategory = oMultiplier.CodeValue.Trim();

                            switch (sBonusCategory.ToLower())
                            {
                                case "category a":
                                    {
                                        sBonusCategoryAbbr = "NP-A";
                                        dBonusWinValue = 54;
                                        dBonusTieValue = 45;
                                        dBonusLossValue = 36;
                                        break;
                                    }

                                case "category b":
                                    {
                                        sBonusCategoryAbbr = "NP-B";
                                        dBonusWinValue = 48;
                                        dBonusTieValue = 40;
                                        dBonusLossValue = 32;
                                        break;
                                    }

                                case "category c":
                                    {
                                        sBonusCategoryAbbr = "NP-C";
                                        dBonusWinValue = 42;
                                        dBonusTieValue = 35;
                                        dBonusLossValue = 28;
                                        break;
                                    }

                                default:
                                    {
                                        bBonusTeam = false;
                                        break;
                                    }
                            }
                        }
                    }

                    bOppUnitedRed = oBonusPoints.Any(bp => bp.ScheduleYearId == oGameReport.ScheduleYearId && bp.Conference.Equals(sOppConf, StringComparison.OrdinalIgnoreCase) && bp.Division.Equals(sOppDivision, StringComparison.OrdinalIgnoreCase) && bp.Division.Contains("Red"));
                    bOppUnitedWhite = oBonusPoints.Any(bp => bp.ScheduleYearId == oGameReport.ScheduleYearId && bp.Conference.Equals(sOppConf, StringComparison.OrdinalIgnoreCase) && bp.Division.Equals(sOppDivision, StringComparison.OrdinalIgnoreCase) && bp.Division.Contains("White"));

                    VsandTeamCustomCode oOppGroup = null/* TODO Change to default(_) if this is not a reference type */;
                    if (!bOutOfState)
                    {
                        // -- Use their OOS designated record (if not 0-0-0)
                        VsandTeamCustomCode oOppWinEx = oOpp.Team.CustomCodes.FirstOrDefault(gt => gt.CodeName == "OOSFinalWins");
                        if (oOppWinEx != null)
                        {
                            string sOppWins = oOppWinEx.CodeValue;
                            int.TryParse(sOppWins, out iOppWins);
                        }
                        VsandTeamCustomCode oOppLoseEx = oOpp.Team.CustomCodes.FirstOrDefault(gt => gt.CodeName == "OOSFinalLosses");
                        if (oOppLoseEx != null)
                        {
                            string sOppLosses = oOppLoseEx.CodeValue;
                            int.TryParse(sOppLosses, out iOppLosses);
                        }
                        VsandTeamCustomCode oOppTieEx = oOpp.Team.CustomCodes.FirstOrDefault(gt => gt.CodeName == "OOSFinalTies");
                        if (oOppTieEx != null)
                        {
                            string sOppTies = oOppTieEx.CodeValue;
                            int.TryParse(sOppTies, out iOppTies);
                        }

                        iOppRecordWins = iOppWins;
                        iOppRecordLosses = iOppLosses;
                        iOppRecordTies = iOppTies;

                        if (iOppWins > 0 || iOppLosses > 0 || iOppTies > 0)
                        {
                            bOutOfState = true;

                            // -- For 2018-19 Season, we are no longer off-setting the records based on these values
                            if (oTeam.Team.ScheduleYear.ScheduleYearId < 14)
                            {
                                int iOppRecordWinOffset = 0;
                                int iOppRecordLossOffset = 0;
                                int iOppRecordTieOffset = 0;
                                List<VsandGameReport> oVsGames = oEligibleGames.Where(gr => gr.Teams.Any((grt => grt.TeamId == iOppTeamId))).ToList();
                                foreach (VsandGameReport oVsGame in oVsGames)
                                {
                                    VsandGameReportTeam oVsTeam = oVsGame.Teams.FirstOrDefault(grt => grt.TeamId == this.TeamId);
                                    VsandGameReportTeam oVsOpp = oVsGame.Teams.FirstOrDefault(grt => grt.TeamId == iOppTeamId);

                                    if (oVsOpp.FinalScore > oVsTeam.FinalScore)
                                    {
                                        iOppRecordWinOffset = iOppRecordWinOffset - 1;
                                    }
                                    else if (oVsOpp.FinalScore == oVsTeam.FinalScore)
                                    {
                                        iOppRecordTieOffset = iOppRecordTieOffset - 1;
                                    }
                                    else
                                    {
                                        iOppRecordLossOffset = iOppRecordLossOffset - 1;
                                    }
                                }

                                if (dTeamScore == dOppScore)
                                {
                                    iOppTies = iOppTies + iOppRecordTieOffset;
                                }
                                else if (dTeamScore < dOppScore)
                                {
                                    iOppWins = iOppWins + iOppRecordWinOffset;
                                }
                            }
                        }

                        oOppGroup = oOpp.Team.CustomCodes.FirstOrDefault(cc => cc.CodeName.Equals("GroupExchange", System.StringComparison.OrdinalIgnoreCase));

                        if (oOppGroup == null || (bPrivate && bOppPrivate && !bOutOfState))
                        {
                            oOppGroup = oOpp.Team.CustomCodes.FirstOrDefault(gt => gt.CodeName.Equals("Group", System.StringComparison.OrdinalIgnoreCase));
                        }

                        if (oOppGroup != null)
                        {
                            sOppGroup = oOppGroup.CodeValue.ToLower();
                        }
                    }
                    else
                    {
                        oOppGroup = oOpp.Team.CustomCodes.FirstOrDefault(cc => cc.CodeName.Equals("GroupExchange", System.StringComparison.OrdinalIgnoreCase));
                        if (oOppGroup != null)
                        {
                            sOppGroup = oOppGroup.CodeValue;
                        }

                        VsandTeamCustomCode oOppWinEx = oOpp.Team.CustomCodes.FirstOrDefault(gt => gt.CodeName == "OOSFinalWins");
                        if (oOppWinEx != null)
                        {
                            string sOppWins = oOppWinEx.CodeValue;
                            int.TryParse(sOppWins, out iOppWins);
                        }
                        VsandTeamCustomCode oOppLoseEx = oOpp.Team.CustomCodes.FirstOrDefault(gt => gt.CodeName == "OOSFinalLosses");
                        if (oOppLoseEx != null)
                        {
                            string sOppLosses = oOppLoseEx.CodeValue;
                            int.TryParse(sOppLosses, out iOppLosses);
                        }
                        VsandTeamCustomCode oOppTieEx = oOpp.Team.CustomCodes.FirstOrDefault(gt => gt.CodeName == "OOSFinalTies");
                        if (oOppTieEx != null)
                        {
                            string sOppTies = oOppTieEx.CodeValue;
                            int.TryParse(sOppTies, out iOppTies);
                        }

                        if ((iOppWins + iOppTies + iOppLosses) <= IncludeGames)
                        {
                            if (oTeam.Team.ScheduleYear.ScheduleYearId < 14)
                            {
                                if (dTeamScore == dOppScore)
                                {
                                    iOppTies = iOppTies - 1;
                                }
                                else if (dTeamScore < dOppScore)
                                {
                                    iOppWins = iOppWins - 1;
                                }

                                if (iOppWins < 0)
                                {
                                    iOppWins = 0;
                                }
                            }
                        }
                        else
                        {
                            iOppWins = 0;
                            iOppTies = 0;
                            iOppLosses = 0;
                        }
                    }

                    string sGroup = sOppGroup.ToLower().Replace("group", "").Trim();
                    int.TryParse(sGroup, out iGroupVal);

                    if (!bOutOfState)
                    {
                        iOppRecordWins = 0;
                        iOppRecordTies = 0;
                        iOppRecordLosses = 0;

                        List<VsandGameReport> oOppGameReports = null;

                        IEnumerable<VsandGameReport> oData = _uow.GameReports.List(
                                                                  g => g.Teams.Any(gt => gt.Team.TeamId == iOppTeamId) && g.Deleted == false && g.Final == true && g.PPEligible == true && g.GameDate >= this.StartDate && g.GameDate <= this.EndDate,
                                                                  x => x.OrderBy(g => g.GameDate),
                                                                  new List<string> { "Teams" }
                                                              ).Result.Take(this.IncludeGames);
                        if (oData != null)
                        {
                            oOppGameReports = oData.ToList();
                        }

                        int iOppGameCount = 0;

                        iOppRecordWins = oOppGameReports.Where(gr => gr.Teams.OrderByDescending(grt => grt.FinalScore).FirstOrDefault().TeamId == iOppTeamId).Count();
                        iOppRecordLosses = oOppGameReports.Where(gr => gr.Teams.OrderBy(grt => grt.FinalScore).FirstOrDefault().TeamId == iOppTeamId).Count();
                        iOppRecordTies = oOppGameReports.Where(gr => gr.Teams.Select(grt => grt.FinalScore).Distinct().Count() == 1).Count();

                        foreach (VsandGameReport oOppGR in oOppGameReports)
                        {
                            iOppGameCount += 1;

                            if (iOppGameCount > ResidualGameCount && scheduleYearId >= 14)
                            {
                                break;
                            }

                            var oOppGameTeams = oOppGR.Teams;
                            VsandGameReportTeam oOppTeam = oOppGameTeams.FirstOrDefault(gt => gt.TeamId == iOppTeamId);
                            double dOppTeamScore = oOppTeam.FinalScore;

                            VsandGameReportTeam oOppOpp = oOppGameTeams.FirstOrDefault(gt => gt.TeamId != iOppTeamId);

                            if (oTeam.Team.ScheduleYear.ScheduleYearId >= 14 || oOppOpp.TeamId != this.TeamId)
                            {
                                double dOppOppScore = oOppOpp.FinalScore;

                                if (dOppTeamScore > dOppOppScore)
                                {
                                    iOppWins = iOppWins + 1;
                                }
                                else if (dOppOppScore > dOppTeamScore)
                                {
                                    if (oOppGR.GameReportId != iGameReportId)
                                    {
                                        iOppLosses = iOppLosses + 1;
                                    }
                                }
                                else if (oOppGR.GameReportId != iGameReportId)
                                {
                                    iOppTies = iOppTies + 1;
                                }
                            }

                            if (oOppGR.GameReportId == iGameReportId)
                            {
                                bVs9th = false;
                            }
                        }
                    }
                    else
                    {
                        // -- We can't apply this rule for OOS
                        bVs9th = false;
                    }
                }

                string sResultOutcome = "";
                // string sResultAnnotation = "";
                string sResult = "";

                string sForfeit = bTeamforfeit ? "*" : " ";
                string s9th = bVs9th ? "9" : " ";
                string sBonus = bBonusTeam ? "x" + dBonusMultiplier.ToString("0.#") : " ";
                if (scheduleYearId >= 14)
                {
                    sBonus = bBonusTeam ? sBonusCategoryAbbr : " ";
                }

                if (bBonusTeam && oGameReport.ScheduleYear.ScheduleYearId == 13)
                {
                    // -- Clear out the bonus indicator, might use something else later
                    sBonus = "";
                    bBonusTeam = false;
                }

                // -- Need to apply Bonus points if this is a Red / White Non-Public Opponent
                if (dTeamScore > dOppScore || (bBonusTeam && scheduleYearId == 13))
                {
                    iQualityPoints = 6;
                    iGroupPoints = iGroupVal;
                    dResidualPoints = (iOppWins * 3) + (iOppTies * 1.5);

                    if (bBonusTeam)
                    {
                        iQualityPoints = iQualityPoints * dBonusMultiplier;
                        iGroupPoints = iGroupPoints * dBonusMultiplier;
                        dResidualPoints = dResidualPoints * dBonusMultiplier;
                    }

                    if (oGameReport.ScheduleYear.ScheduleYearId < 13)
                    {
                        if (bVs9th)
                        {
                            dResidualPoints = dResidualPoints - 3;
                        }
                    }
                    else if (oGameReport.ScheduleYear.ScheduleYearId < 14)
                    {
                        if (bVs9th && iOppWins == 8)
                        {
                            dResidualPoints = dResidualPoints - 3;
                        }
                    }
                    sResultOutcome = "W";
                    sResult = "W" + sForfeit + sBonus + s9th + dTeamScore + "-" + dOppScore;
                }
                else if (dTeamScore < dOppScore)
                {
                    iQualityPoints = 0;
                    iGroupPoints = 0;
                    dResidualPoints = (iOppWins * 1);

                    if (bVs9th && oGameReport.ScheduleYear.ScheduleYearId < 14)
                    {
                        dResidualPoints = dResidualPoints - 1;
                    }
                    sResultOutcome = "L";
                    sResult = "L" + sForfeit + sBonus + s9th + dOppScore + "-" + dTeamScore;
                }
                else
                {
                    iQualityPoints = 3;
                    iGroupPoints = iGroupVal;
                    dResidualPoints = (iOppWins * 1.5) + (iOppTies * 0.75);

                    if (oGameReport.ScheduleYear.ScheduleYearId < 13)
                    {
                        if (bVs9th)
                        {
                            dResidualPoints = dResidualPoints - 1.5;
                        }
                    }
                    else
                    {
                        // No deduction for tie starting in SY13
                    }

                    sResultOutcome = "T";
                    sResult = "T" + sForfeit + sBonus + s9th + dTeamScore + "-" + dOppScore;
                }

                // -- Do not allow negative residuals
                if (dResidualPoints < 0)
                {
                    dResidualPoints = 0;
                }

                if (scheduleYearId <= 13 && bTeamforfeit)
                {
                    iQualityPoints = 0;
                    iGroupPoints = 0;
                    dResidualPoints = 0;
                }

                // -- 20181015 Chris says this is wrong. That teams are just limited to residuals from their opponents first 7 games.
                // '-- Only allow residuals for up to "N" games
                // If iGameCount > ResidualGameCount Then
                // dResidualPoints = 0
                // End If

                // -- 20181028 Per Chris Faytok, residuals are capped at 18 points (not in regs)
                if (scheduleYearId >= 14 && dResidualPoints > 18)
                {
                    dResidualPoints = 18;
                }

                double dPPVal = iQualityPoints + iGroupPoints + dResidualPoints;
                double dAbcPoints = 0;
                // bool bAbcPoints = false;

                if (scheduleYearId == 13)
                {
                    if ((bTeamUnitedRed || bTeamUnitedWhite || bOppUnitedRed || bOppUnitedWhite) && !bTeamforfeit)
                    {
                        bool bResetCalculatedPoints = true;
                        // -- New NJSFC Power Points Adjustment from SY 17 (2017-18) onward
                        if ((bTeamUnitedRed && bOppUnitedRed) || (bTeamUnitedWhite && bOppUnitedWhite))
                        {
                            if (dTeamScore > dOppScore)
                            {
                                dPPVal = 32;
                            }
                            else if (dTeamScore < dOppScore)
                            {
                                dPPVal = 16;
                            }
                            else
                            {
                                dPPVal = 24;
                            }
                        }
                        else if (bTeamUnitedRed && bOppUnitedWhite)
                        {
                            if (dTeamScore > dOppScore)
                            {
                                // dPPVal = dPPVal; // United Red gets Natural Power Points for a win over United White
                                bResetCalculatedPoints = false;
                            }
                            else if (dTeamScore < dOppScore)
                            {
                                if (dPPVal < 12)
                                {
                                    dPPVal = 12; // United Red gets Natural Power Points for a loss or 12 (whichever is greater) against United White
                                    bResetCalculatedPoints = false;
                                }
                            }
                            else
                            {
                                // dPPVal = dPPVal; // United Red gets Natural Power Points for a tie with United White
                                bResetCalculatedPoints = false;
                            }
                        }
                        else if (bTeamUnitedWhite && bOppUnitedRed)
                        {
                            if (dTeamScore > dOppScore)
                            {
                                dPPVal = 36;
                            }
                            else if (dTeamScore < dOppScore)
                            {
                                dPPVal = 24;
                            }
                            else
                            {
                                dPPVal = 30;
                            }
                        }
                        else if ((bTeamUnitedWhite || bTeamUnitedRed) && bOutOfState)
                        {
                            if (dTeamScore > dOppScore)
                            {
                                dPPVal = 32;
                            }
                            else if (dTeamScore < dOppScore)
                            {
                                dPPVal = 16;
                            }
                            else
                            {
                                dPPVal = 24;
                            }
                        }
                        else if ((!bTeamUnitedRed && !bTeamUnitedWhite) && (bOppUnitedRed))
                        {
                            if (dTeamScore > dOppScore)
                            {
                                dPPVal = 54;
                            }
                            else if (dTeamScore < dOppScore)
                            {
                                dPPVal = 36;
                            }
                            else
                            {
                                dPPVal = 45;
                            }
                        }
                        else if ((!bTeamUnitedWhite && !bTeamUnitedWhite) && (bOppUnitedWhite))
                        {
                            if (dTeamScore > dOppScore)
                            {
                                dPPVal = 48;
                            }
                            else if (dTeamScore < dOppScore)
                            {
                                dPPVal = 32;
                            }
                            else
                            {
                                dPPVal = 40;
                            }
                        }
                        else
                        {
                            bResetCalculatedPoints = false;
                        }

                        if (bResetCalculatedPoints)
                        {
                            iQualityPoints = 0;
                            iGroupPoints = 0;
                            dResidualPoints = 0;
                        }
                    }
                }
                else if (scheduleYearId == 14 && bBonusTeam)
                {
                    // bAbcPoints = true;
                    if (dTeamScore > dOppScore)
                    {
                        dAbcPoints = dBonusWinValue;
                    }
                    else if (dTeamScore < dOppScore)
                    {
                        dAbcPoints = dBonusLossValue;
                    }
                    else
                    {
                        dAbcPoints = dBonusTieValue;
                    }
                }

                dRunQuality = dRunQuality + iQualityPoints;
                dRunGroup = dRunGroup + iGroupPoints;
                dRunResidual = dRunResidual + dResidualPoints;

                if (!bTeamforfeit)
                {
                    if (dPPVal < dLowPpVal)
                    {
                        dLowPpVal = dPPVal;
                    }
                }

                PowerPointsGame oPpGame = new PowerPointsGame()
                {
                    GameDate = oGR.GameDate,
                    OpponentName = sOppName,
                    OppRecordWins = iOppRecordWins,
                    OppRecordLosses = iOppRecordLosses,
                    OppRecordTies = iOppRecordTies,
                    Result = sResultOutcome,
                    ResultAnnotation = sForfeit + sBonus + s9th,
                    TeamScore = (int)dTeamScore,
                    OppScore = (int)dOppScore,
                    QualityPoints = iQualityPoints,
                    GroupPoints = iGroupPoints,
                    ResidualPoints = dResidualPoints,
                    AbcPoints = dAbcPoints,
                    PowerPoints = dPPVal
                };

                oPowerpointsGames.Add(oPpGame);

                dPowerPoints = dPowerPoints + dPPVal; // iQualityPoints + iGroupPoints + dResidualPoints

                string sRec = iOppWins + "-" + iOppLosses + (iOppTies > 0 ? "-" + iOppTies : "");
                if (iOppRecordWins > 0 || iOppRecordLosses > 0 || iOppRecordTies > 0)
                {
                    sRec = iOppRecordWins + "-" + iOppRecordLosses + (iOppRecordTies > 0 ? "-" + iOppRecordTies : "");
                }
            }

            StringBuilder oCalcSb = new StringBuilder();
            oCalcSb.AppendLine("<table width=\"100%\" border=\"0\" cellpadding=\"5\">");
            oCalcSb.AppendLine("<caption>Limit first " + this.IncludeGames + " games" + (this.StartDate > DateHelp.SqlMinDate ? " between " + this.StartDate.ToString("%M/%d/yyyy") + " and " + this.EndDate.ToString("%M/%d/yyyy") : "") + "</caption>");
            oCalcSb.AppendLine("<thead><tr><th>Date</th><th>Opp</th><th>Result</th><th>Quality</th><th>Group</th><th>Residual</th><th>PP</th></tr></thead>");
            oCalcSb.AppendLine("<tbody>");

            if (dLowPpVal == 999 || oEligibleGames.Count < IncludeGames)
            {
                dLowPpVal = 0;
            }

            if (scheduleYearId >= 14)
            {
                // -- Let's sort out our special stuff now
                if (oPowerpointsGames.Any(ppg => ppg.AbcPoints > 0))
                {
                    var oAbcGames = oPowerpointsGames
                                        .Where(ppg => ppg.AbcPoints > 0)
                                        .OrderByDescending(ppg => ppg.AbcPoints)
                                        .ThenBy(ppg => ppg.PowerPoints).ToList();

                    var oAbcGame = oAbcGames.FirstOrDefault();
                    oAbcGame.QualityPoints = 0;
                    oAbcGame.GroupPoints = 0;
                    oAbcGame.ResidualPoints = 0;
                    oAbcGame.PowerPoints = oAbcGame.AbcPoints;
                }
            }

            foreach (var oPpGame in oPowerpointsGames)
            {
                oCalcSb.AppendLine(oPpGame.ToSupportRow());
            }

            dRunQuality = oPowerpointsGames.Sum(ppg => ppg.QualityPoints);
            dRunGroup = oPowerpointsGames.Sum(ppg => ppg.GroupPoints);
            dRunResidual = oPowerpointsGames.Sum(ppg => ppg.ResidualPoints);
            dPowerPoints = oPowerpointsGames.Sum(ppg => ppg.PowerPoints);

            oCalcSb.AppendLine("<tr><td colspan=\"3\" style=\"text-align:right;font-weight:bold;\">Totals:</td><td class=\"center\">" + dRunQuality + "</td><td class=\"center\">" + dRunGroup + "</td><td class=\"center\">" + dRunResidual + "</td><td class=\"center\">" + dPowerPoints + "</td></tr>");
            if (scheduleYearId < 14 && dLowPpVal > 0)
            {
                oCalcSb.AppendLine("<tr><td colspan=\"6\" style=\"text-align:right;font-weight:bold;\">Less Lowest Powerpoint Value:</td><td class=\"center\">-" + dLowPpVal + "</td></tr>");
                oCalcSb.AppendLine("<tr><td colspan=\"6\" style=\"text-align:right;font-weight:bold;\">Final Value:</td><td class=\"center\">" + (dPowerPoints - dLowPpVal) + "</td></tr>");
            }

            if (scheduleYearId >= 14)
            {
                oCalcSb.AppendLine("<tr><td colspan=\"3\" style=\"text-align:right;font-weight:bold;\">Average:</td><td class=\"center\">&nbsp;</td><td class=\"center\">&nbsp;</td><td class=\"center\">&nbsp;</td><td class=\"center\">" + Math.Round(dPowerPoints / (double)oEligibleGames.Count, 2, MidpointRounding.AwayFromZero) + "</td></tr>");
            }

            oCalcSb.AppendLine("</tbody></table>");

            // -- Add residuals message to bottom of table
            if (ResidualGameCount != IncludeGames)
            {
                oCalcSb.AppendLine("<div>Residuals up to opponent's first " + ResidualGameCount + " games</div>");
            }

            try
            {
                // -- save the calculation for reference
                VsandGameReport oRefGame = oEligibleGames.FirstOrDefault();
                if (oRefGame != null)
                {
                    int iSchedYear = oRefGame.ScheduleYearId;
                    int iSport = oRefGame.SportId;
                    string sDir = "appdata/PPCalc/" + iSchedYear + "/" + iSport;
                    Directory.CreateDirectory(sDir);
                    File.WriteAllText(Path.Combine(sDir, TeamId + ".htm"), oCalcSb.ToString());
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
            }

            if (scheduleYearId < 14)
            {
                dPowerPoints = dPowerPoints - dLowPpVal;
            }

            PowerPoints = dPowerPoints;

            return (decimal)dPowerPoints;
        }

        public double TieBreak()
        {
            double dPPTB = 0;

            // -- Per Chris Faytok, this is no longer used, YAY!

            return dPPTB;
        }

        public System.TypeCode GetTypeCode()
        {
            return TypeCode.Object;
        }

        public bool ToBoolean(System.IFormatProvider provider)
        {
            throw new InvalidCastException("NJAM.FootballPowerPoints cannot be converted to Boolean");
        }

        public byte ToByte(System.IFormatProvider provider)
        {
            throw new InvalidCastException("NJAM.FootballPowerPoints cannot be converted to Boolean");
        }

        public char ToChar(System.IFormatProvider provider)
        {
            throw new InvalidCastException("NJAM.FootballPowerPoints cannot be converted to Boolean");
        }

        public DateTime ToDateTime(System.IFormatProvider provider)
        {
            throw new InvalidCastException("NJAM.FootballPowerPoints cannot be converted to Boolean");
        }

        public decimal ToDecimal(System.IFormatProvider provider)
        {
            return this.ToDecimal();
        }

        public double ToDouble(System.IFormatProvider provider)
        {
            return (double)this.ToDecimal();
        }

        public short ToInt16(System.IFormatProvider provider)
        {
            throw new InvalidCastException("NJAM.FootballPowerPoints cannot be converted to Boolean");
        }

        public int ToInt32(System.IFormatProvider provider)
        {
            throw new InvalidCastException("NJAM.FootballPowerPoints cannot be converted to Boolean");
        }

        public long ToInt64(System.IFormatProvider provider)
        {
            throw new InvalidCastException("NJAM.FootballPowerPoints cannot be converted to Boolean");
        }

        public sbyte ToSByte(System.IFormatProvider provider)
        {
            throw new InvalidCastException("NJAM.FootballPowerPoints cannot be converted to Boolean");
        }

        public float ToSingle(System.IFormatProvider provider)
        {
            throw new InvalidCastException("NJAM.FootballPowerPoints cannot be converted to Boolean");
        }

        public string ToString(System.IFormatProvider provider)
        {
            return this.ToDecimal().ToString();
        }

        public object ToType(System.Type conversionType, System.IFormatProvider provider)
        {
            return this.GetType();
        }

        public ushort ToUInt16(System.IFormatProvider provider)
        {
            throw new InvalidCastException("NJAM.FootballPowerPoints cannot be converted to Boolean");
        }

        public uint ToUInt32(System.IFormatProvider provider)
        {
            throw new InvalidCastException("NJAM.FootballPowerPoints cannot be converted to Boolean");
        }

        public ulong ToUInt64(System.IFormatProvider provider)
        {
            throw new InvalidCastException("NJAM.FootballPowerPoints cannot be converted to Boolean");
        }

        protected class BonusPoints
        {
            public int ScheduleYearId { get; set; } = 0;
            public string Conference { get; set; } = "";
            public string Division { get; set; } = "";
            public double Multiplier { get; set; } = 0;

            public BonusPoints()
            {

            }

            public BonusPoints(int scheduleYearId, string conference, string division, double multiplier)
            {
                this.ScheduleYearId = scheduleYearId;
                this.Conference = conference;
                this.Division = division;
                this.Multiplier = multiplier;
            }
        }

        protected class PowerPointsGame
        {
            public DateTime GameDate { get; set; }
            public string OpponentName { get; set; }
            public int OppRecordWins { get; set; }
            public int OppRecordLosses { get; set; }
            public int OppRecordTies { get; set; }
            public string Result { get; set; }
            public string ResultAnnotation { get; set; }
            public int TeamScore { get; set; }
            public int OppScore { get; set; }
            public double QualityPoints { get; set; }
            public double GroupPoints { get; set; }
            public double ResidualPoints { get; set; }
            public double AbcPoints { get; set; } = 0;
            public bool HighAbcPoints { get; set; } = false;
            public double PowerPoints { get; set; }

            public PowerPointsGame()
            {

            }

            private string FormatRecord()
            {
                List<string> oRec = new List<string>();
                oRec.Add(OppRecordWins.ToString());
                oRec.Add(OppRecordLosses.ToString());
                if (OppRecordTies > 0)
                {
                    oRec.Add(OppRecordTies.ToString());
                }
                return string.Join("-", oRec.ToArray());
            }

            private string FormatScore()
            {
                return TeamScore.ToString() + "-" + OppScore.ToString();
            }

            private string FormatAnnotation()
            {
                string sRet = "";
                if (!string.IsNullOrEmpty(ResultAnnotation))
                {
                    sRet = " <span style=\"color:red;\"><sup>" + ResultAnnotation + "</sup></span>";
                }
                return sRet;
            }

            public string ToSupportRow()
            {
                StringBuilder oSb = new StringBuilder();
                oSb.Append("<tr>");
                oSb.Append("<td class=\"center\">" + GameDate.ToString("%M/%d/yy") + "</td>");
                oSb.Append("<td>" + OpponentName + " (" + FormatRecord() + ")</td>");
                oSb.Append("<td>" + Result + FormatAnnotation() + " " + FormatScore() + "</td>");
                oSb.Append("<td class=\"center\">" + QualityPoints.ToString() + "</td>");
                oSb.Append("<td class=\"center\">" + GroupPoints.ToString() + "</td>");
                oSb.Append("<td class=\"center\">" + ResidualPoints.ToString() + "</td>");
                oSb.Append("<td class=\"center\">" + PowerPoints.ToString() + "</td>");
                oSb.Append("</tr>");
                return oSb.ToString();
            }
        }

        public SortedList<string, SortedList<string, List<TeamRecordInfo>>> CreatePlayoffSections(ref List<TeamRecordInfo> oTeamRecords)
        {
            var oGroupNotes = new SortedList<string, List<string>>();
            return CreatePlayoffSections(ref oTeamRecords, ref oGroupNotes);
        }

        public SortedList<string, SortedList<string, List<TeamRecordInfo>>> CreatePlayoffSections(ref List<TeamRecordInfo> oTeamRecords, ref SortedList<string, List<string>> oGroupNotes)
        {
            SortedList<string, SortedList<string, List<TeamRecordInfo>>> oRet = new SortedList<string, SortedList<string, List<TeamRecordInfo>>>();

            // -- Setup our output
            oRet.Add("North 1", new SortedList<string, List<TeamRecordInfo>>());
            oRet.Add("North 2", new SortedList<string, List<TeamRecordInfo>>());
            oRet.Add("Central", new SortedList<string, List<TeamRecordInfo>>());
            oRet.Add("South", new SortedList<string, List<TeamRecordInfo>>());
            oRet.Add("Non-Public", new SortedList<string, List<TeamRecordInfo>>());

            // -- This requires UPR to be calculated (which it should already be)

            // -- Get a list of all of the school northing numbers
            var oNorthingNumbers = _uow.SchoolCustomCodes.List(cc => cc.CodeName == "NorthingNumber").Result;

            // -- Apply the northing numbers everywhere (who knows what will happen!)
            Log.Info("Start Applying Northing Numbers");
            foreach (var oTr in oTeamRecords)
            {
                var oNorthing = oNorthingNumbers.FirstOrDefault(n => n.SchoolId == oTr.SchoolId);
                if (oNorthing != null)
                {
                    int iNorthing = 0;
                    int.TryParse(oNorthing.CodeValue, out iNorthing);
                    oTr.NorthingNumber = iNorthing;
                }
            }
            Log.Info("Finish Applying Northing Numbers");

            List<string> oProcessSections = new List<string>
            {
                "North",
                "South"
            };

            var oFbSections = oProcessSections;
            foreach (string sFbSection in oFbSections)
            {
                string sSection = sFbSection;

                string sTopSectionName = "";
                string sBottomSectionName = "";
                string sTopSectionAbbr = "";
                string sBottomSectionAbbr = "";
                switch (sSection.ToLower())
                {
                    case "north":
                        {
                            sTopSectionName = "North 1";
                            sTopSectionAbbr = "N1";
                            sBottomSectionName = "North 2";
                            sBottomSectionAbbr = "N2";
                            break;
                        }

                    case "south":
                        {
                            sTopSectionName = "Central";
                            sTopSectionAbbr = "C";
                            sBottomSectionName = "South";
                            sBottomSectionAbbr = "S";
                            break;
                        }

                    default:
                        {
                            break;
                        }
                }

                var oFbGroups = (from tr in oTeamRecords
                                 where tr.Section.Equals(sSection, StringComparison.OrdinalIgnoreCase)
                                 select tr.Group).Distinct();
                foreach (string sFbGroup in oFbGroups)
                {
                    string sGroup = sFbGroup;

                    var oAllInGroup = oTeamRecords.Where(tr => tr.Section.Equals(sSection, StringComparison.OrdinalIgnoreCase) && tr.Group.Equals(sGroup, StringComparison.OrdinalIgnoreCase))
                                                  .OrderBy(tr => tr.UnitedPowerRank).ToList();

                    // -- Prime the super section seeding number
                    int sortIdx = 1;
                    oAllInGroup.Sort();
                    foreach (var oTr in oAllInGroup)
                    {
                        oTr.SuperSectionSeed = sortIdx;
                        sortIdx += 1;
                    }

                    string sNotesKey = sSection + sGroup;
                    if (!oGroupNotes.ContainsKey(sNotesKey))
                    {
                        oGroupNotes.Add(sNotesKey, new List<string>());
                    }

                    // -- This should already be solved by having performed the IComarable sort on the full list
                    // '-- 1. If there is a tie for 16th ranked UPR, we got some work to do!
                    bool bHasTieBreak = oAllInGroup.FirstOrDefault(tr => tr.SuperSectionSeed == 16).UnitedPowerRank
                                                   .Equals(oAllInGroup.FirstOrDefault(tr => tr.SuperSectionSeed == 17).UnitedPowerRank);

                    // -- 2. If 16 beat any of the top 15, then it is locked
                    var oTop15TeamIds = (from tr in oAllInGroup
                                         where tr.SuperSectionSeed > 0 && tr.SuperSectionSeed < 16
                                         select tr.TeamId).ToList();
                    var o16Team = (from tr in oAllInGroup
                                   where tr.SuperSectionSeed == 16
                                   select tr).FirstOrDefault();
                    int teamId16 = o16Team.TeamId;
                    bool bLocked = (from oGr in o16Team.oGameHistory
                                    select oGr.Teams.OrderByDescending(grt => grt.FinalScore).ToList())
                                              .Any(oResults => oResults.FirstOrDefault().Team.TeamId == teamId16 && oTop15TeamIds.Contains(oResults.LastOrDefault().Team.TeamId));

                    if (!bLocked)
                    {
                        // -- More craziness ensues
                        oGroupNotes[sNotesKey].Add("Not Locked!! Doesn't look like #16 defeated any #1-#15 :(  Commence with the jumping");
                    }
                    else
                    {
                        oGroupNotes[sNotesKey].Add("Locked. #16 beat someone in the top 15");
                    }

                    // -- But, if #17 beat #16, they can move to the #16 slot (as long as they aren't in #17 because of a #16 tiebreak that we just resolved)
                    if (!bLocked && !bHasTieBreak)
                    {
                        var o17Team = (from tr in oAllInGroup
                                       where tr.SuperSectionSeed == 17
                                       select tr).FirstOrDefault();
                        var teamId17 = o17Team.TeamId;
                        bool b17Jump = (from oGr in o16Team.oGameHistory
                                        select oGr.Teams.OrderByDescending(grt => grt.FinalScore).ToList())
                                .Any(oResults => oResults.FirstOrDefault().Team.TeamId == teamId17);
                        if (b17Jump)
                        {
                            oGroupNotes[sNotesKey].Add("#17 jumped to #16 because it defeated original #16 and original #16 didn't defeat any top 15, and there was no 16 tie-break");
                            // -- Need to flip 16 and 17, since 17 beat 16 and we are not locked
                            o17Team.SuperSectionSeed = 16;
                            o16Team.SuperSectionSeed = 17;
                        }
                        else
                        {
                            oGroupNotes[sNotesKey].Add("No 17 jump eligibility");
                        }
                    }

                    var oBestInGroup = oTeamRecords.Where(tr => tr.Section.Equals(sSection, StringComparison.OrdinalIgnoreCase) && tr.Group.Equals(sGroup, StringComparison.OrdinalIgnoreCase))
                                                   .OrderBy(tr => tr.SuperSectionSeed).Take(16).ToList();
                    var sortIdx16 = 1;
                    foreach (var oTr in oBestInGroup)
                    {
                        oTr.Seed16Rank = sortIdx16;
                        sortIdx16 += 1;
                    }

                    // -- Now, sort the top 16 by Northing Number DESC and split into top and bottom groups
                    var oTopGroup = oBestInGroup.OrderByDescending(tr => tr.NorthingNumber).Take(8).ToList();
                    // -- Need to re-sort these to fix-up any ties
                    oTopGroup.Sort();
                    var sortIdx8 = 1;
                    foreach (var oTr in oTopGroup)
                    {
                        oTr.SuperSection = sTopSectionName;
                        oTr.SuperSectionAbbr = sTopSectionAbbr;
                        oTr.Seed8Rank = sortIdx8;
                        sortIdx8 += 1;
                    }

                    var oBottomGroup = oBestInGroup.OrderByDescending(tr => tr.NorthingNumber).Skip(8).Take(8).ToList();
                    oBottomGroup.Sort();
                    sortIdx8 = 1;
                    foreach (var oTr in oBottomGroup)
                    {
                        oTr.SuperSection = sBottomSectionName;
                        oTr.SuperSectionAbbr = sBottomSectionAbbr;
                        oTr.Seed8Rank = sortIdx8;
                        sortIdx8 += 1;
                    }

                    // -- Now, add the top and bottom to their respective group slots, with the records again sorted by UPR ASC
                    oRet[sTopSectionName].Add(sGroup, oTopGroup.OrderBy(tr => tr.UnitedPowerRank).ToList());
                    oRet[sBottomSectionName].Add(sGroup, oBottomGroup.OrderBy(tr => tr.UnitedPowerRank).ToList());
                }
            }

            // -- Now handle the non-public in their own way
            string sNpSection = "Non-Public";
            var oNpGroups = (from tr in oTeamRecords
                             where tr.Section.Equals(sNpSection, StringComparison.OrdinalIgnoreCase)
                             select tr.Group).Distinct();
            foreach (string sNpGroup in oNpGroups)
            {
                string sGroup = sNpGroup;

                var oAllInGroup = oTeamRecords.Where(tr => tr.Section.Equals(sNpSection, StringComparison.OrdinalIgnoreCase) && tr.Group.Equals(sGroup, StringComparison.OrdinalIgnoreCase)).ToList();

                // -- Prime the super section seeding number
                int sortIdx = 1;
                oAllInGroup.Sort();
                foreach (var oTr in oAllInGroup)
                {
                    oTr.SuperSectionSeed = sortIdx;
                    sortIdx += 1;
                }

                var oBestInGroup = oAllInGroup.OrderBy(tr => tr.SuperSectionSeed).Take(12).ToList();

                // -- Now, add the top and bottom to their respective group slots, with the records again sorted by UPR ASC
                oRet[sNpSection].Add(sGroup, oBestInGroup);
            }

            return oRet;
        }

        public static void CalculateUpr(ref List<TeamRecordInfo> oTeamRecords)
        {
            // -- Need to apply the rankings for various stuff based on Section and Group
            var oFbSections = (from tr in oTeamRecords
                               select tr.Section).Distinct().ToList();
            foreach (string sFbSection in oFbSections)
            {
                string sSection = sFbSection;

                var oFbGroups = (from tr in oTeamRecords
                                 where tr.Section.Equals(sSection, StringComparison.OrdinalIgnoreCase)
                                 select tr.Group).Distinct();
                foreach (string sFbGroup in oFbGroups)
                {
                    string sGroup = sFbGroup;

                    // 1. Do the power points average rank
                    var oPpRanked = oTeamRecords.Where(tr => tr.Section.Equals(sSection, StringComparison.OrdinalIgnoreCase) && tr.Group.Equals(sGroup, StringComparison.OrdinalIgnoreCase)).OrderByDescending(tr => tr.PowerPointsAverage)
                            .GroupBy(tr => tr.PowerPointsAverage)
                            .Select((grp, i) => new RankResult
                            {
                                RankId = i,
                                Records = grp.OrderBy(g => g.TeamName)
                            });

                    int iPpRank = 1;
                    int iPpNextRank = 1;
                    foreach (var oRanked in oPpRanked)
                    {
                        int rankId = iPpRank;
                        foreach (var oTR in oRanked.Records)
                        {
                            iPpNextRank += 1;
                            int rankedTeamId = oTR.TeamId;
                            var oRankedTeam = oTeamRecords.FirstOrDefault(tr => tr.TeamId == rankedTeamId);
                            oRankedTeam.PowerPointsRank = rankId;
                        }
                        iPpRank = iPpNextRank;
                    }

                    // 2. Do the bpi rank
                    var oBpiRanked = oTeamRecords.Where(tr => tr.Section.Equals(sSection, StringComparison.OrdinalIgnoreCase) && tr.Group.Equals(sGroup, StringComparison.OrdinalIgnoreCase)).OrderByDescending(tr => tr.BornPowerIndex)
                            .GroupBy(tr => tr.BornPowerIndex)
                            .Select((grp, i) => new RankResult
                            {
                                RankId = i,
                                Records = grp.OrderBy(g => g.TeamName)
                            });

                    int iBpiRank = 1;
                    int iBpiNextRank = 1;
                    foreach (var oRanked in oBpiRanked)
                    {
                        int rankId = iBpiRank;
                        foreach (var oTR in oRanked.Records)
                        {
                            iBpiNextRank += 1;
                            int rankedTeamId = oTR.TeamId;
                            var oRankedTeam = oTeamRecords.FirstOrDefault(tr => tr.TeamId == rankedTeamId);
                            oRankedTeam.BornPowerIndexRank = rankId;
                        }
                        iBpiRank = iBpiNextRank;
                    }

                    // 3. Do the upr rank
                    var oUprRanked = oTeamRecords.Where(tr => tr.Section.Equals(sSection, StringComparison.OrdinalIgnoreCase) && tr.Group.Equals(sGroup, StringComparison.OrdinalIgnoreCase)).OrderBy(tr => tr.UnitedPowerRank)
                            .GroupBy(tr => tr.UnitedPowerRank)
                            .Select((grp, i) => new RankResult
                            {
                                RankId = i,
                                Records = grp.OrderBy(g => g.TeamName)
                            });

                    int iUprRank = 1;
                    int iUprNextRank = 1;
                    foreach (var oRanked in oUprRanked)
                    {
                        int rankId = iUprRank;
                        foreach (var oTR in oRanked.Records)
                        {
                            iUprNextRank += 1;
                            int rankedTeamId = oTR.TeamId;
                            var oRankedTeam = oTeamRecords.FirstOrDefault(tr => tr.TeamId == rankedTeamId);
                            oRankedTeam.UPRRank = rankId;
                        }
                        iUprRank = iUprNextRank;
                    }
                }
            }
        }
    }
}
