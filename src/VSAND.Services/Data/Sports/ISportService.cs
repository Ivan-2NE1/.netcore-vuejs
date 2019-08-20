using System.Collections.Generic;
using System.Threading.Tasks;
using VSAND.Data.Entities;
using VSAND.Data.ViewModels;
using VSAND.Data.ViewModels.Sports;

namespace VSAND.Services.Data.Sports
{
    public interface ISportService
    {
        SortedList<int, string> GetSeasons(string activeSeason);
        Task<SortedList<string, List<ListItem<string>>>> GetSportNavBySeasonAsync();
        Task<SortedList<string, List<ListItem<string>>>> GetSportNavBySeasonCachedAsync();
        Task<Dictionary<string, List<ListItem<string>>>> GetSportNavByGenderAsync();
        Task<Dictionary<string, List<ListItem<string>>>> GetSportNavByGenderCachedAsync();
        Task<IEnumerable<VsandSport>> GetListAsync();
        Task<IEnumerable<ListItem<int>>> GetActiveListAsync();
        Task<VsandSport> GetSportAsync(int sportId);
        Task<VsandSport> GetFullSportCachedAsync(int sportId);
        Task<ServiceResult<VsandSport>> AddSportAsync(VsandSport addSport);
        Task<ServiceResult<VsandSport>> UpdateSportAsync(VsandSport chgSport);

        Task<VsandSport> GetPositionsAsync(int sportId);
        Task<ServiceResult<VsandSportPosition>> UpdatePositionAsync(VsandSportPosition chgPosition);
        Task<ServiceResult<VsandSportPosition>> AddPositionAsync(VsandSportPosition addPosition);

        Task<VsandSport> GetGameMetaAsync(int sportId);
        Task<ServiceResult<VsandSportGameMeta>> AddGameMetaAsync(VsandSportGameMeta addGameMeta);
        Task<ServiceResult<VsandSportGameMeta>> UpdateGameMetaAsync(VsandSportGameMeta chgGameMeta);
        Task<ServiceResult<VsandSportGameMeta>> DeleteGameMetaAsync(int gameMetaId);
        Task<bool> UpdateGameMetaOrder(int sportId, List<VsandSportGameMeta> gameMeta);

        Task<VsandSport> GetEventsAsync(int sportId);
        Task<ServiceResult<VsandSportEvent>> AddEventAsync(VsandSportEvent addEvent);
        Task<ServiceResult<VsandSportEvent>> UpdateEventAsync(VsandSportEvent chgEvent);
        Task<ServiceResult<VsandSportEvent>> DeleteEventAsync(int eventId);
        Task<bool> UpdateEventOrder(int sportId, List<VsandSportEvent> events);

        Task<ServiceResult<VsandSportEventResult>> AddEventResultAsync(VsandSportEventResult addEventResult);
        Task<ServiceResult<VsandSportEventResult>> UpdateEventResultAsync(VsandSportEventResult chgEventResult);
        Task<ServiceResult<VsandSportEventResult>> DeleteEventResultAsync(int eventResultId);
        Task<bool> UpdateEventResultOrder(int sportId, List<VsandSportEventResult> events);

        Task<VsandSport> GetTeamStatsAsync(int sportId);
        Task<IEnumerable<TeamStatCategory>> GetTeamStatCategoryAsync(int sportId);
        Task<ServiceResult<VsandSportTeamStatCategory>> AddTeamStatCategoryAsync(VsandSportTeamStatCategory addTeamStatCategory);
        Task<ServiceResult<VsandSportTeamStatCategory>> UpdateTeamStatCategoryAsync(VsandSportTeamStatCategory chgTeamStatCategory);
        Task<ServiceResult<VsandSportTeamStatCategory>> DeleteTeamStatCategoryAsync(int teamStatCategoryId);
        Task<bool> UpdateTeamStatCategoryOrder(int sportId, List<VsandSportTeamStatCategory> teamStatCategories);

        Task<VsandSport> GetPlayerStatsAsync(int sportId);
        Task<IEnumerable<PlayerStatCategory>> GetPlayerStatCategoryAsync(int sportId);
        Task<ServiceResult<VsandSportPlayerStatCategory>> AddPlayerStatCategoryAsync(VsandSportPlayerStatCategory addPlayerStatCategory);
        Task<ServiceResult<VsandSportPlayerStatCategory>> UpdatePlayerStatCategoryAsync(VsandSportPlayerStatCategory chgPlayerStatCategory);
        Task<ServiceResult<VsandSportPlayerStatCategory>> DeletePlayerStatCategoryAsync(int playerStatCategoryId);
        Task<bool> UpdatePlayerStatCategoryOrder(int sportId, List<VsandSportPlayerStatCategory> playerStatCategories);

        Task<IEnumerable<TeamStat>> GetTeamStatAsync(int sportId);
        Task<ServiceResult<VsandSportTeamStat>> AddTeamStatAsync(VsandSportTeamStat addTeamStat);
        Task<ServiceResult<VsandSportTeamStat>> UpdateTeamStatAsync(VsandSportTeamStat chgTeamStat);
        Task<ServiceResult<VsandSportTeamStat>> DeleteTeamStatAsync(int teamStatId);
        Task<bool> UpdateTeamStatOrder(int sportId, List<VsandSportTeamStat> teamStats);
        Task<bool> MoveTeamStat(int teamStatCategoryId, int teamStatId);

        Task<IEnumerable<PlayerStat>> GetPlayerStatAsync(int sportId);
        Task<ServiceResult<VsandSportPlayerStat>> AddPlayerStatAsync(VsandSportPlayerStat addPlayerStat);
        Task<ServiceResult<VsandSportPlayerStat>> UpdatePlayerStatAsync(VsandSportPlayerStat chgPlayerStat);
        Task<ServiceResult<VsandSportPlayerStat>> DeletePlayerStatAsync(int playerStatId);
        Task<bool> UpdatePlayerStatOrder(int sportId, List<VsandSportPlayerStat> playerStats);
        Task<bool> MovePlayerStat(int playerStatCategoryId, int playerStatId);

        Task<List<ListItem<string>>> GetFeaturedSportItemsCachedAsync();
        Task<StandingsView> GetSportsStandingViewAsync(int sportId, int scheduleYearId, string conference);
        Task<StandingsView> GetSportStandingsViewCachedAsync(int sportId, int? scheduleYearId, string conference);

        Task<PowerpointsView> GetSportsPowerpointsViewAsync(int sportId, int scheduleYearId, string section, string group);
        Task<PowerpointsView> GetSportPowerpointsViewCachedAsync(int sportId, int? scheduleYearId, string section, string group);

        Task<List<VsandSportEventType>> GetTopLevelEventTypesAsync(int sportId, int scheduleYearId);
        Task<List<VsandSportEventType>> GetChildEventTypes(int parentEventTypeId);
        Task<VsandSportEventType> GetEventTypeAsync(int eventTypeId);
        Task<ServiceResult<VsandSportEventType>> AddEventTypeAsync(VsandSportEventType addEventType);
        Task<ServiceResult<VsandSportEventType>> UpdateEventTypeAsync(VsandSportEventType chgEventType);
        Task<ServiceResult<VsandSportEventType>> DeleteEventTypeAsync(int eventTypeId);
        Task<List<EventTypeListItem>> GetActiveEventTypeObjects(int sportId, int scheduleYearId);
        Task<List<ListItem<string>>> GetActiveEventTypes(int sportId, int scheduleYearId);
        Task<List<EventTypeListItem>> GetAllEventTypeObjects(int sportId, int scheduleYearId);
        Task<List<ListItem<string>>> GetAllEventTypes(int sportId, int scheduleYearId);

        Task<List<VsandSportEventTypeRound>> GetEventTypeRoundsAsync(int eventTypeId);
        Task<ServiceResult<VsandSportEventTypeRound>> AddEventTypeRoundAsync(VsandSportEventTypeRound addRound);
        Task<ServiceResult<VsandSportEventTypeRound>> UpdateEventTypeRoundAsync(VsandSportEventTypeRound chgRound);
        Task<ServiceResult<VsandSportEventTypeRound>> DeleteEventTypeRoundAsync(int roundCategoryId);
        Task<bool> UpdateEventTypeRoundOrder(int eventTypeId, List<VsandSportEventTypeRound> rounds);

        Task<List<VsandSportEventTypeSection>> GetEventTypeSectionsAsync(int eventTypeId);
        Task<ServiceResult<VsandSportEventTypeSection>> AddEventTypeSectionAsync(VsandSportEventTypeSection addSection);
        Task<ServiceResult<VsandSportEventTypeSection>> UpdateEventTypeSectionAsync(VsandSportEventTypeSection chgSection);
        Task<ServiceResult<VsandSportEventTypeSection>> DeleteEventTypeSectionAsync(int sectionCategoryId);
        Task<bool> UpdateEventTypeSectionOrder(int eventTypeId, List<VsandSportEventTypeSection> sections);

        Task<ServiceResult<VsandSportEventTypeGroup>> AddEventTypeGroupAsync(VsandSportEventTypeGroup addGroup);
        Task<ServiceResult<VsandSportEventTypeGroup>> UpdateEventTypeGroupAsync(VsandSportEventTypeGroup chgGroup);
        Task<ServiceResult<VsandSportEventTypeGroup>> DeleteEventTypeGroupAsync(int groupCategoryId);
        Task<bool> UpdateEventTypeGroupOrder(int sectionId, List<VsandSportEventTypeGroup> groups);
        Task<IEnumerable<TeamStatistics>> GetTeamStatisticsAsync(int sportId);
    }
}
