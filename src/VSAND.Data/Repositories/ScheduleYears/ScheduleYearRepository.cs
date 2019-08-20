using Microsoft.EntityFrameworkCore;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VSAND.Data.Entities;
using VSAND.Data.Identity;
using VSAND.Data.ViewModels;

namespace VSAND.Data.Repositories
{
    public class ScheduleYearRepository : Repository<VsandScheduleYear>, IScheduleYearRepository
    {
        private readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly VsandContext _context;
        public ScheduleYearRepository(VsandContext context) : base(context)
        {
            _context = context;
        }

        public int GetActiveScheduleYearId()
        {
            return GetActiveScheduleYearIdAsync().Result;
        }

        public async Task<int> GetActiveScheduleYearIdAsync()
        {
            int iActive = 0;

            iActive = await (from sy in _context.VsandScheduleYear
                       where sy.Active.Value
                       select sy.ScheduleYearId).FirstOrDefaultAsync();

            return iActive;
        }

        public int GetLastScheduleYearId(int ScheduleYearId)
        {
            int iRet = 0;

            VsandScheduleYear oSY = (from ly in _context.VsandScheduleYear
                                     where ly.EndYear == (from sy in _context.VsandScheduleYear
                                                          where sy.ScheduleYearId == ScheduleYearId
                                                          select sy).FirstOrDefault().EndYear - 1
                                     select ly).FirstOrDefault();

            if (oSY != null)
            {
                iRet = oSY.ScheduleYearId;
            }

            return iRet;
        }

        public string GetActiveScheduleYearName()
        {
            int iActiveId = GetActiveScheduleYearId();

            return (from sy in _context.VsandScheduleYear
                    where sy.ScheduleYearId == iActiveId
                    select sy).First().Name;
        }

        public string GetScheduleYearName(int ScheduleYearId)
        {
            string sName = "";
            VsandScheduleYear oSY = GetScheduleYear(ScheduleYearId);
            if (oSY != null)
            {
                sName = oSY.Name;
            }
            return sName;
        }

        public VsandScheduleYear GetScheduleYear()
        {
            return GetScheduleYear(GetActiveScheduleYearId());
        }

        public async Task<VsandScheduleYear> GetScheduleYearAsync()
        {
            int scheduleYearId = await GetActiveScheduleYearIdAsync();
            return await _context.VsandScheduleYear.FirstOrDefaultAsync(sy => sy.ScheduleYearId == scheduleYearId);
        }

        public VsandScheduleYear GetScheduleYear(int ScheduleYearId)
        {
            return (from sy in _context.VsandScheduleYear
                    where sy.ScheduleYearId == ScheduleYearId
                    select sy).FirstOrDefault();
        }

        public List<VsandScheduleYear> GetScheduleYears()
        {
            return (from s in _context.VsandScheduleYear
                    orderby s.Active descending, s.Name descending
                    select s).ToList();
        }

        public List<ScheduleYearSummary> GetScheduleYearSummary(int ScheduleYearId)
        {
            List<ScheduleYearSummary> oRet = null;

            IEnumerable<ScheduleYearSummary> oData = (from s in _context.VsandSport
                                                      where s.Enabled == true
                                                      select new ScheduleYearSummary()
                                                      {
                                                          SportId = s.SportId,
                                                          Sport = s.Name,
                                                          EnablePowerPoints = s.EnablePowerPoints ?? false,
                                                          Season = s.Season,
                                                          TeamCount = s.Teams.Where(t => t.ScheduleYear.ScheduleYearId == ScheduleYearId).Count(),
                                                          EventCount = s.EventTypes.Where(et => et.ScheduleYear.ScheduleYearId == ScheduleYearId).Count()
                                                      });

            if (oData != null)
            {
                oRet = oData.OrderBy(s => s.Season).ThenBy(s => s.Sport).ToList();
            }

            return oRet;
        }

        public int AddScheduleYear(int scheduleYearId, string Name, int endYear, ApplicationUser user)
        {
            string sMsg = "";
            return AddScheduleYear(scheduleYearId, Name, endYear, user, ref sMsg);
        }

        public int AddScheduleYear(int scheduleYearId, string name, int endYear, ApplicationUser user, ref string sMsg)
        {
            int iRet = 0;

            string sUser = user.AppxUser.UserId;
            int iUser = user.AppxUser.AdminId;

            VsandScheduleYear oSY = new VsandScheduleYear
            {
                Name = name,
                EndYear = endYear,
                Active = false
            };

            _context.VsandScheduleYear.Add(oSY);

            try
            {
                _context.SaveChanges();

                iRet = oSY.ScheduleYearId;

                AuditRepository.AuditChange(_context, "vsand_ScheduleYear", "ScheduleYearId", iRet, "Insert", sUser, iUser);
            }
            catch (Exception ex)
            {
                sMsg = ex.Message;

                Log.Error(ex, ex.Message);
            }

            return iRet;
        }

        public bool UpdateScheduleYear(int scheduleYearId, string name, int EndYear, ApplicationUser user)
        {
            string sMsg = "";
            return UpdateScheduleYear(scheduleYearId, name, EndYear, user, ref sMsg);
        }

        public bool UpdateScheduleYear(int scheduleYearId, string name, int EndYear, ApplicationUser user, ref string sMsg)
        {
            bool bRet = false;

            string sUser = user.AppxUser.UserId;
            int iUser = user.AppxUser.AdminId;

            VsandScheduleYear oSY = (from sy in _context.VsandScheduleYear
                                     where sy.ScheduleYearId == scheduleYearId
                                     select sy).FirstOrDefault();

            if (oSY != null)
            {
                oSY.Name = name;
                oSY.EndYear = EndYear;

                try
                {
                    AuditRepository.AuditChange(_context, "vsand_ScheduleYear", "ScheduleYearId", scheduleYearId, "Update", sUser, iUser);
                    _context.SaveChanges();

                    bRet = true;
                }
                catch (Exception ex)
                {
                    Log.Error(ex, ex.Message);
                    sMsg = ex.Message;
                }
            }
            else
            {
                sMsg = "The selected schedule year cannot be located. Perhaps it has been deleted?";
            }

            return bRet;
        }

        public bool SetActiveScheduleYear(int ScheduleYearId, ApplicationUser user)
        {
            string sMsg = "";
            return SetActiveScheduleYear(ScheduleYearId, user, ref sMsg);
        }

        public bool SetActiveScheduleYear(int ScheduleYearId, ApplicationUser user, ref string sMsg)
        {
            bool bRet = false;

            string sUser = user.AppxUser.UserId;
            int iUser = user.AppxUser.AdminId;

            List<int> aChanged = new List<int>();

            IEnumerable<VsandScheduleYear> oActive = (from sy in _context.VsandScheduleYear
                                                      where sy.Active.Value
                                                      select sy);

            foreach (var oASY in oActive)
            {
                oASY.Active = false;
                aChanged.Add(oASY.ScheduleYearId);
            }

            VsandScheduleYear oSY = (from sy in _context.VsandScheduleYear
                                     where sy.ScheduleYearId == ScheduleYearId
                                     select sy).FirstOrDefault();

            if (oSY != null)
            {
                oSY.Active = true;
                aChanged.Add(ScheduleYearId);

                for (int iChanged = 0; iChanged <= aChanged.Count - 1; iChanged++)
                {
                    AuditRepository.AuditChange(_context, "vsand_ScheduleYear", "ScheduleYearId", aChanged[iChanged], "Update", sUser, iUser);
                }

                try
                {
                    _context.SaveChanges();

                    bRet = true;
                }
                catch (Exception ex)
                {
                    sMsg = ex.Message;
                    Log.Error(ex, ex.Message);
                }
            }
            else
            {
                sMsg = "Cannot locate the selected schedule year. Perhaps it has been deleted?";
            }

            return bRet;
        }
    }
}
