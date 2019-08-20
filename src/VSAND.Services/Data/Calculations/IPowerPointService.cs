using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VSAND.Data.Entities;
using VSAND.Data.ViewModels;

namespace VSAND.Services.Data.Calculations
{
    public interface IPowerPointService
    {
        decimal Calculate(int teamId, List<VsandGameReport> oGameReports);
        decimal Calculate(VsandPowerPointsConfig ppConfig, int teamId, List<VsandGameReport> oGameReports);
        decimal Calculate(int teamId, List<VsandGameReport> oGameReports, DateTime dStart, DateTime dEnd, string sDataType);
        decimal Calculate(int teamId, List<VsandGameReport> oGameReports, DateTime dStart, DateTime dEnd, int maxGames, string sDataType);
        double TieBreak(int teamId, List<VsandGameReport> oGameReports, DateTime dStart, DateTime dEnd, string sDataType);
        int GetIncludeGames(string sDataType);
        decimal SummarizeCalculate(int teamId, List<VsandGameReport> oGameReports, DateTime dStart, DateTime dEnd, int maxGames, string sDataType, ref int eligibleGames);
        Task<VsandPowerPointsConfig> GetPPConfig(int sportId, int scheduleYearId);
        Task<ServiceResult<VsandPowerPointsConfig>> SavePPConfig(int scheduleyearId, int sportId, int includeGamesCount, bool includeTieGames, int bestNGames, DateTime startDate, DateTime endDate, double winValue, double lossValue, double tieValue, double groupWinMultiplier, double groupLossMultiplier, double groupTieMultiplier, double residualWinMultiplierOppWins, double residualWinMultiplierOppLosses, double residualWinMultiplierOppTies, double residualLossMultiplierOppWins, double residualLossMultiplierOppLosses, double residualLossMultiplierOppTies, double residualTieMultiplierOppWins, double residualTieMultiplierOppLosses, double residualTieMultiplierOppTies, DateTime gracePeriodEndDate, DateTime seedingPeriodEndDate);
    }
}
