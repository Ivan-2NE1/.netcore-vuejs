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
    public class BaseballPowerPoints : IType, IPowerPoints, IConvertible
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public int TeamId { get; set; } = 0;
        public List<VsandGameReport> GameReports { get; set; }

        private DateTime _startDate = DateHelp.SqlMinDate;
        public DateTime StartDate
        {
            get {
                return new DateTime(_startDate.Year, _startDate.Month, _startDate.Day, 0, 0, 0);
            }
            set {
                _startDate = value;
            }
        }

        private DateTime _endDate = DateHelp.SqlMaxDate;
        public DateTime EndDate
        {
            get {
                return new DateTime(_endDate.Year, _endDate.Month, _endDate.Day, 23, 59, 59);
            }
            set {
                _endDate = value;
            }
        }

        public int IncludeGames { get; set; } = 15;
        public int EligibleGamesCount { get; set; } = 0;
        public decimal PowerPoints { get; set; } = 0;

        double IPowerPoints.PowerPoints { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        private readonly IUnitOfWork _uow;

        public BaseballPowerPoints(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public BaseballPowerPoints(IUnitOfWork uow, int teamId, List<VsandGameReport> oGameReports, DateTime dStart, DateTime dEnd)
        {
            _uow = uow;

            this.TeamId = teamId;
            GameReports = oGameReports;
            StartDate = dStart;
            EndDate = dEnd;
        }

        public BaseballPowerPoints(IUnitOfWork uow, int TeamId, List<VsandGameReport> oGameReports, DateTime dStart, DateTime dEnd, int maxGames)
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
            decimal dPowerPoints = 0;
            int iIncludeGames = IncludeGames;

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

            DateTime dEndDate = new DateTime(EndDate.Year, EndDate.Month, EndDate.Day, 23, 59, 59, 59);

            // Dim oEligibleGames As List(Of VsandGameReport) = GameReports.Where(Function(gr As VsandGameReport) gr.Deleted = False).OrderBy(Function(gt As VsandGameReport) gt.GameDate).Take(iIncludeGames).ToList()
            List<VsandGameReport> oEligibleGames = GameReports.Where(gr => gr.Deleted == false
                                                                                 && gr.Final == true
                                                                                 && gr.Teams.Count == 2
                                                                                 && gr.Teams.Any((grt => grt.FinalScore > gr.Teams.OrderBy(grtfs => grtfs.FinalScore).FirstOrDefault().FinalScore))
                                                                                 && gr.GameDate >= StartDate
                                                                                 && gr.PPEligible == true
                                                                                 && gr.GameDate <= dEndDate).OrderBy(gt => gt.GameDate).Take(iIncludeGames).ToList();

            StringBuilder oCalcSb = new StringBuilder();
            oCalcSb.AppendLine("<table width=\"100%\" border=\"0\" cellpadding=\"5\">");
            string sLimit = "";
            if (StartDate > DateHelp.SqlMinDate)
            {
                sLimit = " between " + StartDate.ToString("%M/%d/yyyy") + " and " + EndDate.ToString("%M/%d/yyyy");
            }
            oCalcSb.AppendLine("<caption>Limit first " + IncludeGames.ToString() + " games" + sLimit + "</caption>");
            oCalcSb.AppendLine("<thead><tr><th>Date</th><th>Opp</th><th>Result</th><th>Quality</th><th>Group</th><th>Residual</th><th>PP</th></tr></thead>");
            oCalcSb.AppendLine("<tbody>");
            int iRunQuality = 0;
            int iRunGroup = 0;
            decimal dRunResidual = 0;

            foreach (VsandGameReport oGr in oEligibleGames)
            {
                var oGameTeams = oGr.Teams;
                // If oGameTeams.Count = 2 Then
                int iQualityPoints = 0;
                int iGroupPoints = 0;
                decimal dResidualPoints = 0;

                // Dim iGameReportId As Integer = oGr.GameReportId
                VsandGameReportTeam oTeam = oGameTeams.FirstOrDefault(gt => gt.TeamId == TeamId);
                double dTeamScore = oTeam.FinalScore;

                VsandGameReportTeam oOpp = oGameTeams.FirstOrDefault(gt => gt.TeamId != TeamId);
                if (oOpp == null)
                {
                    Log.Debug("Check gamereportid " + oGr.GameReportId);
                }
                else
                {
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

                            if (dTeamScore.Equals(dOppScore))
                            {
                                iOppTies = iOppTies - 1;
                            }
                            else if (dTeamScore < dOppScore)
                            {
                                iOppWins = iOppWins - 1;
                            }
                        }

                        if (iOppWins < 0)
                        {
                            iOppWins = 0;
                        }

                        if (iOppTies < 0)
                        {
                            iOppTies = 0;
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
                                                g => g.Teams.Any(gt => gt.Team.TeamId == iOppTeamId) && g.Deleted == false && g.Final == true && g.Teams.Count > 1 && g.Teams.Any(grt => grt.FinalScore > g.Teams.OrderBy(grtfs => grtfs.FinalScore).FirstOrDefault().FinalScore) && g.GameDate >= StartDate && g.GameDate <= EndDate && g.PPEligible == true,
                                                x => x.OrderBy(g => g.GameDate),
                                                new List<string> { "Teams" }
                                            ).Result.Take(IncludeGames);

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
                                if (oOppOpp != null)
                                {
                                    // If oOppOpp.TeamId <> Me.TeamId Then
                                    double dOppOppScore = oOppOpp.FinalScore;

                                    if (dOppTeamScore > dOppOppScore)
                                    {
                                        iOppRecordWins = iOppRecordWins + 1;
                                        if (oOppGR.GameReportId != oGr.GameReportId)
                                        {
                                            iOppWins = iOppWins + 1;
                                        }
                                    }
                                    else if (dOppOppScore > dOppTeamScore)
                                    {
                                        iOppRecordLosses = iOppRecordLosses + 1;
                                        if (oOppGR.GameReportId != oGr.GameReportId)
                                        {
                                            iOppLosses = iOppLosses + 1;
                                        }
                                    }
                                    else
                                    {
                                        iOppRecordTies = iOppRecordTies + 1;
                                        if (oOppGR.GameReportId != oGr.GameReportId)
                                        {
                                            iOppTies = iOppTies + 1;
                                        }
                                    }
                                }
                                else
                                {
                                    Log.Debug("Check this game for a problem: " + oOppGR.GameReportId);
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

                    decimal dPpVal = iQualityPoints + iGroupPoints + dResidualPoints;
                    oCalcSb.AppendLine("<tr><td class=\"center\">" + oGr.GameDate.ToString("%M/%d/yy") + "</td><td>" + oOpp.TeamName + " (" + iOppRecordWins + "-" + iOppRecordLosses + "-" + iOppRecordTies + ")</td><td>" + sResult + "</td><td class=\"center\">" + iQualityPoints + "</td><td class=\"center\">" + iGroupPoints + "</td><td class=\"center\">" + dResidualPoints + "</td><td class=\"center\">" + dPpVal + "</td></tr>");

                    dPowerPoints = dPowerPoints + iQualityPoints + iGroupPoints + dResidualPoints;
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
                    string sPpFile = Path.Combine(sDir, TeamId + ".htm");
                    if (File.Exists(sPpFile))
                    {
                        File.Delete(sPpFile);
                    }
                    File.WriteAllText(sPpFile, oCalcSb.ToString());
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
            }

            PowerPoints = dPowerPoints;

            return dPowerPoints;
        }

        public decimal TieBreak()
        {
            decimal dPPTB = 0;
            int iIncludeGames = IncludeGames;

            IEnumerable<VsandGameReport> oEligibleGames = GameReports.Where(gr => gr.Deleted == false
                                                                                             && gr.Final == true
                                                                                             && gr.Teams.Count == 2
                                                                                             && gr.PPEligible == true
                                                                                             && gr.Teams.Any((grt => grt.FinalScore > gr.Teams.OrderBy(grtfs => grtfs.FinalScore).FirstOrDefault().FinalScore))).OrderBy(gt => gt.GameDate).Take(iIncludeGames);

            foreach (VsandGameReport oGr in oEligibleGames)
            {
                var oGameTeams = oGr.Teams;
                if (oGameTeams.Count == 2)
                {
                    VsandGameReportTeam oOpp = oGameTeams.FirstOrDefault(gt => gt.TeamId != TeamId);
                    if (oOpp == null)
                    {
                        Log.Debug("Check gamereportid " + oGr.GameReportId);
                    }
                    else
                    {
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
                                                g => g.Teams.Any(gt => gt.Team.TeamId == iOppTeamId) && g.Deleted == false && g.Final == true && g.Teams.Count == 2 && g.GameDate >= StartDate && g.GameDate <= EndDate && g.PPEligible == true && g.Teams.Any((grt => grt.FinalScore > g.Teams.OrderBy(grtfs => grtfs.FinalScore).FirstOrDefault().FinalScore)),
                                                x => x.OrderBy(g => g.GameDate),
                                                new List<string> { "Teams" }
                                            ).Result.Take(IncludeGames);

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
                                    if (oOppOpp != null)
                                    {
                                        double dOppOppScore = oOppOpp.FinalScore;

                                        if (dOppTeamScore > dOppOppScore)
                                        {
                                            iOppWins = iOppWins + 1;
                                        }
                                    }
                                    else
                                    {
                                        Log.Debug("Check this gamereportid: " + oOppGR.GameReportId);
                                    }
                                }
                            }
                        }

                        dPPTB = dPPTB + iGroupVal + iOppWins;
                    }
                }
            }

            return dPPTB;
        }

        public TypeCode GetTypeCode()
        {
            return TypeCode.Object;
        }

        public bool ToBoolean(IFormatProvider provider)
        {
            throw new InvalidCastException("NJAM.PowerPoints.BaseballPowerPoints cannot be converted to Boolean");
        }

        public byte ToByte(IFormatProvider provider)
        {
            throw new InvalidCastException("NJAM.PowerPoints.BaseballPowerPoints cannot be converted to Boolean");
        }

        public char ToChar(IFormatProvider provider)
        {
            throw new InvalidCastException("NJAM.PowerPoints.BaseballPowerPoints cannot be converted to Boolean");
        }

        public DateTime ToDateTime(IFormatProvider provider)
        {
            throw new InvalidCastException("NJAM.PowerPoints.BaseballPowerPoints cannot be converted to Boolean");
        }

        public decimal ToDecimal(IFormatProvider provider)
        {
            return this.ToDecimal();
        }

        public double ToDouble(IFormatProvider provider)
        {
            return (double)ToDecimal();
        }

        public short ToInt16(IFormatProvider provider)
        {
            throw new InvalidCastException("NJAM.PowerPoints.BaseballPowerPoints cannot be converted to Boolean");
        }

        public int ToInt32(IFormatProvider provider)
        {
            throw new InvalidCastException("NJAM.PowerPoints.BaseballPowerPoints cannot be converted to Boolean");
        }

        public long ToInt64(IFormatProvider provider)
        {
            throw new InvalidCastException("NJAM.PowerPoints.BaseballPowerPoints cannot be converted to Boolean");
        }

        public sbyte ToSByte(IFormatProvider provider)
        {
            throw new InvalidCastException("NJAM.PowerPoints.BaseballPowerPoints cannot be converted to Boolean");
        }

        public float ToSingle(IFormatProvider provider)
        {
            throw new InvalidCastException("NJAM.PowerPoints.BaseballPowerPoints cannot be converted to Boolean");
        }

        public string ToString(IFormatProvider provider)
        {
            return ToDecimal().ToString();
        }

        public object ToType(Type conversionType, IFormatProvider provider)
        {
            return this.GetType();
        }

        public ushort ToUInt16(IFormatProvider provider)
        {
            throw new InvalidCastException("NJAM.PowerPoints.BaseballPowerPoints cannot be converted to Boolean");
        }

        public uint ToUInt32(IFormatProvider provider)
        {
            throw new InvalidCastException("NJAM.PowerPoints.BaseballPowerPoints cannot be converted to Boolean");
        }

        public ulong ToUInt64(IFormatProvider provider)
        {
            throw new InvalidCastException("NJAM.PowerPoints.BaseballPowerPoints cannot be converted to Boolean");
        }

        double IPowerPoints.TieBreak()
        {
            throw new NotImplementedException();
        }
    }
}
