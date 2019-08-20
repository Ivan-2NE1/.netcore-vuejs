using System.Collections.Generic;
using System.Threading.Tasks;
using VSAND.Data.Entities;
using VSAND.Data.Repositories;
using VSAND.Data.ViewModels;

namespace VSAND.Services.Data.Schools
{
    public interface ISchoolService
    {
        Task<List<ListItem<string>>> GetFrontEndDisplayListCachedAsync();
        Task<List<ListItem<string>>> GetInCoverageSchoolList();
        Task<IEnumerable<ListItem<int>>> AutocompleteAsync(string keyword);
        Task<ListItem<int>> AutocompleteRestoreAsync(int schoolId);
        Task<VsandSchool> GetSchoolAsync(int schoolId);
        Task<VsandSchool> GetFullSchoolAsync(int schoolId);
        Task<PagedResult<SchoolSummary>> SearchAsync(string name, string city, string state, bool coreCoverage, int pageSize, int pageNumber);
        Task<ServiceResult<VsandSchool>> AddSchoolAsync(VsandSchool addSchool);
        Task<ServiceResult<VsandSchool>> UpdateSchoolAsync(VsandSchool chgSchool);
        Task<ServiceResult<VsandSchool>> DeleteSchoolAsync(int schoolId);

        Task<ServiceResult<VsandSchoolCustomCode>> AddCustomCodeAsync(VsandSchoolCustomCode addCustomCode);
        Task<ServiceResult<VsandSchoolCustomCode>> UpdateCustomCodeAsync(VsandSchoolCustomCode chgCustomCode);
        Task<ServiceResult<VsandSchoolCustomCode>> DeleteCustomCodeAsync(int eventId);
    }
}
