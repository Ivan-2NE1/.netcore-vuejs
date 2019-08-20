using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VSAND.Data.ViewModels;

namespace VSAND.Data.Repositories
{
    public interface IScheduleYearProvisioningSummaryRepository
    {
        List<ScheduleYearProvisioningSummary> GetList(int scheduleYearId, int sportId);
        Task<List<ScheduleYearProvisioningSummary>> GetListAsync(int scheduleYearId, int sportId);
    }
}
