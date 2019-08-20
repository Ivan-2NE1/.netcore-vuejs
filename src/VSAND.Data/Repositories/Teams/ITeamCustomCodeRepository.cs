using System.Collections.Generic;
using System.Threading.Tasks;
using VSAND.Data.Entities;

namespace VSAND.Data.Repositories
{
    public interface ITeamCustomCodeRepository : IRepository<VsandTeamCustomCode>
    {
        Task<IEnumerable<string>> GetUniqueValues(string codeName);
    }
}
