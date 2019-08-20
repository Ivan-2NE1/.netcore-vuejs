using System.Collections.Generic;
using System.Threading.Tasks;
using VSAND.Data.Entities;
using VSAND.Data.ViewModels;
using VSAND.Data.Repositories;
using VSAND.Data.ViewModels.Teams;

namespace VSAND.Services.Data.Teams
{
    public interface ITeamService
    {
        Task<List<ListItem<int>>> AutocompleteAsync(string k, int sportId, int scheduleYearId);
        Task<ListItem<int>> AutocompleteRestoreAsync(int teamId);
        Task<PagedResult<TeamSummary>> GetTeamsAsync(SearchRequest searchRequest);
        Task<List<ListItem<int>>> CoreCoverageListAsync(int sportId, int scheduleYearId);
        Task<VsandTeam> GetTeamAsync(int teamId);
        Task<VsandTeam> GetTeamAsync(int schoolId, int sportId, int scheduleYearId);
        FullTeam GetFullTeam(int schoolId, int sportId, int scheduleYearId);
        Task<FullTeam> GetFullTeamAsync(int schoolId, int sportId, int scheduleYearId);
        Task<FullTeam> GetFullTeamCachedAsync(int schoolId, int sportId, int scheduleYearId);
        Task<VsandTeam> GetTeamDistribution(int teamId);
        Task<VsandTeam> GetTeamRoster(int teamId);
        Task<List<VsandSportPosition>> GetPositionsAsync(int sportId);
        Task<int> GetTeamIdAsync(int schoolId, int sportId, int scheduleYearId);
        Task ProvisionTeams(AppxUser user, int scheduleYearId, int sportId, List<int> schoolIds);
        Task<List<ListItem<string>>> GetUniqueCustomCodeValues(string codeName);
        Task<int> AddTeamAsync(AppxUser user, int scheduleYearId, int sportId, int schoolId);
        Task<List<TeamRoster>> GetRoster(int teamid);
        Task<ServiceResult<VsandTeamRoster>> SaveRoster(VsandTeamRoster teamRoster);
        Task<ServiceResult<VsandTeamRoster>> DeleteRoster(int rosterId);
        Task<TeamOosRecord> GetOosRecord(int teamId);
        Task<ServiceResult<TeamOosRecord>> SaveOosRecord(TeamOosRecord teamRecord);
        Task<List<TeamCustomCode>> GetTeamCustomCodes(int teamId);
        Task<ServiceResult<VsandTeamCustomCode>> AddCustomCodeAsync(VsandTeamCustomCode addCustomCode);
        Task<ServiceResult<VsandTeamCustomCode>> UpdateCustomCodeAsync(VsandTeamCustomCode chgCustomCode);
        Task<ServiceResult<VsandTeamCustomCode>> DeleteCustomCodeAsync(int customCodeId);
        Task<ServiceResult<bool>> BulkSaveTeamCustomCodes(int sportId, int scheduleYearId, List<VsandTeamCustomCode> codes);
    }
}
