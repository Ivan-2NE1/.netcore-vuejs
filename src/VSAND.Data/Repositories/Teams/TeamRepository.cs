using NLog;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using VSAND.Data.Entities;
using VSAND.Data.Identity;
using VSAND.Data.ViewModels;
using System.Threading.Tasks;
using VSAND.Data.ViewModels.Teams;

namespace VSAND.Data.Repositories
{
    public class TeamRepository : Repository<VsandTeam>, ITeamRepository
    {
        private readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly VsandContext _context;
        public TeamRepository(VsandContext context) : base(context)
        {
            _context = context;
        }

        // TODO: move authentication out of repositories
        /*
        public bool AllowedToEdit(int teamId)
        {
            bool bRet = false;

            // -- Admins are allowed to edit all
            if (HttpContext.Current.User.IsInRole("Admin"))
            {
                return true;
            }

            VsandTeam oTeam = GetTeam(teamId);
            ProfileCommon oP = new ProfileCommon();
            ProfileCommon oProfile = oP.GetProfile(HttpContext.Current.User.Identity.Name);
            if (oProfile != null)
            {
                int schoolId = oProfile.SchoolId;
                if (schoolId == VSAND.EF.ForeignKey(oTeam.SchoolReference))
                    bRet = true;
            }

            return bRet;
        }
        */

        public VsandTeam GetTeam(int teamId)
        {
            return GetTeamAsync(teamId).Result;
        }
        public async Task<VsandTeam> GetTeamAsync(int teamId)
        {
            VsandTeam oTeam = await (from t in _context.VsandTeam
                                    .Include(t => t.Sport)
                                    .Include(t => t.School).ThenInclude(s => s.County)
                                    .Include(t => t.ScheduleYear)
                                    .Include(t => t.CustomCodes)
                                    .Include(t => t.VsandTeamContact)
                                    .Include(t => t.RosterEntries).ThenInclude(re => re.Player)
                                    .Include(t => t.RosterEntries).ThenInclude(re => re.Position2Navigation)
                                    .Include(t => t.RosterEntries).ThenInclude(re => re.PositionNavigation)
                                    .Include(t => t.RosterEntries).ThenInclude(re => re.VsandTeamRosterCustomCode)
                                     where t.TeamId == teamId
                               select t).FirstOrDefaultAsync();

            return oTeam;
        }

        public VsandTeam GetTeam(int SportId, int SchoolId, int ScheduleYearId)
        {
            return GetTeam(GetSchoolTeamId(SportId, SchoolId, ScheduleYearId));
        }

        public VsandTeam GetTeam(int SportId, string TeamName, int ScheduleYearId)
        {
            // TODO: remove reference to school repository in team repository and rewrite as EF query
            int SchoolId = 0; // VSAND.Helper.School.GetSchoolIdByName(TeamName);
            if (SchoolId > 0)
            {
                return GetTeam(SportId, SchoolId, ScheduleYearId);
            }
            else
            {
                return null;
            }
        }

        public VsandTeam GetTeamByName(string Name, int SportId, int ScheduleYearId)
        {
            return _context.VsandTeam.FirstOrDefault(t => t.Name == Name && t.Sport.SportId == SportId && t.ScheduleYear.ScheduleYearId == ScheduleYearId);
        }

        public VsandTeam GetReviewTeam(int TeamId)
        {
            VsandTeam oRet = null;

            oRet = (from t in _context.VsandTeam
                    .Include(t => t.GameReportEntries).ThenInclude(gre => gre.GameReport)
                    .Include(t => t.RosterEntries)
                    .Include(t => t.ScheduleTeamEntries)
                    where t.TeamId == TeamId
                    select t).FirstOrDefault();

            return oRet;
        }

        public string GetTeamName(int TeamId)
        {
            string sRet = "";

            if (TeamId > 0)
            {
                sRet = (from t in _context.VsandTeam
                        where t.TeamId == TeamId
                        select t).First().Name;
            }

            return sRet;
        }

        public int GetTeamSportId(int TeamId)
        {
            int SportId = 0;
            if (TeamId > 0)
            {
                SportId = (from t in _context.VsandTeam.Include(t => t.Sport)
                           where t.TeamId == TeamId
                           select t).First().Sport.SportId;
            }
            return SportId;
        }

        public VsandSport GetTeamSport(int TeamId)
        {
            VsandSport oRet = null;
            if (TeamId > 0)
            {
                oRet = (from t in _context.VsandTeam
                        select t.Sport).FirstOrDefault();
            }
            return oRet;
        }

        public int GetSchoolTeamId(int SportId, int SchoolId, int ScheduleYearId)
        {
            return GetSchoolTeamIdAsync(SportId, SchoolId, ScheduleYearId).Result;
        }

        public async Task<int> GetSchoolTeamIdAsync(int sportId, int schoolId, int scheduleYearId)
        {
            int iTeamId = 0;
            if (sportId > 0 && schoolId > 0 && scheduleYearId > 0)
            {
                VsandTeam oTeam = await _context.VsandTeam.FirstOrDefaultAsync(t => t.SchoolId == schoolId && t.ScheduleYearId == scheduleYearId && t.SportId == sportId);

                if (oTeam != null)
                {
                    iTeamId = oTeam.TeamId;
                }
            }
            return iTeamId;
        }

        public int GetPlayerTeamId(int SportId, int PlayerId, int ScheduleYearId)
        {
            int iRet = 0;

            int SchoolId = 0;
            // TODO: remove reference to player repository and rewrite as EF query
            VsandPlayer oPlayer = null; // VSAND.Helper.Player.GetPlayer(PlayerId);
            if (oPlayer != null)
            {
                SchoolId = oPlayer.School.SchoolId;
            }

            if (SchoolId > 0)
            {
                VsandTeam oTeam = (from t in _context.VsandTeam
                                   where t.School.SchoolId == SchoolId
                                   && t.Sport.SportId == SportId
                                   && t.ScheduleYear.ScheduleYearId == ScheduleYearId
                                   select t).FirstOrDefault();

                if (oTeam != null)
                {
                    iRet = oTeam.TeamId;
                }
            }

            return iRet;
        }

        public VsandTeam GetSchoolTeam(int SportId, int SchoolId, int ScheduleYearId)
        {
            VsandTeam oRet = null;


            oRet = (from t in _context.VsandTeam
                    where t.Sport.SportId == SportId && t.School.SchoolId == SchoolId && t.ScheduleYear.ScheduleYearId == ScheduleYearId
                    select t).FirstOrDefault();

            return oRet;
        }

        public List<VsandTeam> GetSchoolTeamList(int SchoolId)
        {
            List<VsandTeam> oDs = null;

            if (SchoolId > 0)
            {
                IEnumerable<VsandTeam> oData = null;

                oData = from t in _context.VsandTeam.Include(t => t.Sport)
                        where t.School.SchoolId == SchoolId && (t.ScheduleYear.Active.Value)
                        orderby t.Sport.Name ascending
                        select t;

                oDs = oData.ToList();
            }

            return oDs;
        }

        public List<VsandTeam> GetSchoolTeamList(int SchoolId, int ScheduleYearId)
        {
            List<VsandTeam> oDs = null;

            if (SchoolId > 0)
            {
                IEnumerable<VsandTeam> oData = null;

                oData = from t in _context.VsandTeam.Include(t => t.Sport)
                        where t.School.SchoolId == SchoolId
                        && t.ScheduleYear.ScheduleYearId == ScheduleYearId
                        orderby t.Sport.Name ascending
                        select t;

                oDs = oData.ToList();
            }

            return oDs;
        }

        public List<TeamListItem> FindSimilarTeams(int SportId, int ScheduleYearId, string q)
        {
            List<TeamListItem> oTeams = new List<TeamListItem>();

            if (_context.Database.IsSqlServer())
            {
                throw new Exception("Database context is not an instance of SQL server");
            }

            // TODO: write this as an EF query
            using (SqlConnection oConn = (SqlConnection)_context.Database.GetDbConnection())
            {
                using (SqlCommand oComm = new SqlCommand())
                {
                    oComm.Connection = oConn;
                    oComm.CommandText = "vsand_FindSimilarTeams";
                    oComm.CommandType = CommandType.StoredProcedure;
                    oComm.Parameters.AddWithValue("SportId", SportId);
                    oComm.Parameters.AddWithValue("ScheduleYearId", ScheduleYearId);
                    oComm.Parameters.AddWithValue("SearchName", q);
                    oConn.Open();

                    using (SqlDataReader rdr = oComm.ExecuteReader(CommandBehavior.SequentialAccess))
                    {
                        while (rdr.Read())
                        {
                            TeamListItem oTeamItem = new TeamListItem((int)rdr["TeamId"], rdr["Name"].ToString(), rdr["ActualName"].ToString(), rdr["City"].ToString(), rdr["State"].ToString());
                            oTeams.Add(oTeamItem);
                        }
                    }

                    oConn.Close();
                }
            }

            return oTeams;
        }

        public async Task<int> AddSchoolTeamAsync(int SchoolId, int SportId, string TeamName, int ScheduleYearId, bool bValidated, AppxUser user)
        {
            int iTeamId = 0;

            string Username = user.UserId;
            int UserId = user.AdminId;

            var oTeam = await _context.VsandTeam.FirstOrDefaultAsync(t => t.SportId == SportId && t.SchoolId == SchoolId && t.ScheduleYearId == ScheduleYearId);
            if (oTeam == null)
            {
                if (string.IsNullOrEmpty(TeamName.Trim()))
                {
                    var school = _context.VsandSchool.FirstOrDefault(s => s.SchoolId == SchoolId);
                    if (school != null)
                    {
                        TeamName = school.Name.Trim();
                    }
                }

                oTeam = new VsandTeam
                {
                    Name = TeamName,
                    Validated = bValidated,
                    SportId = SportId,
                    SchoolId = SchoolId,
                    ScheduleYearId = ScheduleYearId
                };

                await _context.VsandTeam.AddAsync(oTeam);

                try
                {
                    await _context.SaveChangesAsync();
                    iTeamId = oTeam.TeamId;
                    AuditRepository.AuditChange(_context, "vsand_Team", "TeamId", iTeamId, "Insert", Username, UserId);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, ex.Message);
                }
            } else
            {
                iTeamId = oTeam.TeamId;
            }
            
            return iTeamId;
        }

        public int QuickAddTeam(string Name, int SchoolId, int SportId, int ScheduleYearId)
        {
            return QuickAddTeamAsync(Name, SchoolId, SportId, ScheduleYearId).Result;
        }

        public async Task<int> QuickAddTeamAsync(string name, int schoolId, int sportId, int scheduleYearId)
        {
            int iRet = 0;

            VsandTeam oTeam = new VsandTeam
            {
                Name = name,
                Validated = false,
                SportId = sportId,
                SchoolId = schoolId,
                ScheduleYearId = scheduleYearId
            };

            await _context.VsandTeam.AddAsync(oTeam);
            try
            {
                await _context.SaveChangesAsync();

                iRet = oTeam.TeamId;
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
            }

            return iRet;
        }

        public List<VsandTeam> GetUserTeamList(int AdminId, int SchoolId)
        {
            List<VsandTeam> oRet = null;

            IEnumerable<VsandTeam> oData = (from t in _context.VsandTeam
                                                        .Include(t => t.Sport)
                                                        .Include(t => t.RosterEntries).ThenInclude(re => re.Player)
                                                        .Include(t => t.School).ThenInclude(s => s.County)
                                                        .Include(t => t.ScheduleYear)
                                                        .Include(t => t.CustomCodes)
                                            where t.School.SchoolId == SchoolId && (t.ScheduleYear.Active.Value) && t.Sport.Users.Any(us => us.AdminId == AdminId)
                                            orderby t.Sport.Name ascending
                                            select t);

            if (oData != null)
            {
                oRet = oData.ToList();
            }

            return oRet;
        }

        public bool SaveTeamInformation(int TeamId, string Nickname, string Colors, string Conference, string Superintendent, string Principal, string AthleticDirector, string HeadCoach, string AssistantCoaches, ref string sMsg, ApplicationUser user)
        {
            bool bRet = false;

            string UserName = user.AppxUser.UserId;
            int UserId = user.AppxUser.AdminId;

            // VSAND.Helper.TeamCustomCode.AddOrUpdateTeamCustomCode("Conference", Conference, TeamId, "");
            AuditRepository.AuditChange(_context, "vsand_Team", "TeamId", TeamId, "Update", UserName, UserId);

            VsandTeam oTeam = (from t in _context.VsandTeam
                               where t.TeamId == TeamId
                               select t).FirstOrDefault();

            if (oTeam != null)
            {
                oTeam.TeamNickname = Nickname;
                oTeam.TeamColors = Colors;
                // Dim oConf As VsandTeamCustomCode = oTeam.CustomCodes.FirstOrDefault(Function(cc As VsandTeamCustomCode) cc.CodeName = "Conference")
                // If oConf IsNot Nothing Then
                // oConf.CodeValue = Conference
                // Else
                // oConf = VsandTeamCustomCode.CreateTeamCustomCode(0, "Conference", Conference)
                // oTeam.CustomCodes.Add(oConf)
                // End If
                oTeam.Superintendent = Superintendent;
                oTeam.Principal = Principal;
                oTeam.AthleticDirector = AthleticDirector;
                oTeam.HeadCoach = HeadCoach;
                oTeam.AssistantCoaches = AssistantCoaches;

                try
                {
                    _context.SaveChanges();
                    bRet = true;
                }
                catch (Exception ex)
                {
                    sMsg = "There was an error saving your changes.";
                    Log.Error(ex, ex.Message);
                }
            }
            else
            {
                sMsg = "The team record cannot be found.";
            }

            return bRet;
        }

        // TODO: remediate this
        /*
        public bool CalculateRecord(int TeamId, List<VsandGameReport> oGameReports, ref int iWins, ref int iLosses, ref int iTies, ref int iInState, string sTeamState, ref string sMsg)
        {
            int iConfWins = 0;
            int iConfLosses = 0;
            int iConfTies = 0;
            return CalculateRecord(TeamId, oGameReports, ref iWins, ref iLosses, ref iTies, ref iConfWins, ref iConfLosses, ref iConfTies, ref iInState, sTeamState, ref sMsg);
        }

        public bool CalculateRecord(int TeamId, List<VsandGameReport> oGameReports, ref int iWins, ref int iLosses, ref int iTies, ref int iConfWins, ref int iConfLosses, ref int iConfTies, ref int iInState, string sTeamState, ref string sMsg)
        {
            int iDivWins = 0;
            int iDivLosses = 0;
            int iDivTies = 0;
            int iLeagueWins = 0;
            int iLeagueLosses = 0;
            int iLeagueTies = 0;
            int iHomeWins = 0;
            int iHomeLosses = 0;
            int iHomeTies = 0;
            int iRoadWins = 0;
            int iRoadLosses = 0;
            int iRoadTies = 0;
            int iPointsFor = 0;
            int iPointsAgainst = 0;
            return CalculateRecord(TeamId, oGameReports, ref iWins, ref iLosses, ref iTies, ref iConfWins, ref iConfLosses, ref iConfTies, ref iDivWins, ref iDivLosses, ref iDivTies, ref iLeagueWins, ref iLeagueLosses, ref iLeagueTies, ref iHomeWins, ref iHomeLosses, ref iHomeTies, ref iRoadWins, ref iRoadLosses, ref iRoadTies, ref iInState, sTeamState, ref iPointsFor, ref iPointsAgainst, ref sMsg);
        }

        public bool CalculateRecord(int TeamId, List<VsandGameReport> oGameReports, ref int iWins, ref int iLosses, ref int iTies, ref int iConfWins, ref int iConfLosses, ref int iConfTies, ref int iDivWins, ref int iDivLosses, ref int iDivTies, ref int iLeagueWins, ref int iLeagueLosses, ref int iLeagueTies, ref int iHomeWins, ref int iHomeLosses, ref int iHomeTies, ref int iRoadWins, ref int iRoadLosses, ref int iRoadTies, ref int iInState, string sTeamState, ref int iPointsFor, ref int iPointsAgainst, ref string sMsg)
        {
            // -- Get sport to determine the highscore/lowscore wins
            VsandSport oSport = null;

            oSport = (from t in _context.VsandTeam
                      where t.TeamId == TeamId
                      select t.Sport).FirstOrDefault();

            int ScheduleYearId = 0;
            VsandTeam oRefTeam = GetTeam(TeamId);
            if (oRefTeam != null)
            {
                ScheduleYearId = oRefTeam.ScheduleYearId;
            }

            if (oSport == null)
            {
                sMsg = "Unable to determine team's sport";
                return false;
            }
            bool bGolf = false;
            if (oSport.Name.ToLower().Contains("golf"))
            {
                bGolf = true;
            }

            bool bLowScoreWins = oSport.EnableLowScoreWins.Value;

            // -- Get the EventId for "Regular Season" for thi sport
            int RegularSeasonId = VSAND.Helper.SportEventType.GetDefaultEventType(oSport.SportId, ScheduleYearId);

            // -- For each game, determine if the team won, lost or tied and update the numbers appropriately
            // -- Only perform calculation on non-deleted, regular season games with 2 participants, or pairings configured
            foreach (VsandGameReport oGR in oGameReports)
            {
                if (!oGR.Deleted && !oGR.Exhibition)
                {
                    if (oGR.Teams.Count == 2)
                    {
                        bool bConf = false;
                        bool bDiv = false;
                        bool bLeague = false;
                        bool bHome = false;
                        bool bReg = oGR.GameTypeId == RegularSeasonId ? true : false;
                        int GameReportId = oGR.GameReportId;
                        VsandGameReportTeam oTeam = oGR.Teams.FirstOrDefault(gt => gt.TeamId == TeamId);
                        // TODO: see if this is a valid cast?
                        iPointsFor = iPointsFor + (int)oTeam.FinalScore;
                        if (oTeam.HomeTeam)
                        {
                            bHome = true;
                        }
                        VsandGameReportTeam oOpp = oGR.Teams.FirstOrDefault(gt => gt.TeamId != TeamId);
                        if (oOpp != null)
                        {
                            // TODO: see if this is a valid cast?
                            iPointsAgainst = iPointsAgainst + (int)oOpp.FinalScore;
                            if (oOpp.Team != null && oOpp.Team.School.State == sTeamState)
                            {
                                iInState = iInState + 1;

                                string sTeamConf = "";
                                string sOppConf = "";
                                string sTeamDiv = "";
                                string sOppDiv = "";
                                VsandTeamCustomCode oTeamConf = oTeam.Team.CustomCodes.FirstOrDefault(gt => gt.CodeName == "Conference");
                                VsandTeamCustomCode oOppConf = oOpp.Team.CustomCodes.FirstOrDefault(gt => gt.CodeName == "Conference");
                                if (oTeamConf != null)
                                {
                                    sTeamConf = oTeamConf.CodeValue.Trim();
                                }
                                if (oOppConf != null)
                                {
                                    sOppConf = oOppConf.CodeValue.Trim();
                                }
                                if (sTeamConf.Equals(sOppConf) && !string.IsNullOrEmpty(sTeamConf))
                                {
                                    bConf = true;
                                }

                                VsandTeamCustomCode oTeamDiv = oTeam.Team.CustomCodes.FirstOrDefault(gt => gt.CodeName == "Division");
                                if (oTeamDiv != null)
                                {
                                    sTeamDiv = oTeamDiv.CodeValue.Trim();
                                }
                                VsandTeamCustomCode oOppDiv = oOpp.Team.CustomCodes.FirstOrDefault(gt => gt.CodeName == "Division");
                                if (oOppDiv != null)
                                {
                                    sOppDiv = oOppDiv.CodeValue.Trim();
                                }

                                // -- Check for division                                    
                                if (bConf)
                                {
                                    if (sTeamDiv.Equals(sOppDiv, StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(sTeamDiv))
                                        bDiv = true;
                                }

                                // -- Check for league based on the league rules for the loaded team
                                if (ScheduleYearId < 12)
                                {
                                    // Prior to 2016-17 SY, the league record was the Division record
                                    bLeague = bDiv;
                                }
                                else
                                {
                                    // Starting in 2016-17 SY, a sport + conf + div + sy can have one or more settings for league affiliates
                                    bLeague = VSAND.Helper.LeagueRules.IsLeagueOpponent(oSport.SportId, ScheduleYearId, sTeamConf, sTeamDiv, sOppConf, sOppDiv);
                                }
                            }
                        }

                        bool bMatchPlay = false;
                        if (bGolf)
                        {
                            // -- Check to see if this was match play, if so, high score wins

                            VsandGameReportMeta oMeta = (from m in _context.VsandGameReportMeta
                                                         where m.GameReport.GameReportId == GameReportId && m.SportGameMeta.ValueType == "VSAND.GolfPlayFormat"
                                                         select m).FirstOrDefault();

                            if (oMeta != null)
                            {
                                if (oMeta.MetaValue == "1")
                                {
                                    bMatchPlay = true;
                                }
                            }
                        }

                        if (bLowScoreWins && !bMatchPlay)
                        {
                            double iLowScore = oGR.Teams.Min(gt => gt.FinalScore);
                            IEnumerable<VsandGameReportTeam> oWinners = oGR.Teams.Where(gt => gt.FinalScore == iLowScore);
                            if (oWinners.Count() == 1)
                            {
                                VsandGameReportTeam oWin = oWinners.ElementAt(0);
                                if (oWin != null)
                                {
                                    if (oWin.Team.TeamId == TeamId)
                                    {
                                        iWins = iWins + 1;
                                        if (bReg)
                                        {
                                            if (bConf)
                                            {
                                                iConfWins = iConfWins + 1;
                                            }
                                            if (bDiv)
                                            {
                                                iDivWins = iDivWins + 1;
                                            }
                                        }
                                        if (bHome)
                                        {
                                            iHomeWins = iHomeWins + 1;
                                        }
                                        if (!bHome)
                                        {
                                            iRoadWins = iRoadWins + 1;
                                        }
                                    }
                                    else
                                    {
                                        iLosses = iLosses + 1;
                                        if (bReg)
                                        {
                                            if (bConf)
                                                iConfLosses = iConfLosses + 1;
                                            if (bDiv)
                                                iDivLosses = iDivLosses + 1;
                                        }
                                        if (bHome)
                                        {
                                            iHomeLosses = iHomeLosses + 1;
                                        }
                                        if (!bHome)
                                        {
                                            iRoadLosses = iRoadLosses + 1;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                // -- It was a tie
                                iTies = iTies + 1;
                                if (bReg)
                                {
                                    if (bConf)
                                    {
                                        iConfTies = iConfTies + 1;
                                    }
                                    if (bDiv)
                                    {
                                        iDivTies = iDivTies + 1;
                                    }
                                }
                                if (bHome)
                                {
                                    iHomeTies = iHomeTies + 1;
                                }
                                if (!bHome)
                                {
                                    iRoadTies = iRoadTies + 1;
                                }
                            }
                        }
                        else
                        {
                            double iMaxScore = oGR.Teams.Max(gt => gt.FinalScore);
                            IEnumerable<VsandGameReportTeam> oWinners = oGR.Teams.Where(gt => gt.FinalScore == iMaxScore);
                            if (oWinners.Count() == 1)
                            {
                                VsandGameReportTeam oWin = oWinners.ElementAt(0);
                                if (oWin != null)
                                {
                                    if (oWin.TeamId == TeamId)
                                    {
                                        iWins = iWins + 1;
                                        if (bReg)
                                        {
                                            if (bConf)
                                            {
                                                iConfWins = iConfWins + 1;
                                            }
                                            if (bDiv)
                                            {
                                                iDivWins = iDivWins + 1;
                                            }
                                            if (bLeague)
                                            {
                                                iLeagueWins = iLeagueWins + 1;
                                            }
                                        }
                                        if (bHome)
                                        {
                                            iHomeWins = iHomeWins + 1;
                                        }
                                        if (!bHome)
                                        {
                                            iRoadWins = iRoadWins + 1;
                                        }
                                    }
                                    else
                                    {
                                        iLosses = iLosses + 1;
                                        if (bReg)
                                        {
                                            if (bConf)
                                            {
                                                iConfLosses = iConfLosses + 1;
                                            }
                                            if (bDiv)
                                            {
                                                iDivLosses = iDivLosses + 1;
                                            }
                                            if (bLeague)
                                            {
                                                iLeagueLosses = iLeagueLosses + 1;
                                            }
                                        }
                                        if (bHome)
                                        {
                                            iHomeLosses = iHomeLosses + 1;
                                        }
                                        if (!bHome)
                                        {
                                            iRoadLosses = iRoadLosses + 1;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                // -- It was a tie
                                iTies = iTies + 1;
                                if (bReg)
                                {
                                    if (bConf)
                                    {
                                        iConfTies = iConfTies + 1;
                                    }
                                    if (bDiv)
                                    {
                                        iDivTies = iDivTies + 1;
                                    }
                                    if (bLeague)
                                    {
                                        iLeagueTies = iLeagueTies + 1;
                                    }
                                }
                                if (bHome)
                                {
                                    iHomeTies = iHomeTies + 1;
                                }
                                if (!bHome)
                                {
                                    iRoadTies = iRoadTies + 1;
                                }
                            }
                        }
                    }
                    else if (oGR.Teams.Count > 2 && oGR.Pairings.Count > 0 && oSport.EnableTriPlusScheduling)
                    {
                    }
                }
            }

            return true;
        }

        public vsandHelper.Reports.TeamRecordInfoDataTable TeamRecordInfoDataSet(List<TeamRecordInfo> oTeamRecInfo)
        {
            vsandHelper.Reports.TeamRecordInfoDataTable oDs = new vsandHelper.Reports.TeamRecordInfoDataTable();

            foreach (TeamRecordInfo oRecInfo in oTeamRecInfo)
            {
                DataRow oDataRow = oDs.NewRow;
                oDataRow["TeamId"] = oRecInfo.TeamId;
                oDataRow["TeamName"] = oRecInfo.TeamName;
                oDataRow["County"] = oRecInfo.County;
                oDataRow["Section"] = oRecInfo.Section;
                oDataRow["Group"] = oRecInfo.Group;
                oDataRow["Wins"] = oRecInfo.Wins;
                oDataRow["Losses"] = oRecInfo.Losses;
                oDataRow["Ties"] = oRecInfo.Ties;
                oDataRow["WinningPercentage"] = oRecInfo.WinningPercentage;
                oDataRow["InStatePercentage"] = oRecInfo.InStatePercentage;
                oDataRow["PowerPoints"] = oRecInfo.PowerPoints;
                oDataRow["Differential"] = oRecInfo.Differential;
                if (oRecInfo.LastReport.HasValue)
                {
                    oDataRow["LastReport"] = oRecInfo.LastReport;
                }
                oDs.Rows.Add(oDataRow);
            }

            return oDs;
        }
        
        public vsandHelper.Reports.TeamComplianceInfoDataTable TeamComplianceInfoDataSet(List<TeamComplianceInfo> oTeamCompInfo)
        {
            vsandHelper.Reports.TeamComplianceInfoDataTable oDs = new vsandHelper.Reports.TeamComplianceInfoDataTable();

            foreach (Helper.TeamComplianceInfo oRecInfo in oTeamCompInfo)
            {
                DataRow oDataRow = oDs.NewRow;
                oDataRow["TeamId"] = oRecInfo.TeamId;
                oDataRow["TeamName"] = oRecInfo.TeamName;
                oDataRow["EventCount"] = oRecInfo.EventCount;
                oDataRow["RosterCount"] = oRecInfo.RosterCount;
                oDataRow["UnvalidatedCount"] = oRecInfo.UnvalidatedCount;

                oDs.Rows.Add(oDataRow);
            }

            return oDs;
        }

        public vsandHelper.Reports.TeamClassificationInfoDataTable TeamClassificationInfoDataSet(List<TeamClassificationInfo> oTeamClassInfo)
        {
            vsandHelper.Reports.TeamClassificationInfoDataTable oDs = new vsandHelper.Reports.TeamClassificationInfoDataTable();

            foreach (TeamClassificationInfo oRecInfo in oTeamClassInfo)
            {
                DataRow oDataRow = oDs.NewRow;
                oDataRow["TeamId"] = oRecInfo.TeamId;
                oDataRow["TeamName"] = oRecInfo.TeamName;
                oDataRow["Section"] = oRecInfo.Section;
                oDataRow["Group"] = oRecInfo.Group;

                oDs.Rows.Add(oDataRow);
            }

            return oDs;
        }
        */

        public List<VsandTeam> GetSportsTeams(int SportId, int ScheduleYearId)
        {
            List<VsandTeam> oRet = null;

            IEnumerable<VsandTeam> oData = (from t in _context.VsandTeam
                                            where t.Sport.SportId == SportId && t.ScheduleYear.ScheduleYearId == ScheduleYearId
                                            orderby t.Name ascending
                                            select t);

            if (oData != null)
            {
                oRet = oData.ToList();
            }

            return oRet;
        }

        public List<VsandTeam> GetSportsTeamsWithGames(int sportId, int ScheduleYearId)
        {
            List<VsandTeam> oRet = null;

            IEnumerable<VsandTeam> oData = (from t in _context.VsandTeam
                                            where t.Sport.SportId == sportId
                                            && t.ScheduleYear.ScheduleYearId == ScheduleYearId
                                            && t.GameReportEntries.Where(ge => ge.GameReport.Deleted == false).Count() > 0
                                            orderby t.Name ascending
                                            select t);

            if (oData != null)
            {
                oRet = oData.ToList();
            }

            return oRet;
        }

        public List<VsandTeam> GetSportsTeamsWithCodes(int SportId, int ScheduleYearId)
        {
            List<VsandTeam> oRet = null;

            IEnumerable<VsandTeam> oData = (from t in _context.VsandTeam.Include(t => t.CustomCodes)
                                            where t.Sport.SportId == SportId
                                            && t.ScheduleYear.ScheduleYearId == ScheduleYearId
                                            orderby t.Name ascending
                                            select t);

            if (oData != null)
            {
                oRet = oData.ToList();
            }

            return oRet;
        }

        public List<VsandTeam> GetInStateSportsTeams(int SportId, int ScheduleYearId, string State)
        {
            List<VsandTeam> oRet = null;

            IEnumerable<VsandTeam> oData = (from t in _context.VsandTeam
                                            where t.Sport.SportId == SportId
                                            && t.ScheduleYear.ScheduleYearId == ScheduleYearId
                                            && t.School.State == State
                                            orderby t.Name ascending
                                            select t);

            if (oData != null)
            {
                oRet = oData.ToList();
            }

            return oRet;
        }

        public List<VsandTeam> GetInStateSportsTeamsWithCustomCodes(int SportId, int ScheduleYearId, string State)
        {
            List<VsandTeam> oRet = null;

            IEnumerable<VsandTeam> oData = (from t in _context.VsandTeam.Include(t => t.CustomCodes)
                                            where t.Sport.SportId == SportId
                                            && t.ScheduleYear.ScheduleYearId == ScheduleYearId
                                            && t.School.State == State
                                            orderby t.Name ascending
                                            select t);

            if (oData != null)
            {
                oRet = oData.ToList();
            }

            return oRet;
        }

        public List<VsandTeam> GetTeamsByConference(int SportId, int ScheduleYearId, string Conference)
        {
            List<VsandTeam> oRet = null;

            IEnumerable<VsandTeam> oData = (from t in _context.VsandTeam.Include(t => t.School)
                                            where t.Sport.SportId == SportId
                                            && t.ScheduleYear.ScheduleYearId == ScheduleYearId
                                            && t.CustomCodes.Any(cc => cc.CodeName == "Conference" && cc.CodeValue == Conference)
                                            orderby t.Name ascending
                                            select t);

            if (oData != null)
            {
                oRet = oData.ToList();
            }

            return oRet;
        }

        public List<VsandTeam> GetTeamsWithoutConference(int SportId, int ScheduleYearId, string State)
        {
            List<VsandTeam> oRet = null;


            IEnumerable<VsandTeam> oData = (from t in _context.VsandTeam.Include(t => t.School)
                                            where t.Sport.SportId == SportId
                                            && t.ScheduleYear.ScheduleYearId == ScheduleYearId
                                            && t.School.State == State
                                            && t.CustomCodes.Any(cc => cc.CodeName == "Conference" && cc.CodeValue != "") == false
                                            orderby t.Name ascending
                                            select t);

            if (oData != null)
            {
                oRet = oData.ToList();
            }

            return oRet;
        }

        public List<VsandTeam> GetTeamsByTeamCustomCode(int SportId, int ScheduleYearId, string CodeName, string CodeValue)
        {
            List<VsandTeam> oRet = null;

            IEnumerable<VsandTeam> oData = (from t in _context.VsandTeam
                                            where t.Sport.SportId == SportId
                                            && t.ScheduleYear.ScheduleYearId == ScheduleYearId
                                            && t.CustomCodes.Any(cc => cc.CodeName == CodeName && cc.CodeValue == CodeValue)
                                            orderby t.Name ascending
                                            select t);

            if (oData != null)
            {
                oRet = oData.ToList();
            }

            return oRet;
        }

        public List<VsandTeam> GetTeamsBySchoolCustomCode(int SportId, int ScheduleYearId, string CodeName, string CodeValue)
        {
            List<VsandTeam> oRet = null;

            IEnumerable<VsandTeam> oData = (from t in _context.VsandTeam
                                            where t.Sport.SportId == SportId
                                            && t.ScheduleYear.ScheduleYearId == ScheduleYearId
                                            && t.School.CustomCodes.Any(cc => cc.CodeName == CodeName && cc.CodeValue == CodeValue)
                                            orderby t.Name ascending
                                            select t);

            if (oData != null)
            {
                oRet = oData.ToList();
            }

            return oRet;
        }

        public List<VsandTeam> GetTeamsBySportEventTypeRound(int RoundId)
        {
            List<VsandTeam> oRet = null;

            IEnumerable<VsandTeam> oData = (from grep in _context.VsandGameReportEventPlayer
                                            where grep.EventResult.GameReportEvent.GameReport.Round.RoundId == RoundId
                                            select grep.GameReportTeam.Team);

            if (oData != null)
            {
                oRet = oData.ToList();
            }

            return oRet;
        }

        public List<VsandTeam> GetTeamsBySelfAdvancingRounds(int RoundId)
        {
            List<VsandTeam> oRet = new List<VsandTeam>();

            VsandSportEventTypeRound oCurRound = (from setr in _context.VsandSportEventTypeRound
                                                           .Include(setr => setr.SportEventType)
                                                  where setr.RoundId == RoundId
                                                  select setr).FirstOrDefault();

            if (oCurRound != null)
            {
                string sFilter = "";
                if (oCurRound.ParticipatingTeamsFilter != null)
                    sFilter = oCurRound.ParticipatingTeamsFilter;
                if (string.IsNullOrEmpty(sFilter.Trim()))
                {
                    // -- Get the parent event's filter
                    if (oCurRound.SportEventType.ParticipatingTeamsFilter != null)
                    {
                        sFilter = oCurRound.SportEventType.ParticipatingTeamsFilter;
                    }
                }

                if (!string.IsNullOrEmpty(sFilter))
                {
                    char[] aSplit = new[] { ',', ';', '|', ' ' };
                    string[] aRounds = sFilter.Split(aSplit);

                    for (int i = 0; i < aRounds.Length; i++)
                    {
                        string sRound = aRounds[i].Trim();
                        bool bWinnerOnly = false;
                        bool bLoserOnly = false;
                        if (sRound.ToUpper().EndsWith("W"))
                        {
                            bWinnerOnly = true;
                            sRound = sRound.Substring(0, sRound.Length - 1);
                        }
                        else if (sRound.ToUpper().EndsWith("L"))
                        {
                            bLoserOnly = true;
                            sRound = sRound.Substring(0, sRound.Length - 1);
                        }
                        int.TryParse(sRound, out int iRound);

                        if (iRound > 0)
                        {
                            IEnumerable<VsandTeam> oData = null;
                            if (bWinnerOnly)
                            {

                                oData = (from grep in _context.VsandGameReportEventPlayer
                                         where grep.EventResult.GameReportEvent.GameReport.Round.RoundId == iRound
                                         && grep.Winner == true
                                         select grep.GameReportTeam.Team);
                            }
                            else if (bLoserOnly)
                            {
                                oData = (from grep in _context.VsandGameReportEventPlayer
                                         where grep.EventResult.GameReportEvent.GameReport.Round.RoundId == iRound
                                         && grep.Winner == false
                                         select grep.GameReportTeam.Team);
                            }
                            else
                            {
                                oData = (from grep in _context.VsandGameReportEventPlayer
                                         where grep.EventResult.GameReportEvent.GameReport.Round.RoundId == iRound
                                         select grep.GameReportTeam.Team);
                            }

                            if (oData != null)
                            {
                                if (oRet == null)
                                {
                                    oRet = oData.ToList();
                                }
                                else
                                {
                                    List<VsandTeam> oTeams = oData.ToList();
                                    foreach (VsandTeam oTeam in oTeams)
                                    {
                                        if (oTeam != null)
                                        {
                                            int iTeam = oTeam.TeamId;
                                            VsandTeam oFound = oRet.FirstOrDefault(t => t.TeamId == iTeam);
                                            if (oFound == null)
                                            {
                                                oRet.Add(oTeam);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return oRet;
        }

        public bool ValidateTeam(int TeamId, int UserId, string Username, bool SuppressEvents, ref string sMsg)
        {
            bool bRet = false;

            AuditRepository.AuditChange(_context, "vsand_Team", "TeamId", TeamId, "Update", Username, UserId);

            VsandTeam oTeam = (from t in _context.VsandTeam
                               where t.TeamId == TeamId
                               select t).FirstOrDefault();

            if (oTeam != null)
            {
                if (!oTeam.Validated)
                {
                    oTeam.Validated = true;
                    oTeam.ValidatedBy = Username;
                    oTeam.ValidatedById = UserId;

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

                    if (bRet && !SuppressEvents)
                    {
                        // TODO: event remediation
                        // VSAND.Events.Team.RaiseTeamValidated(new object(), new VSAND.Events.Team.TeamValidatedEventArgs(TeamId, UserId));
                    }
                }
                else
                {
                    bRet = true;
                }
            }

            return bRet;
        }

        public bool UnValidateTeam(int TeamId, int UserId, string Username, ref string sMsg)
        {
            bool bRet = false;

            AuditRepository.AuditChange(_context, "vsand_Team", "TeamId", TeamId, "Update", Username, UserId);

            VsandTeam oTeam = (from t in _context.VsandTeam
                               where t.TeamId == TeamId
                               select t).FirstOrDefault();

            if (oTeam != null)
            {
                oTeam.Validated = false;
                oTeam.ValidatedBy = "";
                oTeam.ValidatedById = new int?();

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

            return bRet;
        }

        public List<VsandTeam> GetUnusedTeams(int ScheduleYearId, int SportId)
        {
            List<VsandTeam> oRet = null;


            IEnumerable<VsandTeam> oData = (from t in _context.VsandTeam
                                            where t.ScheduleYear.ScheduleYearId == ScheduleYearId
                                            && t.Sport.SportId == SportId
                                            && t.GameReportEntries.Count == 0
                                            && t.RosterEntries.Count == 0
                                            && t.Schedules.Count == 0
                                            orderby t.Name ascending
                                            select t);

            if (oData != null)
            {
                oRet = oData.ToList();
            }

            return oRet;
        }

        public List<VsandTeam> GetUnvalidatedTeamsInGames(int ScheduleYearId)
        {
            List<VsandTeam> oRet = (from t in _context.VsandTeam.Include(t => t.School)
                                    where t.Validated == false
                                    && t.ScheduleYear.ScheduleYearId == ScheduleYearId
                                    && t.GameReportEntries.Any(grt => grt.GameReport.Deleted == false)
                                    select t).ToList();

            return oRet;
        }

        public bool DeleteTeam(int TeamId, int UserId, string Username, ref string sMsg)
        {
            bool bRet = false;

            VsandTeam oTeam = (from t in _context.VsandTeam
                               where t.TeamId == TeamId
                               select t).FirstOrDefault();

            if (oTeam != null)
            {
                _context.VsandTeam.Remove(oTeam);

                AuditRepository.AuditChange(_context, "vsand_Team", "TeamId", TeamId, "Delete", Username, UserId);

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
                sMsg = "The team cannot be found; perhaps it was already removed?";
            }

            return bRet;
        }

        public bool MergeTeams(int TargetTeamId, int SourceTeamId, SortedList<int, int> RosterResolver, ref string sMsg, ApplicationUser user)
        {
            bool bRet = false;

            string sUser = user.AppxUser.UserId;
            int iUser = user.AppxUser.AdminId;

            VsandTeam oTarget = GetTeam(TargetTeamId);
            VsandTeam oSource = GetTeam(SourceTeamId);
            int SportId = oTarget.SportId;
            int ScheduleYearId = oTarget.ScheduleYearId;

            List<int> oGameReports = new List<int>();
            List<int> oDeletedPlayers = new List<int>();

            // TODO: Do this in a transaction
            using (TransactionScope oTrans = new TransactionScope())
            {
                IEnumerable<VsandBookNote> oBookNotes = (from bn in _context.VsandBookNote
                                                         where bn.Team.TeamId == SourceTeamId
                                                         select bn);

                foreach (VsandBookNote oBookNote in oBookNotes)
                {
                    oBookNote.TeamId = oTarget.TeamId;
                }

                IEnumerable<VsandBookMember> oBooks = (from bm in _context.VsandBookMember
                                                       where (bm.School.SchoolId == oSource.School.SchoolId
                                                       && bm.Book.Sport.SportId == SportId
                                                       && bm.Book.ScheduleYear.ScheduleYearId == ScheduleYearId)
                                                       select bm);
                foreach (VsandBookMember oBook in oBooks)
                {
                    // -- Check for our target in the same book
                    VsandBookMember oTargetBook = (from bm in _context.VsandBookMember
                                                   where bm.School.SchoolId == oTarget.School.SchoolId
                                                   && bm.Book.Sport.SportId == SportId
                                                   && bm.Book.ScheduleYear.ScheduleYearId == ScheduleYearId
                                                   select bm).FirstOrDefault();

                    if (oTargetBook == null)
                    {
                        oBook.SchoolId = oTarget.SchoolId.Value;
                    }
                    else
                    {
                        _context.VsandBookMember.Remove(oBook);
                    }
                }

                IEnumerable<VsandTeamContact> oContacts = (from tc in _context.VsandTeamContact
                                                           where tc.Team.TeamId == SourceTeamId
                                                           select tc);

                foreach (VsandTeamContact oContact in oContacts)
                {
                    oContact.TeamId = oTarget.TeamId;
                }

                IEnumerable<VsandTeamCustomCode> oCustomCodes = (from cc in _context.VsandTeamCustomCode
                                                                 where cc.Team.TeamId == SourceTeamId
                                                                 select cc);

                // TODO: test RemoveRange method
                _context.VsandTeamCustomCode.RemoveRange(oCustomCodes);

                /*
                foreach (VsandTeamCustomCode oCC in oCustomCodes)
                {
                    _context.VsandTeamCustomCode.Remove(oCC);
                }
                */

                IEnumerable<VsandGameReportTeam> oGameEntries = (from grt in _context.VsandGameReportTeam
                                                                 where grt.Team.TeamId == SourceTeamId
                                                                 select grt);

                foreach (VsandGameReportTeam oGE in oGameEntries)
                {
                    oGE.TeamId = oTarget.TeamId;
                    oGameReports.Add(oGE.GameReportId);
                }

                IEnumerable<VsandTeamScheduleTeam> oSchedules = (from tst in _context.VsandTeamScheduleTeam
                                                                 where tst.Team.TeamId == SourceTeamId
                                                                 select tst);

                foreach (VsandTeamScheduleTeam oSched in oSchedules)
                {
                    oSched.TeamId = oTarget.TeamId;
                }

                // -- Reconcile the Players
                IEnumerator<KeyValuePair<int, int>> oEnum = RosterResolver.GetEnumerator();
                while (oEnum.MoveNext())
                {
                    int SourcePlayerId = oEnum.Current.Key;
                    int TargetPlayerId = oEnum.Current.Value;

                    if (TargetPlayerId == -1) // -- Delete source player
                    {
                        IEnumerable<VsandGameReportPlayerStat> oGameStats = null;
                        oGameStats = (from ps in _context.VsandGameReportPlayerStat
                                      where ps.Player.PlayerId == SourcePlayerId
                                      select ps);

                        foreach (VsandGameReportPlayerStat oPlayerStat in oGameStats)
                        {
                            _context.VsandGameReportPlayerStat.Remove(oPlayerStat);
                        }

                        IEnumerable<VsandGameReportEventPlayerGroupPlayer> oGameEventPlayerGroupPlayers = null;
                        oGameEventPlayerGroupPlayers = (from pgp in _context.VsandGameReportEventPlayerGroupPlayer
                                                        where pgp.Player.PlayerId == SourcePlayerId
                                                        select pgp);

                        foreach (VsandGameReportEventPlayerGroupPlayer oPlayerGroupPlayer in oGameEventPlayerGroupPlayers)
                        {
                            _context.VsandGameReportEventPlayerGroupPlayer.Remove(oPlayerGroupPlayer);
                        }

                        IEnumerable<VsandGameReportEventPlayer> oGameEventPlayers = null;
                        oGameEventPlayers = (from ep in _context.VsandGameReportEventPlayer
                                             where ep.Player.PlayerId == SourcePlayerId
                                             select ep);

                        foreach (VsandGameReportEventPlayer oEventPlayer in oGameEventPlayers)
                        {
                            _context.VsandGameReportEventPlayer.Remove(oEventPlayer);
                        }

                        IEnumerable<VsandGameReportRoster> oGameRosters = null;
                        oGameRosters = (from gr in _context.VsandGameReportRoster
                                        where gr.Player.PlayerId == SourcePlayerId
                                        select gr);

                        foreach (VsandGameReportRoster oGR in oGameRosters)
                        {
                            // -- Check to see if our target player is already on this roster
                            _context.VsandGameReportRoster.Remove(oGR);
                        }

                        IEnumerable<VsandTeamRoster> oTeamRosters = null;
                        oTeamRosters = (from tr in _context.VsandTeamRoster
                                        where tr.Player.PlayerId == SourcePlayerId
                                        select tr);

                        foreach (VsandTeamRoster oTR in oTeamRosters)
                        {
                            _context.VsandTeamRoster.Remove(oTR);
                        }

                        VsandPlayer oDelPlayer = (from p in _context.VsandPlayer
                                                  where p.PlayerId == SourcePlayerId
                                                  select p).FirstOrDefault();

                        if (oDelPlayer != null)
                        {
                            _context.VsandPlayer.Remove(oDelPlayer);
                        }

                        oDeletedPlayers.Add(SourcePlayerId);
                    }
                    else if (TargetPlayerId == SourcePlayerId) // -- Migrate the source player to the new school / team
                    {
                        VsandPlayer oPlayer = (from p in _context.VsandPlayer
                                               where p.PlayerId == SourcePlayerId
                                               select p).FirstOrDefault();

                        oPlayer.SchoolId = oTarget.SchoolId;

                        // -- Add the player onto the target team roster
                        VsandTeamRoster oRosterEntry = (from tr in _context.VsandTeamRoster
                                                        where tr.Player.PlayerId == SourcePlayerId
                                                        && tr.Team.TeamId == oSource.TeamId
                                                        select tr).FirstOrDefault();

                        if (oRosterEntry != null)
                        {
                            oRosterEntry.TeamId = oTarget.TeamId;
                        }
                    }
                    else
                    {
                        IEnumerable<VsandGameReportPlayerStat> oGameStats = null;
                        oGameStats = (from ps in _context.VsandGameReportPlayerStat
                                      where ps.Player.PlayerId == SourcePlayerId
                                      select ps);

                        foreach (VsandGameReportPlayerStat oPlayerStat in oGameStats)
                            oPlayerStat.PlayerId = TargetPlayerId;

                        IEnumerable<VsandGameReportEventPlayerGroupPlayer> oGameEventPlayerGroupPlayers = null;
                        oGameEventPlayerGroupPlayers = (from pgp in _context.VsandGameReportEventPlayerGroupPlayer
                                                        where pgp.Player.PlayerId == SourcePlayerId
                                                        select pgp);

                        foreach (VsandGameReportEventPlayerGroupPlayer oPlayerGroupPlayer in oGameEventPlayerGroupPlayers)
                        {
                            oPlayerGroupPlayer.PlayerId = TargetPlayerId;
                        }

                        IEnumerable<VsandGameReportEventPlayer> oGameEventPlayers = null;
                        oGameEventPlayers = (from ep in _context.VsandGameReportEventPlayer
                                             where ep.Player.PlayerId == SourcePlayerId
                                             select ep);

                        foreach (VsandGameReportEventPlayer oEventPlayer in oGameEventPlayers)
                            oEventPlayer.PlayerId = TargetPlayerId;

                        IEnumerable<VsandGameReportRoster> oGameRosters = (from gr in _context.VsandGameReportRoster
                                                                           where gr.Player.PlayerId == SourcePlayerId
                                                                           select gr);

                        foreach (VsandGameReportRoster oGR in oGameRosters)
                        {
                            // -- Check to see if our target player is already on this roster
                            int GameReportTeamId = oGR.GameReportTeamId;

                            VsandGameReportRoster oTargetGameRoster = (from grr in _context.VsandGameReportRoster
                                                                       where grr.Player.PlayerId == TargetPlayerId
                                                                       && grr.GameReportTeam.GameReportTeamId == GameReportTeamId
                                                                       select grr).FirstOrDefault();
                            if (oTargetGameRoster != null)
                            {
                                // -- Delete the entry for the source player, we already have one
                                _context.VsandGameReportRoster.Remove(oGR);
                            }
                            else
                            {
                                // -- Change the reference to our target player
                                oGR.PlayerId = TargetPlayerId;
                            }
                        }

                        IEnumerable<VsandTeamRoster> oTeamRosters = null;
                        oTeamRosters = (from tr in _context.VsandTeamRoster
                                        where tr.Player.PlayerId == SourcePlayerId
                                        select tr);

                        foreach (VsandTeamRoster oTR in oTeamRosters)
                        {
                            int TeamId = oTR.TeamId;
                            VsandTeamRoster oTargetTeamRoster = (from tr in _context.VsandTeamRoster
                                                                 where tr.Player.PlayerId == TargetPlayerId
                                                                 && tr.Team.TeamId == TeamId
                                                                 select tr).FirstOrDefault();
                            if (oTargetTeamRoster != null)
                            {
                                // -- Delete the entry for the source player, we alrady have one
                                _context.VsandTeamRoster.Remove(oTR);
                            }
                            else
                            {
                                // -- Change the reference to our target player
                                oTR.PlayerId = TargetPlayerId;
                            }
                        }

                        IEnumerable<VsandPlayerRecruiting> oPlayerRecruits = (from pr in _context.VsandPlayerRecruiting
                                                                              where pr.Player.PlayerId == SourcePlayerId
                                                                              select pr);

                        foreach (VsandPlayerRecruiting oRecruit in oPlayerRecruits)
                        {
                            oRecruit.PlayerId = TargetPlayerId;
                        }

                        // -- Delete the original player after all of the related references are updated
                        VsandPlayer oDelPlayer = (from p in _context.VsandPlayer
                                                  where p.PlayerId == SourcePlayerId
                                                  select p).FirstOrDefault();
                        if (oDelPlayer != null)
                        {
                            _context.VsandPlayer.Remove(oDelPlayer);
                        }

                        oDeletedPlayers.Add(SourcePlayerId);
                    }
                }

                // -- Delete the Source Team
                VsandTeam oTeam = (from t in _context.VsandTeam
                                   where t.TeamId == SourceTeamId
                                   select t).FirstOrDefault();

                _context.VsandTeam.Remove(oTeam);

                try
                {
                    _context.SaveChanges();
                    oTrans.Complete();
                    bRet = true;
                }
                catch (Exception ex)
                {
                    sMsg = ex.Message;
                    Log.Error(ex, sMsg);
                }
            }

            if (bRet)
            {
                // -- Trigger the team delete
                // TODO: event remediation
                // VSAND.Events.Team.RaiseTeamDeleted(new object(), new VSAND.Events.Team.TeamDeletedEventArgs(SourceTeamId));

                // -- Trigger Game Teams Changed
                for (int i = 0; i <= oGameReports.Count - 1; i++)
                {
                    int GameReportId = oGameReports[i];
                    // TODO: event remediation
                    // VSAND.Events.GameReport.RaiseGameTeamsChanged(new object(), new VSAND.Events.GameReport.GameReportEventArgs(GameReportId, iUser));
                }

                // -- Recalculate the Target Team's Record
                // TODO: event remediation
                // VSAND.Events.Records.RaiseRecalculate(new object(), new VSAND.Events.Records.RecordsEventArgs(TargetTeamId));

                // -- Triger the player deleted messages
                for (int i = 0; i <= oDeletedPlayers.Count - 1; i++)
                {
                    int PlayerId = oDeletedPlayers[i];
                    // TODO: event remediation
                    // VSAND.Events.Player.RaisePlayerDeleted(new object(), new VSAND.Events.Player.PlayerDeletedEventArgs(PlayerId, iUser));
                }
            }

            return bRet;
        }

        public async Task<PagedResult<TeamSummary>> GetLatestTeamsPagedAsync(SearchRequest searchRequest)
        {
            var oRet = new PagedResult<TeamSummary>(null, 0, searchRequest.PageSize, searchRequest.PageNumber);

            var oQuery = from t in _context.VsandTeam select t;
            if (searchRequest.SchoolId.HasValue && searchRequest.SchoolId.Value > 0)
            {
                oQuery = oQuery.Where(x => x.SchoolId == searchRequest.SchoolId);
            }
            if (searchRequest.Sports != null && searchRequest.Sports.Any())
            {
                oQuery = oQuery.Where(x => searchRequest.Sports.Contains(x.SportId));
            }
            if (searchRequest.ScheduleYearId.HasValue && searchRequest.ScheduleYearId.Value > 0)
            {
                oQuery = oQuery.Where(x => x.ScheduleYearId == searchRequest.ScheduleYearId);
            }

            var totalResults = await oQuery.CountAsync();

            var iPg = searchRequest.PageNumber - 1;
            if (iPg < 0) iPg = 0;
            var iSkip = iPg * searchRequest.PageSize;

            var oResults = await (from x in oQuery
                                  select new TeamSummary()
                                  {
                                      TeamId = x.TeamId,
                                      SchoolId = (x.SchoolId.HasValue ? x.SchoolId.Value : 0),
                                      SportId = x.SportId,
                                      ScheduleYearId = x.ScheduleYearId,
                                      Name = x.School.Name,
                                      Sport = x.Sport.Name,
                                      ScheduleYear = x.ScheduleYear.Name
                                  }).Skip(iSkip).Take(searchRequest.PageSize).ToListAsync();

            oRet.Results = oResults;
            oRet.TotalResults = totalResults;

            return oRet;
        }
        // TODO: move this comparer off of the interface?
        public class Comparer : IEqualityComparer<VsandTeam>
        {
            public bool Equals(VsandTeam x, VsandTeam y)
            {
                return x.TeamId == y.TeamId;
            }

            public int GetHashCode(VsandTeam obj)
            {
                return obj.TeamId.GetHashCode();
            }
        }
    }
}
