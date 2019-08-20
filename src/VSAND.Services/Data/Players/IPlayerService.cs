using System.Threading.Tasks;
using System.Collections.Generic;
using VSAND.Data.Entities;
using VSAND.Data.Repositories;
using VSAND.Data.ViewModels;
using VSAND.Data.ViewModels.Players;
using VSAND.Data.ViewModels.Teams;

namespace VSAND.Services.Data.Players
{
    public interface IPlayerService
    {
        Task<VsandPlayer> GetPlayerAsync(int playerId);
        Task<VsandPlayer> GetPlayerTeamsAsync(int playerId);
        Task<PlayerView> GetFullPlayerViewCachedAsync(int playerId, int? viewSportId, int? viewScheduleYearId);
        Task<PagedResult<PlayerSummary>> Search(string firstName, string lastName, int graduationYear, int schoolId, int pageSize, int pageNumber);
        Task<VsandPlayer> GetPlayerStatsAsync(int playerId, int teamId);
        Task<ServiceResult<VsandPlayer>> AddPlayerAsync(VsandPlayer addPlayer);
        Task<ServiceResult<VsandPlayer>> UpdatePlayerAsync(VsandPlayer chgPlayer);
        Task<ServiceResult<VsandPlayer>> DeletePlayerAsync(int playerId);
        Task<List<VsandPlayer>> GetListAsync(int teamid);
    }
}
