using System;
using System.Threading.Tasks;
using VSAND.Data.Entities;

namespace VSAND.Data.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<AppxConfig> AppxConfigs { get; }
        IRepository<AppxUserRoleMember> AppxUserRoleMembers { get; }
        IAppxUserRoleRepository AppxUserRoles { get; }
        IRepository<AppxUser> AppxUsers { get; }
        IAuditRepository Audit { get; }
        IRepository<VsandCounty> Counties { get; }
        IRepository<VsandConference> Conferences { get; }
        IRepository<VsandEntitySlug> EntitySlugs { get; }
        IGameReportRepository GameReports { get; }
        IRepository<VsandGameReportEventPlayer> GameReportEventPlayers { get; }
        IRepository<VsandGameReportEventPlayerGroup> GameReportEventPlayerGroups { get; }
        IRepository<VsandGameReportMeta> GameReportMeta { get; }
        IRepository<VsandGameReportTeam> GameReportTeams { get; }
        IRepository<VsandGameReportRoster> GameReportRoster { get; }
        IRepository<VsandGameReportPlayerStat> GameReportPlayerStats { get; }
        IRepository<VsandGameReportTeamStat> GameReportTeamStats { get; }
        IRepository<VsandGameReportNote> GameReportNotes { get; }
        IRepository<VsandLeagueRule> LeagueRules { get; }
        IRepository<VsandLeagueRuleItem> LeagueRuleItems { get; }
        IRepository<LocalLiveEvent> LocalLiveEvents { get; }
        IPlayerRepository Players { get; }
        IRepository<VsandPowerPointsConfig> PowerPointsConfig { get; }
        IPublicationRepository Publications { get; }
        
        IRepository<VsandScheduleLoadFile> ScheduleLoadFiles { get; }
        IRepository<VsandScheduleLoadFileParse> ScheduleLoadFileRows { get; }

        IScheduleYearRepository ScheduleYears { get; }
        IScheduleYearProvisioningSummaryRepository ScheduleYearProvisioningSummaries { get; }
        IScheduleYearEventTypeListItemsRepository ScheduleYearEventTypeListItems { get; }
        IScheduleYearSummaryRepository ScheduleYearSummaries { get; }
        ISchoolRepository Schools { get; }
        IRepository<VsandSchoolCustomCode> SchoolCustomCodes { get; }
        IRepository<VsandSportEventResult> SportEventResults { get; }
        IRepository<VsandSportEvent> SportEvents { get; }
        IRepository<VsandSportEventType> SportEventTypes { get; }
        IRepository<VsandSportEventTypeGroup> SportEventTypeGroups { get; }
        IRepository<VsandSportEventTypeRound> SportEventTypeRounds { get; }
        IRepository<VsandSportEventTypeSection> SportEventTypeSections { get; }
        IRepository<VsandSportGameMeta> SportGameMeta { get; }
        IRepository<VsandSportPlayerStatCategory> SportPlayerStatCategories { get; }
        IRepository<VsandSportPosition> SportPositions { get; }
        IRepository<VsandSportTeamStatCategory> SportTeamStatCategories { get; }
        IRepository<VsandSportTeamStat> SportTeamStats { get; }
        IRepository<VsandSportPlayerStat> SportPlayerStats { get; }
        ISportRepository Sports { get; } 
        IRepository<VsandState> States { get; }
        ITeamRepository Teams { get; }
        IRepository<VsandTeamRoster> TeamRoster { get; }
        IRepository<VsandTeamNotifyList> TeamNotifications { get; }
        ITeamCustomCodeRepository TeamCustomCodes { get; }
        IUserRepository Users { get; }
        IRepository<VsandUserSport> UserSports { get; }

        void BeginTransaction();
        Task<bool> Save();
        Task<bool> CommitTransaction();
        Exception GetLastError();
    }
}
