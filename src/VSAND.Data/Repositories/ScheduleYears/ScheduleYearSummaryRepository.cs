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
    public class ScheduleYearSummaryRepository : IScheduleYearSummaryRepository
    {
        private readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly VsandContext _context;
        public ScheduleYearSummaryRepository(VsandContext context)
        {
            _context = context;
        }

        public List<ScheduleYearSummary> GetList(int scheduleYearId)
        {
            return GetListAsync(scheduleYearId).Result;
        }

        public async Task<List<ScheduleYearSummary>> GetListAsync(int scheduleYearId)
        {
            return await GetListQuery(scheduleYearId).ToListAsync();
        }

        private IQueryable<ScheduleYearSummary> GetListQuery(int scheduleYearId)
        {
            var oQuery = (from s in _context.VsandSport
                    select new ScheduleYearSummary()
                    {
                        SportId = s.SportId,
                        Sport = s.Name,
                        Season = s.Season,
                        TeamCount = s.Teams.Count(t => t.ScheduleYearId == scheduleYearId),
                        EventCount = s.EventTypes.Count(e => e.ScheduleYearId == scheduleYearId),
                        EnablePowerPoints = s.EnablePowerPoints ?? false,
                        HasPowerPointsConfig = s.PowerPointsConfigs.Any(p => p.ScheduleYearId == scheduleYearId),
                        HasLeagueRulesConfig = s.LeagueRules.Any(r => r.ScheduleYearId == scheduleYearId)
                    });
            return oQuery;
        }
    }
}
