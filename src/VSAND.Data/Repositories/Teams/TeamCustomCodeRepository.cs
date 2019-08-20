using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VSAND.Data.Entities;

namespace VSAND.Data.Repositories
{
    public class TeamCustomCodeRepository : Repository<VsandTeamCustomCode>, ITeamCustomCodeRepository
    {
        private readonly VsandContext _context;
        public TeamCustomCodeRepository(VsandContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<string>> GetUniqueValues(string codeName)
        {
            var oRet = await (from tc in _context.VsandTeamCustomCode where tc.CodeName.Equals(codeName) select tc.CodeValue).Distinct().OrderBy(tc => tc).ToListAsync();
            return oRet;
        }
    }
}
