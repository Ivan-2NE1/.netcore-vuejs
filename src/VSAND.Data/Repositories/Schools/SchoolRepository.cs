using NLog;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using VSAND.Data.Entities;
using VSAND.Data.Identity;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using VSAND.Data.ViewModels;

namespace VSAND.Data.Repositories
{
    public class SchoolRepository : Repository<VsandSchool>, ISchoolRepository
    {
        private readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly VsandContext _context;
        public SchoolRepository(VsandContext context) : base(context)
        {
            _context = context ?? throw new ArgumentException("Context is null");
        }

        public List<VsandSchool> GetSchoolList()
        {
            return (from s in _context.VsandSchool
                    orderby s.Name ascending
                    select s).ToList();
        }

        public List<VsandSchool> GetSchoolList(string Keyword)
        {
            List<VsandSchool> oDs = new List<VsandSchool>();

            if (!string.IsNullOrEmpty(Keyword))
            {
                oDs = _context.VsandSchool
                    .Where(oS => oS.Name.Contains(Keyword) || oS.Nickname.Contains(Keyword))
                    .ToList();
            }

            return oDs;
        }

        public VsandSchool GetSchool(int SchoolId)
        {
            return (from s in _context.VsandSchool.Include(s => s.County).Include(s => s.Editions)
                    where s.SchoolId == SchoolId
                    select s).FirstOrDefault();
        }

        public VsandSchool GetReviewSchool(int SchoolId)
        {
            VsandSchool oSchool = null;

            oSchool = (from s in _context.VsandSchool.Include(s => s.Teams)
                       where s.SchoolId == SchoolId
                       select s).FirstOrDefault();

            return oSchool;
        }

        public int GetSchoolIdByName(string Name)
        {
            int iRet = 0;

            VsandSchool oSchool = (from s in _context.VsandSchool
                                   where s.Name == Name
                                   select s).First();

            if (oSchool != null)
            {
                iRet = oSchool.SchoolId;
            }

            return iRet;
        }

        public int GetSchoolIdByTeam(int TeamId)
        {
            int iRet = 0;

            VsandSchool oSchool = (from s in _context.VsandSchool
                                   where s.Teams.Any(t => t.TeamId == TeamId)
                                   select s).FirstOrDefault();

            if (oSchool != null)
            {
                iRet = oSchool.SchoolId;
            }

            return iRet;
        }

        public int GetSchoolIdByName(string sSchool, string sState)
        {
            int iRet = 0;

            VsandSchool oSchool = (from s in _context.VsandSchool
                                   where s.Name == sSchool && s.State == sState
                                   select s).FirstOrDefault();

            if (oSchool != null)
            {
                iRet = oSchool.SchoolId;
            }

            return iRet;
        }

        public string GetSchoolName(int SchoolId)
        {
            string sRet = "";

            if (SchoolId > 0)
            {
                VsandSchool oRet = (from s in _context.VsandSchool
                                    where s.SchoolId == SchoolId
                                    select s).FirstOrDefault();
                if (oRet != null)
                {
                    sRet = oRet.Name;
                }
            }

            return sRet;
        }

        public int AddSchool(string Name, string AltName, string Address1, string Address2, string City, string State, string ZipCode, string PhoneNumber, bool PrivateSchool,
            int CountyId, string Nickname, string Mascot, string Color1, string Color2, string Color3, string Url, string Graphic, bool GraphicApproved, ref string sMsg, ApplicationUser user)
        {
            int iRet = 0;

            string Username = user.AppxUser.UserId;
            int UserId = user.AppxUser.AdminId;


            VsandSchool oSchool = (from s in _context.VsandSchool
                                   where s.Name == Name && s.State == State
                                   select s).FirstOrDefault();

            if (oSchool == null)
            {
                oSchool = new VsandSchool
                {
                    Name = Name,
                    PrivateSchool = PrivateSchool,
                    AltName = AltName,
                    Address1 = Address1,
                    Address2 = Address2,
                    City = City,
                    State = State,
                    ZipCode = ZipCode,
                    PhoneNumber = PhoneNumber,
                    Nickname = Nickname,
                    Mascot = Mascot,
                    Color1 = Color1,
                    Color2 = Color2,
                    Color3 = Color3,
                    Url = Url,
                    Graphic = Graphic,
                    GraphicApproved = GraphicApproved,
                    Validated = true,
                    ValidatedById = UserId,
                    ValidatedBy = Username
                };

                if (CountyId > 0)
                {
                    oSchool.CountyId = CountyId;
                }

                _context.VsandSchool.Add(oSchool);

                try
                {
                    _context.SaveChanges();

                    iRet = oSchool.SchoolId;
                }
                catch (Exception ex)
                {
                    sMsg = ex.Message;
                    Log.Error(ex, sMsg);
                }
            }
            else
            {
                sMsg = "A school with the same name already exists.";
            }

            if (iRet > 0)
            {
                // TODO: event remediation
                // VSAND.Events.School.RaiseSchoolUpdated(new object(), new VSAND.Events.School.SchoolUpdatedEventArgs(iRet, UserId));
            }

            return iRet;
        }

        public int SchoolQuickAdd(string Name, string State)
        {
            return SchoolQuickAddAsync(Name, State).Result;
        }

        public async Task<int> SchoolQuickAddAsync(string Name, string State)
        {
            int iRet = 0;

            VsandSchool oSchool = new VsandSchool
            {
                Name = Name,
                State = State
            };

            await _context.VsandSchool.AddAsync(oSchool);

            try
            {
                await _context.SaveChangesAsync();

                iRet = oSchool.SchoolId;
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
            }

            return iRet;
        }

        public bool UpdateSchool(int SchoolId, string Name, string AltName, string Address1, string Address2, string City, string State, string ZipCode, string PhoneNumber, bool PrivateSchool,
            int CountyId, string Nickname, string Mascot, string Color1, string Color2, string Color3, string Url, string Graphic, bool GraphicApproved, ApplicationUser user)
        {
            return UpdateSchool(SchoolId, Name, AltName, Address1, Address2, City, State, ZipCode, PhoneNumber, PrivateSchool, CountyId, Nickname, Mascot, Color1, Color2, Color3, Url, Graphic, GraphicApproved, false, user);
        }

        public bool UpdateSchool(int SchoolId, string Name, string AltName, string Address1, string Address2, string City, string State, string ZipCode, string PhoneNumber, bool PrivateSchool,
            int CountyId, string Nickname, string Mascot, string Color1, string Color2, string Color3, string Url, string Graphic, bool GraphicApproved, bool ForceReapplyName, ApplicationUser user)
        {
            bool bSaved = false;
            VsandSchool oSchool = null;

            string Username = user.AppxUser.UserId;
            int UserId = user.AppxUser.AdminId;
            AuditRepository.AuditChange(_context, "vsand_School", "SchoolId", SchoolId, "Update", Username, UserId);

            List<int> aGames = new List<int>();

            oSchool = (from s in _context.VsandSchool
                       where s.SchoolId == SchoolId
                       select s).First();

            if (oSchool != null)
            {
                if (oSchool.Name != Name)
                {
                    ForceReapplyName = true;
                }

                oSchool.Name = Name;
                oSchool.AltName = AltName;
                oSchool.Address1 = Address1;
                oSchool.Address2 = Address2;
                oSchool.City = City;
                oSchool.State = State;
                oSchool.ZipCode = ZipCode;
                oSchool.PhoneNumber = PhoneNumber;
                oSchool.PrivateSchool = PrivateSchool;
                oSchool.Nickname = Nickname;
                oSchool.Mascot = Mascot;
                oSchool.Color1 = Color1;
                oSchool.Color2 = Color2;
                oSchool.Color3 = Color3;
                oSchool.Url = Url;
                oSchool.Graphic = Graphic;
                oSchool.GraphicApproved = GraphicApproved;

                if (CountyId > 0)
                {
                    oSchool.CountyId = CountyId;
                }

                if (ForceReapplyName)
                {
                    // -- Get all active teams
                    IEnumerable<VsandTeam> oTeamsQ = (from t in _context.VsandTeam.Include(t => t.GameReportEntries)
                                                      where t.School.SchoolId == SchoolId && t.ScheduleYear.Active == true
                                                      select t);

                    foreach (VsandTeam oTeam in oTeamsQ)
                    {
                        oTeam.Name = Name;
                        foreach (VsandGameReportTeam oGameTeam in oTeam.GameReportEntries)
                        {
                            oGameTeam.TeamName = Name;

                            aGames.Add(oGameTeam.GameReportId);
                        }
                    }
                }

                try
                {
                    _context.SaveChanges();
                    bSaved = true;
                }
                catch (Exception ex)
                {
                    Log.Error(ex, ex.Message);
                }
            }

            if (ForceReapplyName && aGames.Count > 0 && bSaved)
            {
                foreach (int GameReportId in aGames)
                {
                    // TODO: update game report name from the controller, not the repository
                    // _gameReportRepository.UpdateGameReportName(GameReportId);
                }
            }

            if (bSaved)
            {
                // TODO: event remediation
                // VsandEvents.School.RaiseSchoolUpdated(new object(), new VSAND.Events.School.SchoolUpdatedEventArgs(SchoolId, UserId));
            }

            return bSaved;
        }

        public bool DeleteSchool(int SchoolId, int UserId, string Username, ref string sMsg)
        {
            bool bRet = false;

            int iTeamCount = (from t in _context.VsandTeam
                              where t.School.SchoolId == SchoolId
                              select t).Count();
            if (iTeamCount == 0)
            {
                // -- Remove Players
                IEnumerable<VsandPlayer> oPlayers = (from p in _context.VsandPlayer
                                                     where p.School.SchoolId == SchoolId
                                                     select p);
                foreach (VsandPlayer oPlayer in oPlayers)
                {
                    _context.VsandPlayer.Remove(oPlayer);
                }

                VsandSchool oSchool = (from s in _context.VsandSchool
                                       where s.SchoolId == SchoolId
                                       select s).FirstOrDefault();

                if (oSchool != null)
                {
                    _context.VsandSchool.Remove(oSchool);
                }

                try
                {
                    _context.SaveChanges();
                    bRet = true;
                }
                catch (Exception ex)
                {
                    sMsg = ex.Message;
                    Log.Error(ex, sMsg);
                }
            }
            else
            {
                sMsg = "The school has teams attached.";
            }

            if (bRet)
            {
                // TODO: event remediation
                // VSAND.Events.School.RaiseSchoolDeleted(new object(), new VSAND.Events.School.SchoolUpdatedEventArgs(SchoolId));
            }

            return bRet;
        }

        public List<ListItem<int>> FindPossibleAltSchoolNameList(string schoolName)
        {
            if (!_context.Database.IsSqlServer())
            {
                return null;
            }

            var oRet = new List<ListItem<int>>();

            var oConn = _context.Database.GetDbConnection();
            if (oConn != null)
            {
                if (oConn.State == System.Data.ConnectionState.Closed)
                {
                    oConn.Open();
                }
            }

            using (SqlCommand oCmd = new SqlCommand("vsand_ScheduleLoadRecommendProblemSchoolAlt", (SqlConnection)oConn))
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.Parameters.AddWithValue("@schoolName", schoolName);

                using (var oRdr = oCmd.ExecuteReader())
                {
                    while (oRdr.Read())
                    {
                        var schoolId = (int)oRdr["SchoolId"];
                        var name = oRdr["Name"].ToString();
                        oRet.Add(new ListItem<int>(schoolId, name));
                    }
                }
            }

            return oRet;
        }

        public async Task<PagedResult<VsandSchool>> Search(string Name, string City, string State, bool coreCoverage, int pageSize, int pageNumber)
        {
            PagedResult<VsandSchool> oRet = new PagedResult<VsandSchool>(null, 0, pageSize, pageNumber);

            pageNumber -= 1;
            if (pageNumber < 0)
            {
                pageNumber = 0;
            }
            int iSkip = pageNumber * pageSize;

            IQueryable<VsandSchool> oQuery = _context.VsandSchool;
            if (coreCoverage)
            {
                oQuery = oQuery.Where(s => s.CoreCoverage);
            }
            if (!string.IsNullOrEmpty(Name))
            {
                oQuery = oQuery.Where(s => s.Name.Contains(Name));
            }

            if (!string.IsNullOrEmpty(City))
            {
                oQuery = oQuery.Where(s => s.City.Contains(City));
            }

            if (!string.IsNullOrEmpty(State))
            {
                oQuery = oQuery.Where(s => s.State == State);
            }

            oRet.TotalResults = await oQuery.CountAsync();
            oRet.Results = await oQuery.OrderBy(s => s.Name).Skip(iSkip).Take(pageSize).ToListAsync();

            return oRet;
        }

        public List<VsandSchool> GetSchoolsWithoutSportsTeam(int SportId)
        {
            List<VsandSchool> oRet = null;

            IEnumerable<VsandSchool> oData = (from s in _context.VsandSchool
                                              where s.Teams.Any(t => t.Sport.SportId == SportId && t.ScheduleYear.Active == false) == false
                                              orderby s.Name ascending
                                              select s);

            if (oData != null)
            {
                oRet = oData.ToList();
            }

            return oRet;
        }

        public List<VsandSchool> GetSchoolsWithoutSportsTeam(int ScheduleYearId, int SportId)
        {
            List<VsandSchool> oRet = null;

            IEnumerable<VsandSchool> oData = (from s in _context.VsandSchool
                                              where s.Teams.Any(t => t.Sport.SportId == SportId && t.ScheduleYear.ScheduleYearId == ScheduleYearId) == false
                                              orderby s.Name ascending
                                              select s);

            if (oData != null)
            {
                oRet = oData.ToList();
            }

            return oRet;
        }

        public List<VsandSchool> GetValidatedSchoolsWithoutSportsTeam(int ScheduleYearId, int SportId)
        {
            List<VsandSchool> oRet = null;

            IEnumerable<VsandSchool> oData = (from s in _context.VsandSchool
                                              where s.Teams.Any(t => t.Sport.SportId == SportId && t.ScheduleYear.ScheduleYearId == ScheduleYearId) == false && s.Validated == true
                                              orderby s.Name ascending
                                              select s);

            if (oData != null)
            {
                oRet = oData.ToList();
            }

            return oRet;
        }

        public List<VsandSchool> GetUnvalidated(bool bHome)
        {
            List<VsandSchool> oRet = null;

            string sState = _context.AppxConfig.FirstOrDefault(c => c.ConfigCat == "VSAND" && c.ConfigName == "HomeState")?.ConfigVal ?? "";

            IEnumerable<VsandSchool> oData = null;

            if (bHome)
            {
                oData = (from s in _context.VsandSchool
                            .Include(s => s.Teams).ThenInclude(t => t.Sport)
                            .Include(s => s.Teams).ThenInclude(t => t.ScheduleYear)
                         where s.Validated == false && s.State.Equals(sState, StringComparison.OrdinalIgnoreCase)
                         orderby s.Name ascending
                         select s);
            }
            else
            {
                oData = (from s in _context.VsandSchool
                            .Include(s => s.Teams).ThenInclude(t => t.Sport)
                            .Include(s => s.Teams).ThenInclude(t => t.ScheduleYear)
                         where s.Validated == false && !s.State.Equals(sState, StringComparison.OrdinalIgnoreCase)
                         orderby s.Name ascending
                         select s);
            }

            if (oData != null)
            {
                oRet = oData.ToList();
            }

            return oRet;
        }

        public bool ValidateSchool(int SchoolId, int UserId, string Username)
        {
            bool bRet = false;


            VsandSchool oSchool = (from s in _context.VsandSchool
                                   where s.SchoolId == SchoolId
                                   select s).FirstOrDefault();

            if (oSchool != null)
            {
                // -- Only trigger validated if school is not already validated
                if (oSchool.Validated == false)
                {
                    oSchool.Validated = true;
                    oSchool.ValidatedBy = Username;
                    oSchool.ValidatedById = UserId;

                    try
                    {
                        _context.SaveChanges();

                        bRet = true;
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, ex.Message);
                    }

                    if (bRet)
                    {
                        // TODO: event remediation
                        // VSAND.Events.School.RaiseSchoolUpdated(new object(), new VSAND.Events.School.SchoolUpdatedEventArgs(SchoolId, UserId));
                    }
                }
                else
                {
                    bRet = true;
                }
            }
            return bRet;
        }

        public List<VsandSchoolsWithoutAccounts> SchoolsWithoutUsers()
        {
            List<VsandSchoolsWithoutAccounts> oRet = null;

            IEnumerable<VsandSchoolsWithoutAccounts> oData = (from s in _context.VsandSchoolsWithoutAccounts
                                                              orderby s.Name ascending
                                                              select s);

            if (oData != null)
            {
                oRet = oData.ToList();
            }

            return oRet;
        }

        public List<VsandSchool> GetSubscribedByEdition(int EditionId)
        {
            List<VsandSchool> oRet = null;

            IEnumerable<VsandSchool> oData = (from s in _context.VsandSchool
                                              where s.Editions.Any(se => se.Edition.EditionId == EditionId)
                                              orderby s.Name ascending
                                              select s);

            if (oData != null)
            {
                oRet = oData.ToList();
            }

            return oRet;
        }

        public List<VsandSchool> GetUnSubscribedByEdition(int EditionId)
        {
            List<VsandSchool> oRet = null;

            IEnumerable<VsandSchool> oData = (from s in _context.VsandSchool
                                              where !s.Editions.Any(se => se.Edition.EditionId == EditionId)
                                              orderby s.Name ascending
                                              select s);

            if (oData != null)
            {
                oRet = oData.ToList();
            }

            return oRet;
        }
    }
}
