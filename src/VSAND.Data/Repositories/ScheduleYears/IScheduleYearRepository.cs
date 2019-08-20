using System.Collections.Generic;
using VSAND.Data.Entities;
using VSAND.Data.Identity;
using VSAND.Data.ViewModels;
using System.Threading.Tasks;

namespace VSAND.Data.Repositories
{
    public interface IScheduleYearRepository : IRepository<VsandScheduleYear>
    {
        int GetActiveScheduleYearId();
        Task<int> GetActiveScheduleYearIdAsync();

        int GetLastScheduleYearId(int ScheduleYearId);

        string GetActiveScheduleYearName();

        string GetScheduleYearName(int ScheduleYearId);

        VsandScheduleYear GetScheduleYear();
        Task<VsandScheduleYear> GetScheduleYearAsync();

        VsandScheduleYear GetScheduleYear(int ScheduleYearId);

        List<VsandScheduleYear> GetScheduleYears();

        List<ScheduleYearSummary> GetScheduleYearSummary(int ScheduleYearId);

        int AddScheduleYear(int scheduleYearId, string Name, int endYear, ApplicationUser user);

        int AddScheduleYear(int scheduleYearId, string name, int endYear, ApplicationUser user, ref string sMsg);

        bool UpdateScheduleYear(int scheduleYearId, string name, int EndYear, ApplicationUser user);

        bool UpdateScheduleYear(int scheduleYearId, string name, int EndYear, ApplicationUser user, ref string sMsg);

        bool SetActiveScheduleYear(int ScheduleYearId, ApplicationUser user);

        bool SetActiveScheduleYear(int ScheduleYearId, ApplicationUser user, ref string sMsg);
    }
}
