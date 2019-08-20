using System.Collections.Generic;
using System.Threading.Tasks;
using VSAND.Data.Entities;
using VSAND.Data.Identity;
using VSAND.Data.ViewModels;
using VSAND.Data.ViewModels.ScheduleYears;

namespace VSAND.Services.Data.Manage.ScheduleYears
{
    public interface IScheduleYearService
    {
        Task<IEnumerable<ListItem<int>>> GetList();
        Task<IEnumerable<ScheduleYear>> List();
        Task<VsandScheduleYear> GetScheduleYear(int scheduleYearId);
        Task<ScheduleYear> GetSummary(int scheduleYearId);
        Task<ScheduleYear> GetProvisioning(int scheduleYearId, int sportId);
        Task<ScheduleYear> GetActiveScheduleYear();
        Task<ScheduleYear> GetActiveScheduleYearCachedAsync();
        int GetLastScheduleYearId(int scheduleYearId);
        Task<ServiceResult<ScheduleYear>> AddScheduleYearAsync(ScheduleYear scheduleYear);
        Task<ServiceResult<ScheduleYear>> SetActiveScheduleYearAsync(int scheduleYearId);

        Task<VsandPowerPointsConfig> GetPowerPointsConfig(int scheduleYearId, int sportId);
        Task<ServiceResult<VsandPowerPointsConfig>> InsertOrUpdatePowerPoints(VsandPowerPointsConfig chgConfig);

        Task<List<VsandLeagueRule>> GetLeagueRulesAsync(int scheduleYearId, int sportId);
        Task<ServiceResult<LeagueRule>> UpdateLeagueRule(LeagueRule leagueRuleVm);
        Task<ServiceResult<VsandLeagueRule>> PopulateLeagueRulesFromPreviousYear(int scheduleYearId, int sportId);
        Task<ServiceResult<VsandLeagueRule>> PopulateLeagueRulesFromExistingData(int scheduleYearId, int sportId);

        Task<List<VsandScheduleLoadFile>> GetScheduleFiles(int scheduleYearId, int sportId);

        Task<ServiceResult<VsandScheduleLoadFile>> UploadScheduleFile(int scheduleYearId, int sportId, string fileName, string fileExt, long fileSize);
        Task ImportScheduleFile(int scheduleYearId, int sportId, int fileId, List<ScheduleLoadFileRow> scheduleData);
        Task<VsandScheduleLoadFile> GetScheduleFileRecord(int fileId);
        Task<List<ScheduleFileResolveItem>> GetScheduleFile(int scheduleYearId, int sportId, int fileId);
        Task CommitScheduleFile(AppxUser user, int scheduleYearId, int sportId, int fileId);

        Task<ServiceResult<bool>> ResolveScheduleItem(ApplicationUser user, int fileId, ScheduleFileResolvedItem resolved);
    }
}
