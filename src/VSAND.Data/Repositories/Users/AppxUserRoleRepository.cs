using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VSAND.Data.Entities;
using VSAND.Data.ViewModels.Users;

namespace VSAND.Data.Repositories
{
    public class AppxUserRoleRepository : Repository<AppxUserRole>, IAppxUserRoleRepository
    {
        private readonly VsandContext _context;
        public AppxUserRoleRepository(VsandContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<AppxUserRoleCategory>> GetRoleCategoriesAsync()
        {
            var categories = await _context.AppxUserRole
                .Where(r => r.RoleName != ".")
                .GroupBy(r => r.RoleCat)
                .Select(g => new AppxUserRoleCategory
                {
                    RoleCat = g.Key,
                    Roles = g.ToList()
                }).OrderBy(c => c.RoleCat).ToListAsync();

            // make the user function category appear first in the list
            int userFunctionIdx = categories.FindIndex(c => c.RoleCat == "UserFunction");
            var userFunctionCategory = categories.ElementAt(userFunctionIdx);

            categories.RemoveAt(userFunctionIdx);
            categories.Insert(0, userFunctionCategory);

            return categories;
        }
    }
}
