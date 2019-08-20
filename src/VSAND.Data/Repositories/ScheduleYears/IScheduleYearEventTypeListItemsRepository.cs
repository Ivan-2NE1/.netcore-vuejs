using System.Collections.Generic;
using System.Threading.Tasks;
using VSAND.Data.ViewModels;

namespace VSAND.Data.Repositories
{
    public interface IScheduleYearEventTypeListItemsRepository
    {
        List<EventTypeListItem> GetEventTypeObjects(int sportId, int scheduleYearId);
        Task<List<EventTypeListItem>> GetEventTypeObjectsAsync(int sportId, int scheduleYearId);
        List<EventTypeListItem> GetActiveEventTypeObjects(int sportId, int scheduleYearId);
        Task<List<EventTypeListItem>> GetActiveEventTypeObjectsAsync(int sportId, int scheduleYearId);
    }
}
