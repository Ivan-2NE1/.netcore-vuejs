using System.Collections.Generic;
using System.Threading.Tasks;
using VSAND.Data.Entities;
using VSAND.Data.ViewModels;

namespace VSAND.Services.Data.Manage.Counties
{
    public interface ICountyService
    {
        Task<IEnumerable<ListItem<int>>> GetList();
        Task<VsandCounty> GetCountyAsync(int CountyId);
        Task<ServiceResult<VsandCounty>> AddCountyAsync(VsandCounty addCounty);
        Task<ServiceResult<VsandCounty>> UpdateCountyAsync(VsandCounty chgCounty);
        Task<ServiceResult<VsandCounty>> DeleteCountyAsync(int countyId);
        Task<List<VsandCounty>> GetListAsync();
    }
}
