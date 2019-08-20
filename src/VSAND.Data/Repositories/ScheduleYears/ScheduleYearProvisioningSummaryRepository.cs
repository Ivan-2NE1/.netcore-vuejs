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
    public class ScheduleYearProvisioningSummaryRepository : IScheduleYearProvisioningSummaryRepository
    {
        private readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly VsandContext _context;
        public ScheduleYearProvisioningSummaryRepository(VsandContext context)
        {
            _context = context;
        }

        public List<ScheduleYearProvisioningSummary> GetList(int scheduleYearId, int sportId)
        {
            return GetListAsync(scheduleYearId, sportId).Result;
        }

        public async Task<List<ScheduleYearProvisioningSummary>> GetListAsync(int scheduleYearId, int sportId)
        {
            // Need to get previous schedule year to get counts
            var refEndYear = (from sy in _context.VsandScheduleYear
                              where sy.ScheduleYearId == scheduleYearId
                              select sy.EndYear).FirstOrDefault();

            var prevScheduleYearId = (from sy in _context.VsandScheduleYear
                               where sy.EndYear == refEndYear - 1
                               select sy.ScheduleYearId).FirstOrDefault();

            var oRet = await (from s in _context.VsandSchool
                              orderby s.Name ascending
                              select new ScheduleYearProvisioningSummary()
                              {
                                  SchoolId = s.SchoolId,
                                  CoreCoverage = s.CoreCoverage,
                                  Name = s.Name,
                                  City = s.City,
                                  State = s.State,
                                  Validated = s.Validated,
                                  PreviousSeasonGameCount = (from grt in _context.VsandGameReportTeam
                                                             where grt.Team.SchoolId == s.SchoolId
                                                             && grt.Team.SportId == sportId
                                                             && grt.Team.ScheduleYearId == prevScheduleYearId select grt.GameReportId).Count(),
                                  PreviousSeasonRosterCount = (from tr in _context.VsandTeamRoster
                                                               where tr.Team.SchoolId == s.SchoolId
                                                               && tr.Team.SportId == sportId 
                                                               && tr.Team.ScheduleYearId == prevScheduleYearId select tr.RosterId).Count(),
                                  CurrentSeasonTeamId = (from t in _context.VsandTeam
                                                         where t.SchoolId == s.SchoolId 
                                                         && t.SportId == sportId 
                                                         && t.ScheduleYearId == scheduleYearId select t.TeamId).FirstOrDefault(),
                                  CurrentSeasonGameCount = (from grt in _context.VsandGameReportTeam
                                                            where grt.Team.SchoolId == s.SchoolId 
                                                            && grt.Team.SportId == sportId 
                                                            && grt.Team.ScheduleYearId == scheduleYearId select grt.GameReportId).Count(),
                                  CurrentSeasonRosterCount = (from tr in _context.VsandTeamRoster
                                                              where tr.Team.SchoolId == s.SchoolId
                                                              && tr.Team.SportId == sportId
                                                              && tr.Team.ScheduleYearId == scheduleYearId
                                                              select tr.RosterId).Count()
                              }).ToListAsync();
            return oRet;
        }
    }
}
