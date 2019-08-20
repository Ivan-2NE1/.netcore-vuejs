using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VSAND.Common;
using VSAND.Data.Entities;
using VSAND.Data.Repositories;
using VSAND.Interfaces;

namespace NJAM.PowerPoints
{
    public class BasketballPowerPoints : IType, IPowerPoints, IConvertible
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

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

        public int IncludeGames { get; set; } = 16;
        public int EligibleGamesCount { get; set; } = 0;
        public double PowerPoints { get; set; } = 0;

        private readonly IUnitOfWork _uow;

        public BasketballPowerPoints(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public BasketballPowerPoints(IUnitOfWork uow, int TeamId, List<VsandGameReport> oGameReports, DateTime dStart, DateTime dEnd)
        {
            _uow = uow;

            this.TeamId = TeamId;
            this.GameReports = oGameReports;
            this.StartDate = dStart;
            this.EndDate = dEnd;
        }

        public BasketballPowerPoints(IUnitOfWork uow, int TeamId, List<VsandGameReport> oGameReports, DateTime dStart, DateTime dEnd, int maxGames)
        {
            _uow = uow;

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

            // Power Point Calculation Procedure:
            // 1. 	Quality Points - Each school will receive the following "Quality Points" for a win from the first sixteen games: Win = 6 points, Lose= 0 points
            // 2. 	Group Points- Each school will receive "Group Points" from a team they defeated from the first sixteen games:
            // Group IV (4) = 4 points
            // Group Ill (3) = 3 points Group II (2) = 2 Points Group I (1) = 1Point
            // 3. 	Residual Points -Each team will receive residual points from one of the categories below based on the result of the
            // Game.

            // A.  Each school will receive residual points from a team they defeated. For each win your opponent has from the first sixteen games, you will receive the following (see example 1 ):
            // Win= 3 points, Lose= 0 points

            // B.  Each school will receive "Residual Points" from a team they lost to. For each win your opponent has that defeated you
            // (not including your game) from the first sixteen games, you will receive the follow (see example 2): Win = 1 point, Lose = 0 points
            // EXAMPLE(1(WIN))
            // Team A defeated Team B (Group IV) Team A record 14-2
            // Team B record 15-1



            // Team A would earn a total of 55 points for the Team B win; (6 points for the  Win, 4 points for the Group, 45  residual points)

            // EXAMPLE(2(LOSE))
            // Non-Public {Ill) Team A defeated Public Team B Team B record 14-2
            // Team A record 15-1

            // Team B would earn a total or 14 points from the Team A defeat (14 residual points)


            List<VsandGameReport> oEligibleGames = this.GameReports.Where(gr => gr.Deleted == false && gr.Final == true && gr.PPEligible == true).OrderBy(gt => gt.GameDate).Take(iIncludeGames).ToList();

            StringBuilder oCalcSb = new StringBuilder();
            oCalcSb.AppendLine("<table width=\"100%\" border=\"0\" cellpadding=\"5\">");
            oCalcSb.AppendLine("<caption>Limit first " + this.IncludeGames + " games" + (this.StartDate > DateHelp.SqlMinDate ? " between " + this.StartDate.ToString("%M/%d/yyyy") + " and " + this.EndDate.ToString("%M/%d/yyyy") : "") + "</caption>");
            oCalcSb.AppendLine("<thead><tr><th>Date</th><th>Opp</th><th>Result</th><th>Quality</th><th>Group</th><th>Residual</th><th>PP</th></tr></thead>");
            oCalcSb.AppendLine("<tbody>");
            int iRunQuality = 0;
            int iRunGroup = 0;
            decimal dRunResidual = 0;

            foreach (VsandGameReport oGR in oEligibleGames)
            {
                var oGameTeams = oGR.Teams;
                if (oGameTeams.Count == 2 && oGR.GameDate >= StartDate && oGR.GameDate <= EndDate)
                {
                    int iQualityPoints = 0;
                    int iGroupPoints = 0;
                    decimal dResidualPoints = 0;

                    int iGameReportId = oGR.GameReportId;
                    VsandGameReportTeam oTeam = oGameTeams.FirstOrDefault(gt => gt.TeamId == TeamId);
                    double dTeamScore = oTeam.FinalScore;

                    VsandGameReportTeam oOpp = oGameTeams.FirstOrDefault(gt => gt.TeamId != TeamId);
                    double dOppScore = oOpp.FinalScore;

                    int iOppTeamId = oOpp.TeamId;
                    string sOppGroup = "";
                    int iGroupVal = 0;
                    int iOppWins = 0;
                    int iOppLosses = 0;
                    int iOppTies = 0;
                    bool bOutOfState = false;
                    string sOppName = "";

                    int iOppRecordWins = 0;
                    int iOppRecordLosses = 0;
                    int iOppRecordTies = 0;

                    if (oOpp.Team != null)
                    {
                        sOppName = oOpp.Team.Name;
                        if (oOpp.Team.School.State != "NJ")
                        {
                            bOutOfState = true;
                        }

                        VsandTeamCustomCode oOppGroup = null/* TODO Change to default(_) if this is not a reference type */;
                        if (!bOutOfState)
                        {
                            oOppGroup = oOpp.Team.CustomCodes.FirstOrDefault(cc => cc.CodeName.Equals("GroupExchange", System.StringComparison.OrdinalIgnoreCase));
                            if (oOppGroup == null)
                            {
                                oOppGroup = oOpp.Team.CustomCodes.FirstOrDefault(gt => gt.CodeName.Equals("Group", System.StringComparison.OrdinalIgnoreCase));
                            }
                            if (oOppGroup != null)
                            {
                                sOppGroup = oOppGroup.CodeValue.ToLower();
                            }

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

                                if (dTeamScore.Equals(dOppScore))
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

                                if (iOppTies < 0)
                                {
                                    iOppTies = 0;
                                }
                            }
                        }
                        else
                        {
                            oOppGroup = oOpp.Team.CustomCodes.FirstOrDefault(cc => cc.CodeName == "GroupExchange");
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

                            iOppRecordWins = iOppWins;
                            iOppRecordLosses = iOppLosses;
                            iOppRecordTies = iOppTies;

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

                            if (iOppTies < 0)
                            {
                                iOppTies = 0;
                            }
                        }

                        switch (sOppGroup.ToLower())
                        {
                            case "group 4":
                                {
                                    iGroupVal = 4;
                                    break;
                                }

                            case "group 3":
                                {
                                    iGroupVal = 3;
                                    break;
                                }

                            case "group 2":
                                {
                                    iGroupVal = 2;
                                    break;
                                }

                            case "group 1":
                                {
                                    iGroupVal = 1;
                                    break;
                                }

                            default:
                                {
                                    int.TryParse(sOppGroup, out iGroupVal);
                                    break;
                                }
                        }

                        if (!bOutOfState)
                        {
                            List<VsandGameReport> oOppGameReports = null;

                            var oData = _uow.GameReports.List(
                                                 g => g.Teams.Any(gt => gt.Team.TeamId == iOppTeamId) && g.Deleted == false && g.Final == true && g.PPEligible == true && g.GameDate >= this.StartDate && g.GameDate <= this.EndDate,
                                                 x => x.OrderBy(g => g.GameDate),
                                                 new List<string> { "Teams" }
                                            ).Result.Take(this.IncludeGames);

                            if (oData != null)
                            {
                                oOppGameReports = oData.ToList();
                            }

                            foreach (VsandGameReport oOppGR in oOppGameReports)
                            {
                                var oOppGameTeams = oOppGR.Teams;
                                VsandGameReportTeam oOppTeam = oOppGameTeams.FirstOrDefault(gt => gt.TeamId == iOppTeamId);
                                double dOppTeamScore = oOppTeam.FinalScore;

                                VsandGameReportTeam oOppOpp = oOppGameTeams.FirstOrDefault(gt => gt.TeamId != iOppTeamId);
                                double dOppOppScore = oOppOpp.FinalScore;

                                if (dOppTeamScore > dOppOppScore)
                                {
                                    iOppRecordWins = iOppRecordWins + 1;
                                    if (oOppGR.GameReportId != iGameReportId)
                                    {
                                        iOppWins = iOppWins + 1;
                                    }
                                }
                                else if (dOppOppScore > dOppTeamScore)
                                {
                                    iOppRecordLosses = iOppRecordLosses + 1;
                                    if (oOppGR.GameReportId != iGameReportId)
                                    {
                                        iOppLosses = iOppLosses + 1;
                                    }
                                }
                                else
                                {
                                    iOppRecordTies = iOppRecordTies + 1;
                                    if (oOppGR.GameReportId != iGameReportId)
                                    {
                                        iOppTies = iOppTies + 1;
                                    }
                                }
                            }
                        }
                    }

                    string sResult = "";
                    if (dTeamScore > dOppScore)
                    {
                        iQualityPoints = 6;
                        iGroupPoints = iGroupVal;
                        dResidualPoints = (iOppWins * 3);
                        sResult = "W " + dTeamScore + "-" + dOppScore;
                    }
                    else if (dTeamScore < dOppScore)
                    {
                        iQualityPoints = 0;
                        iGroupPoints = 0;
                        dResidualPoints = (iOppWins * 1);
                        sResult = "L " + dOppScore + "-" + dTeamScore;
                    }

                    iRunQuality = iRunQuality + iQualityPoints;
                    iRunGroup = iRunGroup + iGroupPoints;
                    dRunResidual = dRunResidual + dResidualPoints;

                    decimal dPPVal = iQualityPoints + iGroupPoints + dResidualPoints;
                    oCalcSb.AppendLine("<tr><td class=\"center\">" + oGR.GameDate.ToString("%M/%d/yy") + "</td><td>" + oOpp.TeamName + " (" + iOppRecordWins + "-" + iOppRecordLosses + "-" + iOppRecordTies + ")</td><td>" + sResult + "</td><td class=\"center\">" + iQualityPoints + "</td><td class=\"center\">" + iGroupPoints + "</td><td class=\"center\">" + dResidualPoints + "</td><td class=\"center\">" + dPPVal + "</td></tr>");

                    dPowerPoints = dPowerPoints + iQualityPoints + iGroupPoints + (double)dResidualPoints;
                }
            }

            oCalcSb.AppendLine("<tr><td colspan=\"3\" style=\"text-align:right;font-weight:bold;\">Totals:</td><td class=\"center\">" + iRunQuality + "</td><td class=\"center\">" + iRunGroup + "</td><td class=\"center\">" + dRunResidual + "</td><td class=\"center\">" + dPowerPoints + "</td></tr>");
            oCalcSb.AppendLine("</tbody></table>");

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
                    string sPPFile = Path.Combine(sDir, TeamId + ".htm");
                    if (File.Exists(sPPFile))
                        File.Delete(sPPFile);
                    File.WriteAllText(sPPFile, oCalcSb.ToString());
                }
            }
            catch (Exception ex)
            {
                log.Error(ex, ex.Message);
            }

            PowerPoints = dPowerPoints;

            return (decimal)dPowerPoints;
        }

        public double TieBreak()
        {
            double dPPTB = 0;
            int iIncludeGames = this.IncludeGames;

            IEnumerable<VsandGameReport> oEligibleGames = this.GameReports.Where(gr => gr.Deleted == false && gr.Final == true && gr.Teams.Count == 2 && gr.PPEligible == true).OrderBy(gt => gt.GameDate).Take(iIncludeGames);

            foreach (VsandGameReport oGR in oEligibleGames)
            {
                var oGameTeams = oGR.Teams;
                if (oGameTeams.Count == 2 && oGR.GameDate >= StartDate && oGR.GameDate <= EndDate)
                {
                    VsandGameReportTeam oOpp = oGameTeams.FirstOrDefault(gt => gt.TeamId != TeamId);
                    int iOppTeamId = oOpp.TeamId;
                    string sOppGroup = "";
                    int iGroupVal = 0;
                    int iOppWins = 0;
                    bool bOutOfState = false;

                    if (oOpp.Team != null)
                    {
                        if (oOpp.Team.School.State != "NJ")
                        {
                            bOutOfState = true;
                        }

                        VsandTeamCustomCode oOppGroup = null;
                        if (!bOutOfState)
                        {
                            oOppGroup = oOpp.Team.CustomCodes.FirstOrDefault(cc => cc.CodeName.Equals("GroupExchange", System.StringComparison.OrdinalIgnoreCase));
                            if (oOppGroup == null)
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
                            oOppGroup = oOpp.Team.CustomCodes.FirstOrDefault(cc => cc.CodeName == "GroupExchange");
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
                        }
                        switch (sOppGroup.ToLower())
                        {
                            case "group 4":
                                {
                                    iGroupVal = 4;
                                    break;
                                }

                            case "group 3":
                                {
                                    iGroupVal = 3;
                                    break;
                                }

                            case "group 2":
                                {
                                    iGroupVal = 2;
                                    break;
                                }

                            case "group 1":
                                {
                                    iGroupVal = 1;
                                    break;
                                }
                        }

                        if (!bOutOfState)
                        {
                            List<VsandGameReport> oOppGameReports = null;

                            var oData = _uow.GameReports.List(
                                                g => g.Teams.Any(gt => gt.Team.TeamId == iOppTeamId) && g.Deleted == false && g.Final == true && g.PPEligible == true && g.Teams.Count == 2 && g.GameDate >= this.StartDate && g.GameDate <= this.EndDate,
                                                x => x.OrderBy(g => g.GameDate),
                                                new List<string> { "Teams" }
                                            ).Result.Take(this.IncludeGames);

                            if (oData != null)
                            {
                                oOppGameReports = oData.ToList();
                            }

                            foreach (VsandGameReport oOppGR in oOppGameReports)
                            {
                                var oOppGameTeams = oOppGR.Teams;
                                VsandGameReportTeam oOppTeam = oOppGameTeams.FirstOrDefault(gt => gt.TeamId == iOppTeamId);
                                double dOppTeamScore = oOppTeam.FinalScore;

                                VsandGameReportTeam oOppOpp = oOppGameTeams.FirstOrDefault(gt => gt.TeamId != iOppTeamId);
                                if (oOppOpp.TeamId != this.TeamId)
                                {
                                    double dOppOppScore = oOppOpp.FinalScore;

                                    if (dOppTeamScore > dOppOppScore)
                                    {
                                        iOppWins = iOppWins + 1;
                                    }
                                }
                            }
                        }
                    }

                    dPPTB = dPPTB + iGroupVal + iOppWins;
                }
            }

            return dPPTB;
        }

        public System.TypeCode GetTypeCode()
        {
            return TypeCode.Object;
        }

        public bool ToBoolean(System.IFormatProvider provider)
        {
            throw new InvalidCastException("NJAM.PowerPoints.BasketballPowerPoints cannot be converted to Boolean");
        }

        public byte ToByte(System.IFormatProvider provider)
        {
            throw new InvalidCastException("NJAM.PowerPoints.BasketballPowerPoints cannot be converted to Boolean");
        }

        public char ToChar(System.IFormatProvider provider)
        {
            throw new InvalidCastException("NJAM.PowerPoints.BasketballPowerPoints cannot be converted to Boolean");
        }

        public DateTime ToDateTime(System.IFormatProvider provider)
        {
            throw new InvalidCastException("NJAM.PowerPoints.BasketballPowerPoints cannot be converted to Boolean");
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
            throw new InvalidCastException("NJAM.PowerPoints.BasketballPowerPoints cannot be converted to Boolean");
        }

        public int ToInt32(System.IFormatProvider provider)
        {
            throw new InvalidCastException("NJAM.PowerPoints.BasketballPowerPoints cannot be converted to Boolean");
        }

        public long ToInt64(System.IFormatProvider provider)
        {
            throw new InvalidCastException("NJAM.PowerPoints.BasketballPowerPoints cannot be converted to Boolean");
        }

        public sbyte ToSByte(System.IFormatProvider provider)
        {
            throw new InvalidCastException("NJAM.PowerPoints.BasketballPowerPoints cannot be converted to Boolean");
        }

        public float ToSingle(System.IFormatProvider provider)
        {
            throw new InvalidCastException("NJAM.PowerPoints.BasketballPowerPoints cannot be converted to Boolean");
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
            throw new InvalidCastException("NJAM.PowerPoints.BasketballPowerPoints cannot be converted to Boolean");
        }

        public uint ToUInt32(System.IFormatProvider provider)
        {
            throw new InvalidCastException("NJAM.PowerPoints.BasketballPowerPoints cannot be converted to Boolean");
        }

        public ulong ToUInt64(System.IFormatProvider provider)
        {
            throw new InvalidCastException("NJAM.PowerPoints.BasketballPowerPoints cannot be converted to Boolean");
        }
    }
}
