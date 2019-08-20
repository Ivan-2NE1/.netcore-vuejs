using System.Collections.Generic;
using System.Threading.Tasks;
using VSAND.Data.Entities;
using VSAND.Data.Identity;
using VSAND.Data.ViewModels;
using VSAND.Data.ViewModels.Teams;

namespace VSAND.Data.Repositories
{
    public interface ITeamRepository : IRepository<VsandTeam>
    {
        VsandTeam GetTeam(int TeamId);
        Task<VsandTeam> GetTeamAsync(int TeamId);

        VsandTeam GetTeam(int SportId, int SchoolId, int ScheduleYearId);

        VsandTeam GetTeam(int SportId, string TeamName, int ScheduleYearId);

        VsandTeam GetTeamByName(string Name, int SportId, int ScheduleYearId);
        VsandTeam GetReviewTeam(int TeamId);
        string GetTeamName(int TeamId);
        int GetTeamSportId(int TeamId);
        VsandSport GetTeamSport(int TeamId);
        int GetSchoolTeamId(int SportId, int SchoolId, int ScheduleYearId);
        Task<int> GetSchoolTeamIdAsync(int sportId, int schoolId, int scheduleYearId);
        int GetPlayerTeamId(int SportId, int PlayerId, int ScheduleYearId);

        VsandTeam GetSchoolTeam(int SportId, int SchoolId, int ScheduleYearId);

        List<VsandTeam> GetSchoolTeamList(int SchoolId);

        List<VsandTeam> GetSchoolTeamList(int SchoolId, int ScheduleYearId);

        List<TeamListItem> FindSimilarTeams(int SportId, int ScheduleYearId, string q);

        Task<int> AddSchoolTeamAsync(int SchoolId, int SportId, string TeamName, int ScheduleYearId, bool bValidated, AppxUser user);

        int QuickAddTeam(string Name, int SchoolId, int SportId, int ScheduleYearId);
        Task<int> QuickAddTeamAsync(string name, int schoolId, int sportId, int scheduleYearId);

        List<VsandTeam> GetUserTeamList(int AdminId, int SchoolId);

        bool SaveTeamInformation(int TeamId, string Nickname, string Colors, string Conference, string Superintendent, string Principal, string AthleticDirector, string HeadCoach, string AssistantCoaches, ref string sMsg, ApplicationUser user);


        // TODO: decide what to do with these
        /*
        bool CalculateRecord(int TeamId, List<VsandGameReport> oGameReports, ref int iWins, ref int iLosses, ref int iTies, ref int iInState, string sTeamState, ref string sMsg);

        bool CalculateRecord(int TeamId, List<VsandGameReport> oGameReports, ref int iWins, ref int iLosses, ref int iTies, ref int iConfWins, ref int iConfLosses, ref int iConfTies, ref int iInState, string sTeamState, ref string sMsg);

        bool CalculateRecord(int TeamId, List<VsandGameReport> oGameReports, ref int iWins, ref int iLosses, ref int iTies, ref int iConfWins, ref int iConfLosses, ref int iConfTies, ref int iDivWins, ref int iDivLosses, ref int iDivTies, ref int iLeagueWins, ref int iLeagueLosses, ref int iLeagueTies, ref int iHomeWins, ref int iHomeLosses, ref int iHomeTies, ref int iRoadWins, ref int iRoadLosses, ref int iRoadTies, ref int iInState, string sTeamState, ref int iPointsFor, ref int iPointsAgainst, ref string sMsg);

        vsandHelper.Reports.TeamRecordInfoDataTable TeamRecordInfoDataSet(List<TeamRecordInfo> oTeamRecInfo);

        vsandHelper.Reports.TeamComplianceInfoDataTable TeamComplianceInfoDataSet(List<Helper.TeamComplianceInfo> oTeamCompInfo);

        vsandHelper.Reports.TeamClassificationInfoDataTable TeamClassificationInfoDataSet(List<Helper.TeamClassificationInfo> oTeamClassInfo);
        */

        List<VsandTeam> GetSportsTeams(int SportId, int ScheduleYearId);

        List<VsandTeam> GetSportsTeamsWithGames(int sportId, int ScheduleYearId);

        List<VsandTeam> GetSportsTeamsWithCodes(int SportId, int ScheduleYearId);

        List<VsandTeam> GetInStateSportsTeams(int SportId, int ScheduleYearId, string State);

        List<VsandTeam> GetInStateSportsTeamsWithCustomCodes(int SportId, int ScheduleYearId, string State);

        List<VsandTeam> GetTeamsByConference(int SportId, int ScheduleYearId, string Conference);

        List<VsandTeam> GetTeamsWithoutConference(int SportId, int ScheduleYearId, string State);

        List<VsandTeam> GetTeamsByTeamCustomCode(int SportId, int ScheduleYearId, string CodeName, string CodeValue);

        List<VsandTeam> GetTeamsBySchoolCustomCode(int SportId, int ScheduleYearId, string CodeName, string CodeValue);

        List<VsandTeam> GetTeamsBySportEventTypeRound(int RoundId);

        List<VsandTeam> GetTeamsBySelfAdvancingRounds(int RoundId);

        bool ValidateTeam(int TeamId, int UserId, string Username, bool SuppressEvents, ref string sMsg);

        bool UnValidateTeam(int TeamId, int UserId, string Username, ref string sMsg);

        List<VsandTeam> GetUnusedTeams(int ScheduleYearId, int SportId);

        List<VsandTeam> GetUnvalidatedTeamsInGames(int ScheduleYearId);

        /// <summary>
        /// Interface definition for default page of latest teams
        /// </summary>
        Task<PagedResult<TeamSummary>> GetLatestTeamsPagedAsync(SearchRequest oSearch);

        bool DeleteTeam(int TeamId, int UserId, string Username, ref string sMsg);

        bool MergeTeams(int TargetTeamId, int SourceTeamId, SortedList<int, int> RosterResolver, ref string sMsg, ApplicationUser user);
    }
}
