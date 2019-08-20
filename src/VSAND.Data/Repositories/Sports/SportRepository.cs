using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using VSAND.Data.Entities;
using VSAND.Data.ViewModels;

namespace VSAND.Data.Repositories
{
    public class SportRepository : Repository<VsandSport>, ISportRepository
    {
        private readonly VsandContext _context;
        public SportRepository(VsandContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ListItem<int>> SportItem(int sportId)
        {
            var oRet = await (from s in _context.VsandSport
                              where s.SportId == sportId
                              select new ListItem<int>
                              {
                                  id = s.SportId,
                                  name = s.Name
                              }).FirstOrDefaultAsync();

            return oRet;
        }
    }
}
