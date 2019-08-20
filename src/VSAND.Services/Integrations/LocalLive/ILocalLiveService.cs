using System.Collections.Generic;
using System.Threading.Tasks;
using VSAND.Data.Entities;
using VSAND.Data.Integrations.LocalLive;
using VSAND.Data.ViewModels;

namespace VSAND.Services.Integrations.LocalLive
{
    public interface ILocalLiveService
    {
        Task<List<Event>> GetAllEventsAsync(string include);
        Task<ServiceResult<List<LocalLiveEvent>>> AddEventsAsync(List<Event> events);
        Task<ServiceResult<List<LocalLiveEvent>>> AddEventsAsync(List<LocalLiveEvent> events);
    }
}
