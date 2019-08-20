using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSAND.Data.Entities;

namespace VSAND.Data.ViewModels.Teams
{
    public class FullTeam
    {
        public int TeamId { get; }
        public int SchoolId { get; }
        public int SportId { get; }
        public int ScheduleYearId { get; }
        public string Name { get; }
        public string Sport { get; }
        public string Season { get; }
        public string Logo { get; }
        public TeamRecord OverallRecord { get; }
        public TeamRecord LeagueRecord { get; }
        public string Conference { get; }
        public string Division { get; }
        public string League { get; }
        public string Classification { get; }
        public bool PowerPointsEnabled { get; }
        
        public FullTeam()
        {

        }

        public FullTeam(VsandTeam team, List<VsandTeamCustomCode> customCodes)
        {
            TeamId = team.TeamId;
            SchoolId = team.SchoolId.Value;
            SportId = team.SportId;
            ScheduleYearId = team.ScheduleYearId;
            Name = team.Name;
            Sport = team.Sport.Name;
            Season = team.ScheduleYear.Name;
            Logo = team.School.Graphic;
            PowerPointsEnabled = team.Sport.EnablePowerPoints.HasValue ? team.Sport.EnablePowerPoints.Value : false;

            // Try to add in their overall record
            VsandTeamCustomCode overallWins = customCodes.FirstOrDefault(tcc => tcc.CodeName == "Record-Overall-Wins");
            VsandTeamCustomCode overallLosses = customCodes.FirstOrDefault(tcc => tcc.CodeName == "Record-Overall-Losses");
            VsandTeamCustomCode overallTies = customCodes.FirstOrDefault(tcc => tcc.CodeName == "Record-Overall-Ties");
            OverallRecord = new TeamRecord(overallWins, overallLosses, overallTies);

            // Try to add in their league record
            VsandTeamCustomCode leagueWins = customCodes.FirstOrDefault(tcc => tcc.CodeName == "Record-League-Wins");
            VsandTeamCustomCode leagueLosses = customCodes.FirstOrDefault(tcc => tcc.CodeName == "Record-League-Losses");
            VsandTeamCustomCode leagueTies = customCodes.FirstOrDefault(tcc => tcc.CodeName == "Record-League-Ties");
            LeagueRecord = new TeamRecord(leagueWins, leagueLosses, leagueTies);

            VsandTeamCustomCode conference = customCodes.FirstOrDefault(tcc => tcc.CodeName == "Conference");
            if (conference != null)
            {
                Conference = conference.GetValue<string>();
            }
            VsandTeamCustomCode division = customCodes.FirstOrDefault(tcc => tcc.CodeName == "Division");
            if (division != null)
            {
                Division = division.GetValue<string>();
            }
            List<string> leagueName = new List<string>();
            if (!string.IsNullOrEmpty(Conference))
            {
                leagueName.Add(Conference);
            }
            if (!string.IsNullOrEmpty(Division))
            {
                leagueName.Add(Division);
            }
            League = string.Join(" - ", leagueName);

            VsandTeamCustomCode section = customCodes.FirstOrDefault(tcc => tcc.CodeName == "Section");
            VsandTeamCustomCode group = customCodes.FirstOrDefault(tcc => tcc.CodeName == "Group");
            string sectionName = section.GetValue<string>();
            string groupName = group.GetValue<string>();
            List<string> className = new List<string>();
            if (!string.IsNullOrEmpty(sectionName))
            {
                className.Add(sectionName);
            }
            if (!string.IsNullOrEmpty(groupName))
            {
                className.Add(groupName);
            }
            Classification = string.Join(", ", className);
        }
    }
}
