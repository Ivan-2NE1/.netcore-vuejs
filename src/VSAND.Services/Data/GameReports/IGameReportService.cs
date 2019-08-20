using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VSAND.Data.Entities;
using VSAND.Data.Repositories;
using VSAND.Data.ViewModels;
using VSAND.Data.ViewModels.GameReport;

namespace VSAND.Services.Data.GameReports
{
    public interface IGameReportService
    {
        Task<ServiceResult<AddGameReport>> AddGameReportAsync(AppxUser user, AddGameReport oGame);
        Task<ServiceResult<AddScheduledGame>> AddScheduledGameAsync(AppxUser user, AddScheduledGame oGame);
        Task<AddGameReport> GetAddGameReport(int? sportId, int? scheduleYearId, int? teamId);
        Task<AddScheduledGame> GetAddScheduledGame(int? sportId, int? scheduleYearId, int? teamId);
        Task<ServiceResult<GameReport>> UpdateGameReportOverview(AppxUser user, GameReport gameReport);
        Task<GameReport> GetFullGameReport(int gameReportId);
        Task<GameReport> GetFullGameReportCachedAsync(int gameReportId);
        Task<GameReport> GetGameReport(int gameReportId);
        Task<GameReport> GetGameReportPlayerStats(int gameReportId, int gameReportTeamId);
        Task<GameReport> GetGameReportScoring(int gameReportId);
        Task<GameReport> GetGameReportTeamStats(int gameReportId);
        Task<GameReport> GetGameReportEvents(int gameReportId);
        Task<PagedResult<GameReportSummary>> ReverseChronologicalList(SearchRequest oSearch);
        Task<List<GameReportSummary>> ScheduleScoreboard(DateTime? viewDate, int? sportId, int? schoolId, int? scheduleYearId);
        Task<IEnumerable<TeamGameSummary>> TeamGames(int teamId);
        Task<IEnumerable<TeamRecordInfo>> TeamRecordInfo(int teamId, bool UseCached);
        Task<ServiceResult<GameReportPlayerStat>> SavePlayerStatAsync(AppxUser user, GameReportPlayerStat grps);
        Task<ServiceResult<GameReportTeamStat>> SaveTeamStatAsync(AppxUser user, GameReportTeamStat grts);
        Task<ServiceResult<List<GameReportPlayerStat>>> InitPlayer(AppxUser user, int gameReportId, int gameReportTeamId, int playerId);
        Task<VsandGameReport> GetEntityGameRport(int gameReportId);
        Task<ServiceResult<VsandGameReport>> DeleteGameReportAsync(int gameReportId);
    }
}
