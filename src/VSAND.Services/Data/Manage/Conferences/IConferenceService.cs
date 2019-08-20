using System.Collections.Generic;
using System.Threading.Tasks;
using VSAND.Data.Entities;
using VSAND.Data.ViewModels;

namespace VSAND.Services.Data.Manage.Conferences
{
    public interface IConferenceService
    {
        Task<VsandConference> GetConferenceAsync(int ConferenceId);
        Task<ServiceResult<VsandConference>> AddConferenceAsync(VsandConference addConference);
        Task<ServiceResult<VsandConference>> UpdateConferenceAsync(VsandConference chgConference);
        Task<ServiceResult<VsandConference>> DeleteConferenceAsync(int ConferenceId);
        Task<List<VsandConference>> GetListAsync();
    }
}
