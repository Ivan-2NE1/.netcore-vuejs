using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NLog;
using System;
using System.Threading.Tasks;
using VSAND.Data.Entities;

namespace VSAND.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private bool _disposed = false;
        private bool _hasTransaction = false;
        private IDbContextTransaction _trans = null;
        private readonly VsandContext _context;
        private Exception _lastError = null;

        #region Custom Repository Implementations
        private IAppxUserRoleRepository _appxUserRoles;
        private IAuditRepository _audit;
        private IGameReportRepository _gameReports;
        private IPlayerRepository _players;
        private IPublicationRepository _publications;
        private IScheduleYearRepository _scheduleYears;
        private IScheduleYearProvisioningSummaryRepository _scheduleYearProvisioningSummaries;
        private IScheduleYearEventTypeListItemsRepository _scheduleYearEventTypeListItems;
        private IScheduleYearSummaryRepository _scheduleYearSummaries;
        private ISchoolContactRepository _schoolContacts;
        private ISchoolRepository _schools;
        private ISportRepository _sports;
        private ITeamRepository _teams;
        private ITeamCustomCodeRepository _teamCustomCodes;
        private IUserRepository _users;
        #endregion

        #region Generic Repository Implementations
        private IRepository<AppxConfig> _appxConfigs;
        private IRepository<AppxUserRoleMember> _appxUserRoleMembers;
        private IRepository<AppxUser> _appxUsers;
        private IRepository<VsandState> _states;
        private IRepository<VsandCounty> _counties;
        private IRepository<VsandConference> _conferences;
        private IRepository<VsandGameReportEventPlayer> _gameReportEventPlayers;
        private IRepository<VsandGameReportEventPlayerGroup> _gameReportEventPlayerGroups;
        private IRepository<VsandGameReportMeta> _gameReportMeta;
        private IRepository<VsandGameReportTeam> _gameReportTeams;
        private IRepository<VsandGameReportNote> _gameReportNotes;
        private IRepository<VsandGameReportRoster> _gameReportRoster;
        private IRepository<VsandGameReportPlayerStat> _gameReportPlayerStats;
        private IRepository<VsandGameReportTeamStat> _gameReportTeamStats;
        private IRepository<VsandLeagueRule> _leagueRules;
        private IRepository<VsandLeagueRuleItem> _leagueRuleItems;
        private IRepository<LocalLiveEvent> _localLiveEvents;
        private IRepository<VsandPowerPointsConfig> _powerPointsConfig;
        private IRepository<VsandScheduleLoadFile> _scheduleLoadFiles;
        private IRepository<VsandScheduleLoadFileParse> _scheduleLoadFileRows;
        private IRepository<VsandSportGameMeta> _sportGameMeta;
        private IRepository<VsandSportEvent> _sportEvent;
        private IRepository<VsandSportEventType> _sportEventTypes;
        private IRepository<VsandSportEventTypeGroup> _sportEventTypeGroups;
        private IRepository<VsandSportEventTypeRound> _sportEventTypeRounds;
        private IRepository<VsandSportEventTypeSection> _sportEventTypeSections;
        private IRepository<VsandSportEventResult> _sportEventResults;
        private IRepository<VsandSportTeamStatCategory> _sportTeamStatCategories;
        private IRepository<VsandSportPlayerStatCategory> _sportPlayerStatCategories;
        private IRepository<VsandSportPlayerStat> _sportPlayerStats;
        private IRepository<VsandSportPosition> _sportPositions;
        private IRepository<VsandSchoolCustomCode> _schoolCustomCodes;
        private IRepository<VsandSportTeamStat> _sportTeamStats;
        private IRepository<VsandTeamRoster> _teamRoster;
        private IRepository<VsandTeamNotifyList> _teamNotifications;
        private IRepository<VsandEntitySlug> _entitySlugs;
        private IRepository<VsandUserSport> _userSports;
        #endregion

        public UnitOfWork(VsandContext context)
        {
            _context = context ?? throw new ArgumentException("Context is null");
        }

        public IRepository<AppxConfig> AppxConfigs => _appxConfigs ?? (_appxConfigs = new Repository<AppxConfig>(_context));
        public IRepository<AppxUserRoleMember> AppxUserRoleMembers => _appxUserRoleMembers ?? (_appxUserRoleMembers = new Repository<AppxUserRoleMember>(_context));
        public IAppxUserRoleRepository AppxUserRoles => _appxUserRoles ?? (_appxUserRoles = new AppxUserRoleRepository(_context));
        public IRepository<AppxUser> AppxUsers => _appxUsers ?? (_appxUsers = new Repository<AppxUser>(_context));
        public IAuditRepository Audit => _audit ?? (_audit = new AuditRepository(_context));
        public IRepository<VsandEntitySlug> EntitySlugs => _entitySlugs ?? (_entitySlugs = new Repository<VsandEntitySlug>(_context));
        public IGameReportRepository GameReports => _gameReports ?? (_gameReports = new GameReportRepository(_context));
        public IRepository<VsandGameReportEventPlayer> GameReportEventPlayers => _gameReportEventPlayers ?? (_gameReportEventPlayers = new Repository<VsandGameReportEventPlayer>(_context));
        public IRepository<VsandGameReportEventPlayerGroup> GameReportEventPlayerGroups => _gameReportEventPlayerGroups ?? (_gameReportEventPlayerGroups = new Repository<VsandGameReportEventPlayerGroup>(_context));
        public IRepository<VsandGameReportMeta> GameReportMeta => _gameReportMeta ?? (_gameReportMeta = new Repository<VsandGameReportMeta>(_context));
        public IRepository<VsandGameReportNote> GameReportNotes => _gameReportNotes ?? (_gameReportNotes = new Repository<VsandGameReportNote>(_context));
        public IRepository<VsandGameReportTeam> GameReportTeams => _gameReportTeams ?? (_gameReportTeams = new Repository<VsandGameReportTeam>(_context));
        public IRepository<VsandGameReportRoster> GameReportRoster => _gameReportRoster ?? (_gameReportRoster = new Repository<VsandGameReportRoster>(_context));
        public IRepository<VsandGameReportPlayerStat> GameReportPlayerStats => _gameReportPlayerStats ?? (_gameReportPlayerStats = new Repository<VsandGameReportPlayerStat>(_context));
        public IRepository<VsandGameReportTeamStat> GameReportTeamStats => _gameReportTeamStats ?? (_gameReportTeamStats = new Repository<VsandGameReportTeamStat>(_context));
        public IRepository<VsandLeagueRule> LeagueRules => _leagueRules ?? (_leagueRules = new Repository<VsandLeagueRule>(_context));
        public IRepository<VsandLeagueRuleItem> LeagueRuleItems => _leagueRuleItems ?? (_leagueRuleItems = new Repository<VsandLeagueRuleItem>(_context));
        public IRepository<LocalLiveEvent> LocalLiveEvents => _localLiveEvents ?? (_localLiveEvents = new Repository<LocalLiveEvent>(_context));
        public IPlayerRepository Players => _players ?? (_players = new PlayerRepository(_context));
        public IRepository<VsandPowerPointsConfig> PowerPointsConfig => _powerPointsConfig ?? (_powerPointsConfig = new Repository<VsandPowerPointsConfig>(_context));
        public IPublicationRepository Publications => _publications ?? (_publications = new PublicationRepository(_context));
        public IRepository<VsandScheduleLoadFile> ScheduleLoadFiles => _scheduleLoadFiles ?? (_scheduleLoadFiles = new Repository<VsandScheduleLoadFile>(_context));
        public IRepository<VsandScheduleLoadFileParse> ScheduleLoadFileRows => _scheduleLoadFileRows ?? (_scheduleLoadFileRows = new Repository<VsandScheduleLoadFileParse>(_context));
        public IScheduleYearRepository ScheduleYears => _scheduleYears ?? (_scheduleYears = new ScheduleYearRepository(_context));
        public IScheduleYearProvisioningSummaryRepository ScheduleYearProvisioningSummaries => _scheduleYearProvisioningSummaries ?? (_scheduleYearProvisioningSummaries = new ScheduleYearProvisioningSummaryRepository(_context));
        public IScheduleYearEventTypeListItemsRepository ScheduleYearEventTypeListItems => _scheduleYearEventTypeListItems ?? (_scheduleYearEventTypeListItems  = new ScheduleYearEventTypeListItemsRepository(_context));
        public IScheduleYearSummaryRepository ScheduleYearSummaries => _scheduleYearSummaries ?? (_scheduleYearSummaries = new ScheduleYearSummaryRepository(_context));
        public ISchoolContactRepository SchoolContacts => _schoolContacts ?? (_schoolContacts = new SchoolContactRepository(_context));
        public ISchoolRepository SchoolRepository => _schools ?? (_schools = new SchoolRepository(_context));
        public ISchoolRepository Schools => _schools ?? (_schools = new SchoolRepository(_context));
        public ISportRepository Sports => _sports ?? (_sports = new SportRepository(_context));
        public ITeamRepository Teams => _teams ?? (_teams = new TeamRepository(_context));
        public IRepository<VsandTeamRoster> TeamRoster => _teamRoster ?? (_teamRoster = new Repository<VsandTeamRoster>(_context));
        public IRepository<VsandTeamNotifyList> TeamNotifications => _teamNotifications ?? (_teamNotifications = new Repository<VsandTeamNotifyList>(_context));
        public ITeamCustomCodeRepository TeamCustomCodes => _teamCustomCodes ?? (_teamCustomCodes = new TeamCustomCodeRepository(_context));
        public IRepository<VsandSportEventType> SportEventTypes => _sportEventTypes ?? (_sportEventTypes = new Repository<VsandSportEventType>(_context));
        public IRepository<VsandSportEventTypeGroup> SportEventTypeGroups => _sportEventTypeGroups ?? (_sportEventTypeGroups = new Repository<VsandSportEventTypeGroup>(_context));
        public IRepository<VsandSportEventTypeRound> SportEventTypeRounds => _sportEventTypeRounds ?? (_sportEventTypeRounds = new Repository<VsandSportEventTypeRound>(_context));
        public IRepository<VsandSportEventTypeSection> SportEventTypeSections => _sportEventTypeSections ?? (_sportEventTypeSections = new Repository<VsandSportEventTypeSection>(_context));
        public IRepository<VsandSportEventResult> SportEventResults => _sportEventResults ?? (_sportEventResults = new Repository<VsandSportEventResult>(_context));
        public IRepository<VsandSportPosition> SportPositions => _sportPositions ?? (_sportPositions = new Repository<VsandSportPosition>(_context));
        public IRepository<VsandState> States => _states ?? (_states = new Repository<VsandState>(_context));
        public IRepository<VsandCounty> Counties => _counties ?? (_counties = new Repository<VsandCounty>(_context));
        public IRepository<VsandConference> Conferences => _conferences ?? (_conferences = new Repository<VsandConference>(_context));
        public IRepository<VsandSportGameMeta> SportGameMeta => _sportGameMeta ?? (_sportGameMeta = new Repository<VsandSportGameMeta>(_context));
        public IRepository<VsandSportEvent> SportEvents => _sportEvent ?? (_sportEvent = new Repository<VsandSportEvent>(_context));
        public IRepository<VsandSportTeamStatCategory> SportTeamStatCategories => _sportTeamStatCategories ?? (_sportTeamStatCategories = new Repository<VsandSportTeamStatCategory>(_context));
        public IRepository<VsandSportPlayerStatCategory> SportPlayerStatCategories => _sportPlayerStatCategories ?? (_sportPlayerStatCategories = new Repository<VsandSportPlayerStatCategory>(_context));
        public IRepository<VsandSportTeamStat> SportTeamStats => _sportTeamStats ?? (_sportTeamStats = new Repository<VsandSportTeamStat>(_context));
        public IRepository<VsandSportPlayerStat> SportPlayerStats => _sportPlayerStats ?? (_sportPlayerStats = new Repository<VsandSportPlayerStat>(_context));
        public IRepository<VsandSchoolCustomCode> SchoolCustomCodes => _schoolCustomCodes ?? (_schoolCustomCodes = new Repository<VsandSchoolCustomCode>(_context));
        public IUserRepository Users => _users ?? (_users = new UserRepository(_context));
        public IRepository<VsandUserSport> UserSports => _userSports ?? (_userSports = new Repository<VsandUserSport>(_context));

        public void BeginTransaction()
        {
            _trans = _context.Database.BeginTransaction();
            _hasTransaction = true;
        }

        public async Task<bool> Save()
        {
            bool bRet;
            try
            {
                await _context.SaveChangesAsync();
                bRet = true;
            }
            catch (Exception ex)
            {
                _lastError = ex;

                Log.Error(ex, ex.Message);
                bRet = false;
            }
            return bRet;
        }

        public async Task<bool> CommitTransaction()
        {
            bool bRet;
            try
            {
                await _context.SaveChangesAsync();
                if (_hasTransaction)
                {
                    _trans?.Commit();
                }

                // Reset the transaction
                _hasTransaction = false;
                _trans = null;

                var oConn = _context.Database.GetDbConnection();
                if (oConn != null)
                {
                    if (oConn.State == System.Data.ConnectionState.Closed)
                    {
                        oConn.Open();
                    }                    
                }

                bRet = true;
            }
            catch (Exception ex)
            {
                _lastError = ex;

                _trans?.Rollback();
                Log.Error(ex, ex.Message);
                bRet = false;
            }
            return bRet;
        }

        public Exception GetLastError()
        {
            return _lastError;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _trans?.Dispose();

                    _context.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
