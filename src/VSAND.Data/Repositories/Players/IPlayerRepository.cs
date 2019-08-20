using System.Threading.Tasks;
using VSAND.Data.Entities;
using VSAND.Data.ViewModels;

namespace VSAND.Data.Repositories
{
    public interface IPlayerRepository : IRepository<VsandPlayer>
    {
        Task<PagedResult<PlayerSummary>> Search(string firstName, string lastName, int graduationYear, int schoolId, int pageSize, int pageNumber);
        Task<VsandPlayer> GetPlayerStatsAsync(int playerId, int teamId);
    }
}
