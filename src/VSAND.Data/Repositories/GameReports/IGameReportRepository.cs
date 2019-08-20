using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VSAND.Data.Entities;
using VSAND.Data.Identity;
using VSAND.Data.ViewModels;
using VSAND.Data.ViewModels.GameReport;

namespace VSAND.Data.Repositories
{
    public interface IGameReportRepository : IRepository<VsandGameReport>
    {
        VsandGameReport GetGameReport(int GameReportId);

        bool IsRegularSeason(int GameReportId);

        Task<VsandGameReport> GetFullGameReport(int GameReportId, int PublicationStoryId = 0);

        /// <summary>
        /// Interface definition for default page of latest games by game date in reverse chronological order
        /// </summary>
        /// <param name="pageSize">The number of games per page</param>
        /// <param name="pageNumber">The current page number</param>
        /// <returns>PagedResult of VsandGameReport entity</returns>
        Task<PagedResult<VsandGameReport>> GetLatestGamesPagedAsync(SearchRequest oSearch);

        Task<List<VsandGameReport>> ScheduleScoreboard(DateTime? viewDate, int? sportId, int? schoolId, int? scheduleYearId);

        /// <summary>
        /// Interface definition for list of games that contain the reference team in forward chronological order
        /// </summary>
        /// <param name="teamId">The unique id of the team</param>
        /// <returns>IEnumerable of VsandGameReport entity</returns>
        Task<IEnumerable<VsandGameReport>> GetTeamGamesAsync(int teamId);

        Task<IEnumerable<VsandTeam>> GetTeamRecordInfo(int teamId);

        List<VsandGameReport> GetOpenGames();

        List<VsandGameReport> GetOpenUnsentGames(int CountyId, int SportId);

        List<VsandGameReport> GetOpenSentGames(int CountyId, int SportId);

        List<int> GetOpenGameIds();

        List<VsandGameReport> GetOpenBySport(int SportId);

        List<VsandGameReport> GetForDateBySport(int SportId, DateTime GameDate);

        List<VsandGameReport> GetOpenForFeedSubscription(int SportId, int FeedSubscriptionId);

        List<VsandGameReport> GetForDateForFeedSubscription(int SportId, int FeedSubscriptionId, DateTime GameDate);

        VsandGameReport GetGameReportByParticipatingTeam(int GameReportTeamId);

        int GetGameReportIdByParticipatingTeams(int TeamId1, int TeamId2);

        bool ExcludeGame(int GameReportId, ref string sMsg, ApplicationUser user);

        List<VsandGameReport> SearchGameReports(int SearchSchoolId, int SearchSportId, int SearchScheduleYearId, DateTime SearchGameDate, int SearchTouchedBy, int maximumRows, int startRowIndex);

        int SearchGameReportsCount(int SearchSchoolId, int SearchSportId, int SearchScheduleYearId, DateTime SearchGameDate, int SearchTouchedBy);

        List<VsandGameReport> GetTeamReportHistory(int TeamId, DateTime? StartDate, DateTime? EndDate);

        string UpdateRecord(int TeamId, bool bLowScoreWins, ICollection<VsandGameReportTeam> oGameTeams, bool bLeague, ref int iWins, ref int iLosses, ref int iTies, ref int iLeagueWins, ref int iLeagueLosses, ref int iLeagueTies);

        string UpdateRecord(int TeamId, bool bLowScoreWins, ICollection<VsandGameReportTeam> oGameTeams, bool bLeague, ref int iWins, ref int iLosses, ref int iTies, ref int iLeagueWins, ref int iLeagueLosses, ref int iLeagueTies, string sConf);
                
        string FormatRecord(int iWins, int iLosses, int iTies, int iLeagueWins, int iLeagueLosses, int iLeagueTies, string sConf);

        // TODO: decide what happens to formatting
        /*
        string GetFormattedGameReport(int GameReportId, SortedList<string, object> dContext, string Formatter);

        string GetFormattedGameReport(VsandGameReport oGameReport, int FormatId);

        string GetFormattedGameReport(VsandGameReport oGameReport, SortedList<string, object> dContext, string Formatter);
        */

        List<VsandTeam> GetDuplicateGameReports(DateTime ViewDate);

        VsandGameReport FindDuplicateGameReport(DateTime GameDate, List<ParticipatingTeam> Teams);
    }
}
