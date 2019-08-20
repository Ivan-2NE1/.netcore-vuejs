using Microsoft.EntityFrameworkCore;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VSAND.Data.Entities;
using VSAND.Data.ViewModels;


namespace VSAND.Data.Repositories
{
    /// <summary>
    /// Read-Only Repository interface for Schedule Year Summary
    /// </summary>
    public interface IScheduleYearSummaryRepository
    {        
        List<ScheduleYearSummary> GetList(int scheduleYearId);
        Task<List<ScheduleYearSummary>> GetListAsync(int schduleYearId);
    }
}
