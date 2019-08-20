using System.Collections.Generic;
using System.Threading.Tasks;
using VSAND.Data.Entities;
using VSAND.Data.ViewModels.Users;

namespace VSAND.Data.Repositories
{
    public interface IAppxUserRoleRepository : IRepository<AppxUserRole>
    {
        Task<List<AppxUserRoleCategory>> GetRoleCategoriesAsync();
    }
}
