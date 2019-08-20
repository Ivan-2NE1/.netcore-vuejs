using NLog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VSAND.Data.Entities;
using VSAND.Data.Repositories;
using VSAND.Data.ViewModels;
using VSAND.Interfaces;

namespace VSAND.Services.Data.Calculations
{
    public class PowerPointService : IPowerPointService
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly IUnitOfWork _uow;

        public PowerPointService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public decimal Calculate(int teamId, List<VsandGameReport> oGameReports)
        {
            VsandTeam oTeam = _uow.Teams.Single(t => t.TeamId == teamId).Result;
            if (oTeam != null)
            {
                var oConfig = GetPPConfig(oTeam.SportId, oTeam.ScheduleYearId).Result;
                if (oConfig != null)
                {
                    return Calculate(oConfig, teamId, oGameReports);
                }
            }
            return 0;
        }

        public decimal Calculate(VsandPowerPointsConfig ppConfig, int teamId, List<VsandGameReport> oGameReports)
        {
            PowerPoints oPpCalc = new PowerPoints(_uow, ppConfig, teamId, oGameReports);

            decimal dValue = oPpCalc.ToDecimal();

            return dValue;
        }

        public decimal Calculate(int teamId, List<VsandGameReport> oGameReports, DateTime dStart, DateTime dEnd, string sDataType)
        {
            decimal dValue = 0;
            Type oType = System.Type.GetType(sDataType, false, true);
            if (oType != null)
            {
                Type oInt = oType.GetInterface("IType");
                if (oInt != null)
                {
                    var aParam = new object[] { _uow, teamId, oGameReports, dStart, dEnd };
                    List<Type> constParamTypes = new List<Type>();
                    for (int iParam = 0; iParam <= aParam.Length - 1; iParam++)
                    {
                        object constParam = aParam[iParam];
                        // ReSharper disable VBPossibleMistakenCallToGetType.2
                        constParamTypes.Add(constParam.GetType());
                    }
                    System.Reflection.ConstructorInfo constructor = oType.GetConstructor(constParamTypes.ToArray());
                    dValue = (constructor.Invoke(aParam) as IType).ToDecimal();
                }
            }
            return dValue;
        }

        public decimal Calculate(int teamId, List<VsandGameReport> oGameReports, DateTime dStart, DateTime dEnd, int maxGames, string sDataType)
        {
            decimal dValue = 0;
            Type oType = System.Type.GetType(sDataType, false, true);
            if (oType != null)
            {
                Type oInt = oType.GetInterface("IType");
                if (oInt != null)
                {
                    var aParam = new object[] { _uow, teamId, oGameReports, dStart, dEnd, maxGames };
                    List<Type> constParamTypes = new List<Type>();
                    for (int iParam = 0; iParam <= aParam.Length - 1; iParam++)
                    {
                        object constParam = aParam[iParam];
                        // ReSharper disable VBPossibleMistakenCallToGetType.2
                        constParamTypes.Add(constParam.GetType());
                    }
                    System.Reflection.ConstructorInfo constructor = oType.GetConstructor(constParamTypes.ToArray());
                    dValue = (constructor.Invoke(aParam) as IType).ToDecimal();
                }
            }
            return dValue;
        }

        public double TieBreak(int teamId, List<VsandGameReport> oGameReports, DateTime dStart, DateTime dEnd, string sDataType)
        {
            double dValue = 0;
            Type oType = System.Type.GetType(sDataType, false, true);
            if (oType != null)
            {
                Type oInt = oType.GetInterface("IPowerPoints");
                if (oInt != null)
                {
                    var aParam = new object[] { teamId, oGameReports, dStart, dEnd };
                    List<Type> constParamTypes = new List<Type>();
                    for (int iParam = 0; iParam <= aParam.Length - 1; iParam++)
                    {
                        object constParam = aParam[iParam];
                        // ReSharper disable VBPossibleMistakenCallToGetType.2
                        constParamTypes.Add(constParam.GetType());
                    }
                    System.Reflection.ConstructorInfo constructor = oType.GetConstructor(constParamTypes.ToArray());

                    dValue = (constructor.Invoke(aParam) as IPowerPoints).TieBreak();
                }
            }
            return dValue;
        }

        public int GetIncludeGames(string sDataType)
        {
            int iRet = 0;
            Type oType = System.Type.GetType(sDataType, false, true);
            if (oType != null)
            {
                Type oInt = oType.GetInterface("IPowerPoints");
                if (oInt != null)
                {
                    var aParam = new object[] { };
                    List<Type> constParamTypes = new List<Type>();
                    for (int iParam = 0; iParam <= aParam.Length - 1; iParam++)
                    {
                        object constParam = aParam[iParam];
                        // ReSharper disable VBPossibleMistakenCallToGetType.2
                        constParamTypes.Add(constParam.GetType());
                    }
                    System.Reflection.ConstructorInfo constructor = oType.GetConstructor(constParamTypes.ToArray());
                    iRet = (constructor.Invoke(aParam) as IPowerPoints).IncludeGames;
                }
            }
            return iRet;
        }

        public decimal SummarizeCalculate(int teamId, List<VsandGameReport> oGameReports, DateTime dStart, DateTime dEnd, int maxGames, string sDataType, ref int eligibleGames)
        {
            decimal dValue = 0;
            Type oType = Type.GetType(sDataType, false, true);
            if (oType != null)
            {
                Type oInt = oType.GetInterface("IType");
                if (oInt != null)
                {
                    var aParam = new object[] { teamId, oGameReports, dStart, dEnd, maxGames };
                    List<Type> constParamTypes = new List<Type>();
                    for (int iParam = 0; iParam <= aParam.Length - 1; iParam++)
                    {
                        object constParam = aParam[iParam];
                        // ReSharper disable VBPossibleMistakenCallToGetType.2
                        constParamTypes.Add(constParam.GetType());
                    }
                    System.Reflection.ConstructorInfo constructor = oType.GetConstructor(constParamTypes.ToArray());
                    IPowerPoints oInstPP = (constructor.Invoke(aParam) as IPowerPoints);
                    var oInstC = (oInstPP as IType);
                    dValue = oInstC.ToDecimal();
                    // -- Need to get the EligibleGames value property and put it in the correct variable
                    eligibleGames = oInstPP.EligibleGamesCount;
                }
            }
            return dValue;
        }

        public async Task<VsandPowerPointsConfig> GetPPConfig(int sportId, int scheduleYearId)
        {
            return await _uow.PowerPointsConfig.Single(ppc => ppc.SportId == sportId && ppc.ScheduleYearId == scheduleYearId);
        }

        public async Task<ServiceResult<VsandPowerPointsConfig>> SavePPConfig(int scheduleyearId, int sportId, int includeGamesCount, bool includeTieGames, int bestNGames, DateTime startDate, DateTime endDate, double winValue, double lossValue, double tieValue, double groupWinMultiplier, double groupLossMultiplier, double groupTieMultiplier, double residualWinMultiplierOppWins, double residualWinMultiplierOppLosses, double residualWinMultiplierOppTies, double residualLossMultiplierOppWins, double residualLossMultiplierOppLosses, double residualLossMultiplierOppTies, double residualTieMultiplierOppWins, double residualTieMultiplierOppLosses, double residualTieMultiplierOppTies, DateTime gracePeriodEndDate, DateTime seedingPeriodEndDate)
        {
            var oRet = new ServiceResult<VsandPowerPointsConfig>();

            var oConfig = _uow.PowerPointsConfig.Single(ppc => ppc.SportId == sportId && ppc.ScheduleYearId == scheduleyearId).Result;

            if (oConfig == null)
            {
                oConfig = new VsandPowerPointsConfig
                {
                    ScheduleYearId = scheduleyearId,
                    SportId = sportId,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    IncludeTieGames = true
                };

                await _uow.PowerPointsConfig.Insert(oConfig);
            }

            oConfig.IncludeGamesCount = includeGamesCount;
            oConfig.IncludeTieGames = includeTieGames;
            oConfig.BestNGames = bestNGames;
            oConfig.StartDate = startDate;
            oConfig.EndDate = endDate;
            oConfig.WinValue = winValue;
            oConfig.LossValue = lossValue;
            oConfig.TieValue = tieValue;
            oConfig.GroupWinMultiplier = groupWinMultiplier;
            oConfig.GroupLossMultiplier = groupLossMultiplier;
            oConfig.GroupTieMultiplier = groupTieMultiplier;

            oConfig.ResidualWinMultiplierOppWins = residualWinMultiplierOppWins;
            oConfig.ResidualWinMultiplierOppLosses = residualWinMultiplierOppLosses;
            oConfig.ResidualWinMultiplierOppTies = residualWinMultiplierOppTies;

            oConfig.ResidualLossMultiplierOppWins = residualLossMultiplierOppWins;
            oConfig.ResidualLossMultiplierOppLosses = residualLossMultiplierOppLosses;
            oConfig.ResidualLossMultiplierOppTies = residualLossMultiplierOppTies;

            oConfig.ResidualTieMultiplierOppWins = residualTieMultiplierOppWins;
            oConfig.ResidualTieMultiplierOppLosses = residualTieMultiplierOppLosses;
            oConfig.ResidualTieMultiplierOppTies = residualTieMultiplierOppTies;

            oConfig.SeedingPeriodEnd = seedingPeriodEndDate;
            oConfig.GracePeriodEnd = gracePeriodEndDate;

            try
            {
                await _uow.Save();
                oRet.Success = true;
            }
            catch (Exception ex)
            {
                oRet.Message = ex.Message;
                Log.Error(ex, ex.Message);
            }

            return oRet;
        }
    }
}
