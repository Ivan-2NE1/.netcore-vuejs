using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VSAND.Data.Entities;
using VSAND.Data.Repositories;

namespace VSAND.Services.Data.Calculations
{
    public class PowerPoints
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public VsandPowerPointsConfig PpConfig { get; set; }
        public int IncludeGames { get; set; } = 0;
        public int TeamId { get; set; } = 0;
        public List<VsandGameReport> GameReports { get; set; }

        private readonly IUnitOfWork _uow;

        public PowerPoints(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public PowerPoints(IUnitOfWork uow, VsandPowerPointsConfig ppConfig, int teamId, List<VsandGameReport> oGameReports)
        {
            _uow = uow;

            this.PpConfig = ppConfig;
            IncludeGames = ppConfig.IncludeGamesCount;
            this.TeamId = teamId;
            GameReports = oGameReports;
        }

        public decimal ToDecimal()
        {
            decimal dPowerPoints = 0;

            DateTime dEnd = new DateTime(PpConfig.EndDate.Year, PpConfig.EndDate.Month, PpConfig.EndDate.Day, 23, 59, 59);

            string sTieExclude = "";
            List<VsandGameReport> oEligibleGames = null;
            var oQuery = (from gr in GameReports
                          where gr.Teams.Count == 2
                          && gr.Deleted == false
                          && gr.Final == true
                          && gr.GameDate >= PpConfig.StartDate
                          && gr.GameDate <= dEnd
                          select gr);
            if (!PpConfig.IncludeTieGames)
            {
                oQuery = oQuery.Where(gr => gr.Teams.Any(grt => grt.FinalScore > gr.Teams.OrderBy(grtfs => grtfs.FinalScore).FirstOrDefault().FinalScore));
                sTieExclude = " non-tie";
            }
            oEligibleGames = oQuery.OrderBy(gr => gr.GameDate).Take(PpConfig.IncludeGamesCount).ToList();

            StringBuilder oCalcSb = new StringBuilder();
            oCalcSb.AppendLine("<table width=\"100%\" border=\"0\" cellpadding=\"5\">");
            string sLimit = " between " + PpConfig.StartDate.ToString("%M/%d/yyyy") + " and " + PpConfig.EndDate.ToString("%M/%d/yyyy");
            oCalcSb.AppendLine("<caption>Limit first " + IncludeGames.ToString() + sTieExclude + " games" + sLimit + "</caption>");
            oCalcSb.AppendLine("<thead><tr><th>Date</th><th>Opp</th><th>Result</th><th>Quality</th><th>Group</th><th>Residual</th><th>PP</th></tr></thead>");
            oCalcSb.AppendLine("<tbody>");

            double dLowPPVal = 999;
            List<double> oPpVals = new List<double>();
            double iRunQuality = 0;
            double iRunGroup = 0;
            double dRunResidual = 0;

            foreach (var oGr in oEligibleGames)
            {
                var oGameTeams = oGr.Teams;

                double iQualityPoints = 0;
                double iGroupPoints = 0;
                double dResidualPoints = 0;

                var oTeam = oGameTeams.FirstOrDefault(gt => gt.TeamId == TeamId);
                double dTeamScore = oTeam.FinalScore;

                var oOpp = oGameTeams.FirstOrDefault(gt => gt.TeamId != TeamId);
                if (oOpp == null)
                {
                    Log.Debug("Check gamereportid " + oGr.GameReportId);
                }
                else
                {
                    double dOppScore = oOpp.FinalScore;

                    int iOppTeamId = oOpp.Team.TeamId;
                    string sOppGroup = "";
                    int iGroupVal = 0;
                    int iOppWins = 0;
                    int iOppLosses = 0;
                    int iOppTies = 0;
                    bool bOutOfState = false;
                    // Dim sOppName As String = ""

                    int iOppRecordWins = 0;
                    int iOppRecordLosses = 0;
                    int iOppRecordTies = 0;

                    if (oOpp.Team != null)
                    {
                        // sOppName = oOpp.Team.Name
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

                            // -- Use their OOS designated record (if not 0-0-0)
                            var oOppWinEx = oOpp.Team.CustomCodes.FirstOrDefault(gt => gt.CodeName == "OOSFinalWins");
                            if (oOppWinEx != null)
                            {
                                string sOppWins = oOppWinEx.CodeValue;
                                int.TryParse(sOppWins, out iOppWins);
                            }
                            var oOppLoseEx = oOpp.Team.CustomCodes.FirstOrDefault(gt => gt.CodeName == "OOSFinalLosses");
                            if (oOppLoseEx != null)
                            {
                                string sOppLosses = oOppLoseEx.CodeValue;
                                int.TryParse(sOppLosses, out iOppLosses);
                            }
                            var oOppTieEx = oOpp.Team.CustomCodes.FirstOrDefault(gt => gt.CodeName == "OOSFinalTies");
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

                                int iOppRecordWinOffset = 0;
                                int iOppRecordLossOffset = 0;
                                int iOppRecordTieOffset = 0;
                                List<VsandGameReport> oVsGames = oEligibleGames.Where(gr => gr.Teams.Any(grt => grt.TeamId == iOppTeamId)).ToList();
                                foreach (var oVsGame in oVsGames)
                                {
                                    var oVsTeam = oVsGame.Teams.FirstOrDefault(grt => grt.TeamId == TeamId);
                                    var oVsOpp = oVsGame.Teams.FirstOrDefault(grt => grt.TeamId == iOppTeamId);

                                    if (oVsOpp.FinalScore > oVsTeam.FinalScore)
                                    {
                                        iOppRecordWinOffset = iOppRecordWinOffset - 1;
                                    }
                                    else if (Math.Abs(oVsOpp.FinalScore - oVsTeam.FinalScore) == 0)
                                    {
                                        iOppRecordTieOffset = iOppRecordTieOffset - 1;
                                    }
                                    else
                                    {
                                        iOppRecordLossOffset = iOppRecordLossOffset - 1;
                                    }
                                }

                                if (Math.Abs(dTeamScore - dOppScore) == 0)
                                {
                                    iOppTies = iOppTies + iOppRecordTieOffset;
                                }
                                else if (dTeamScore < dOppScore)
                                {
                                    iOppWins = iOppWins + iOppRecordWinOffset;
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
                            var oOppWinEx = oOpp.Team.CustomCodes.FirstOrDefault(gt => gt.CodeName == "OOSFinalWins");
                            if (oOppWinEx != null)
                            {
                                string sOppWins = oOppWinEx.CodeValue;
                                int.TryParse(sOppWins, out iOppWins);
                            }
                            var oOppLoseEx = oOpp.Team.CustomCodes.FirstOrDefault(gt => gt.CodeName == "OOSFinalLosses");
                            if (oOppLoseEx != null)
                            {
                                string sOppLosses = oOppLoseEx.CodeValue;
                                int.TryParse(sOppLosses, out iOppLosses);
                            }
                            var oOppTieEx = oOpp.Team.CustomCodes.FirstOrDefault(gt => gt.CodeName == "OOSFinalTies");
                            if (oOppTieEx != null)
                            {
                                string sOppTies = oOppTieEx.CodeValue;
                                int.TryParse(sOppTies, out iOppTies);
                            }

                            iOppRecordWins = iOppWins;
                            iOppRecordLosses = iOppLosses;
                            iOppRecordTies = iOppTies;

                            int iOppRecordWinOffset = 0;
                            int iOppRecordLossOffset = 0;
                            int iOppRecordTieOffset = 0;
                            List<VsandGameReport> oVsGames = System.Linq.Enumerable.ToList<VsandGameReport>(oEligibleGames.Where(gr => gr.Teams.Any((grt => grt.TeamId == iOppTeamId))));
                            foreach (var oVsGame in oVsGames)
                            {
                                var oVSTeam = oVsGame.Teams.FirstOrDefault(grt => grt.TeamId == TeamId);
                                var oVSOpp = oVsGame.Teams.FirstOrDefault(grt => grt.TeamId == iOppTeamId);

                                if (oVSOpp.FinalScore > oVSTeam.FinalScore)
                                {
                                    iOppRecordWinOffset = iOppRecordWinOffset - 1;
                                }
                                else if (oVSOpp.FinalScore == oVSTeam.FinalScore)
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

                            if (iOppWins < 0)
                            {
                                iOppWins = 0;
                            }
                        }

                        string sGroup = sOppGroup.ToLower().Replace("group", "").Trim();
                        int.TryParse(sGroup, out iGroupVal);

                        if (!bOutOfState)
                        {
                            List<VsandGameReport> oOppGameReports = null;

                            oOppGameReports = _uow.GameReports
                                                    .List(gr => gr.Teams.Count == 2 && gr.Teams.Any(grt => grt.Team.TeamId == iOppTeamId) && gr.Deleted == false && gr.Final == true && gr.GameDate >= PpConfig.StartDate && gr.GameDate <= dEnd,
                                                          null,
                                                          new List<string> { "Teams.Team" }).Result;

                            if (!PpConfig.IncludeTieGames)
                            {
                                oOppGameReports = oOppGameReports.Where(gr => gr.Teams.Any(grt => grt.FinalScore > gr.Teams.OrderBy(grtfs => grtfs.FinalScore).FirstOrDefault().FinalScore)).ToList();
                            }
                            oOppGameReports = oOppGameReports.OrderBy(gr => gr.GameDate).Take(PpConfig.IncludeGamesCount).ToList();

                            foreach (var oOppGr in oOppGameReports)
                            {
                                var oOppGameTeams = oOppGr.Teams;
                                var oOppTeam = oOppGameTeams.FirstOrDefault(gt => gt.TeamId == iOppTeamId);
                                double dOppTeamScore = oOppTeam.FinalScore;

                                var oOppOpp = oOppGameTeams.FirstOrDefault(gt => gt.TeamId != iOppTeamId);
                                if (oOppOpp != null)
                                {
                                    double dOppOppScore = oOppOpp.FinalScore;

                                    if (dOppTeamScore > dOppOppScore)
                                    {
                                        if (oOppGr.GameReportId != oGr.GameReportId)
                                        {
                                            iOppWins = iOppWins + 1;
                                        }
                                        // -- They should get a residual for a subsequent victory over themselves
                                        iOppRecordWins = iOppRecordWins + 1;
                                    }
                                    else if (dOppOppScore > dOppTeamScore)
                                    {
                                        iOppRecordLosses = iOppRecordLosses + 1;
                                        if (oOppOpp.TeamId != this.TeamId)
                                        {
                                            iOppLosses = iOppLosses + 1;
                                        }
                                    }
                                    else
                                    {
                                        iOppRecordTies = iOppRecordTies + 1;
                                        if (oOppOpp.TeamId != this.TeamId)
                                        {
                                            iOppTies = iOppTies + 1;
                                        }
                                    }
                                }
                                else
                                {
                                    Log.Debug("Check this game for a problem: " + oOppGr.GameReportId);
                                }
                            }
                        }
                    }

                    string sResult = "";
                    if (dTeamScore > dOppScore)
                    {
                        iQualityPoints = PpConfig.WinValue;
                        iGroupPoints = iGroupVal * PpConfig.GroupWinMultiplier;
                        dResidualPoints = (iOppWins * PpConfig.ResidualWinMultiplierOppWins) + (iOppLosses * PpConfig.ResidualWinMultiplierOppLosses) + (iOppTies * PpConfig.ResidualWinMultiplierOppTies);
                        sResult = "W " + dTeamScore + "-" + dOppScore;
                    }
                    else if (Math.Abs(dTeamScore - dOppScore) == 0)
                    {
                        iQualityPoints = PpConfig.TieValue;
                        iGroupPoints = iGroupVal * PpConfig.GroupTieMultiplier;
                        dResidualPoints = (iOppWins * PpConfig.ResidualTieMultiplierOppWins) + (iOppLosses * PpConfig.ResidualTieMultiplierOppLosses) + (iOppTies * PpConfig.ResidualTieMultiplierOppTies);
                        sResult = "T " + dTeamScore + "-" + dOppScore;
                    }
                    else if (dTeamScore < dOppScore)
                    {
                        iQualityPoints = PpConfig.LossValue;
                        iGroupPoints = PpConfig.GroupLossMultiplier;
                        dResidualPoints = (iOppWins * PpConfig.ResidualLossMultiplierOppWins) + (iOppLosses * PpConfig.ResidualLossMultiplierOppLosses) + (iOppTies * PpConfig.ResidualLossMultiplierOppTies);
                        sResult = "L " + dOppScore + "-" + dTeamScore;
                    }

                    iRunQuality = iRunQuality + iQualityPoints;
                    iRunGroup = iRunGroup + iGroupPoints;
                    dRunResidual = dRunResidual + dResidualPoints;

                    double dPpVal = iQualityPoints + iGroupPoints + dResidualPoints;
                    oPpVals.Add(dPpVal);

                    oCalcSb.AppendLine("<tr><td class=\"center\">" + oGr.GameDate.ToString("%M/%d/yy") + "</td><td>" + oOpp.TeamName + " (" + iOppRecordWins + "-" + iOppRecordLosses + "-" + iOppRecordTies + ")</td><td>" + sResult + "</td><td class=\"center\">" + iQualityPoints + "</td><td class=\"center\">" + iGroupPoints + "</td><td class=\"center\">" + dResidualPoints + "</td><td class=\"center\">" + dPpVal + "</td></tr>");

                    dPowerPoints = dPowerPoints + (decimal)iQualityPoints + (decimal)iGroupPoints + (decimal)dResidualPoints;
                }
            }

            // -- Only use low val if we are configured to reduce the lowest value
            int iRemoveLowCount = 0;
            if (PpConfig.BestNGames < PpConfig.IncludeGamesCount && PpConfig.BestNGames > 0)
            {
                iRemoveLowCount = PpConfig.IncludeGamesCount - PpConfig.BestNGames;
                // -- Do not remove low games until threshold is met
                if (iRemoveLowCount >= oEligibleGames.Count)
                {
                    iRemoveLowCount = 0;
                }
            }

            dLowPPVal = 0;
            oCalcSb.AppendLine("<tr><td colspan=\"3\" style=\"text-align:right;font-weight:bold;\">Totals:</td><td class=\"center\">" + iRunQuality + "</td><td class=\"center\">" + iRunGroup + "</td><td class=\"center\">" + dRunResidual + "</td><td class=\"center\">" + dPowerPoints + "</td></tr>");
            if (iRemoveLowCount > 0)
            {
                List<double> oLowest = oPpVals.OrderBy(d => d).Take(iRemoveLowCount).ToList();
                dLowPPVal = oLowest.Sum(d => d);

                if (iRemoveLowCount == 1)
                {
                    oCalcSb.AppendLine("<tr><td colspan=\"6\" style=\"text-align:right;font-weight:bold;\">Less Lowest Power Point Value:</td><td class=\"center\">-" + dLowPPVal + "</td></tr>");
                }
                else
                {
                    oCalcSb.AppendLine("<tr><td colspan=\"6\" style=\"text-align:right;font-weight:bold;\">Less " + iRemoveLowCount + " Lowest Power Point Values:</td><td class=\"center\">-" + dLowPPVal + "</td></tr>");
                }
                oCalcSb.AppendLine("<tr><td colspan=\"6\" style=\"text-align:right;font-weight:bold;\">Final Value:</td><td class=\"center\">" + (dPowerPoints - (decimal)dLowPPVal) + "</td></tr>");
            }
            oCalcSb.AppendLine("</tbody></table>");

            try
            {
                // -- save the calculation for reference
                var oRefGame = oEligibleGames.FirstOrDefault();
                if (oRefGame != null)
                {
                    int iSchedYear = oRefGame.ScheduleYearId;
                    int iSport = oRefGame.SportId;
                    string sDir = "/appdata/PPCalcTest/" + iSchedYear + "/" + iSport;
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

            dPowerPoints = dPowerPoints - (decimal)dLowPPVal;

            return dPowerPoints;
        }

        public decimal TieBreak()
        {
            decimal dPPTB = 0;

            return dPPTB;
        }
    }
}
