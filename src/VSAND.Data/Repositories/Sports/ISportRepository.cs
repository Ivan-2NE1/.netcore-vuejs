using System.Threading.Tasks;
using VSAND.Data.Entities;
using VSAND.Data.ViewModels;

namespace VSAND.Data.Repositories
{
    public interface ISportRepository : IRepository<VsandSport>
    {
        Task<ListItem<int>> SportItem(int sportId);
    }
}
