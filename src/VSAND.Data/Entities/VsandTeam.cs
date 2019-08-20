using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandTeam
    {
        public VsandTeam()
        {
            VsandBookNote = new HashSet<VsandBookNote>();
            GameReportEntries = new HashSet<VsandGameReportTeam>();
            VsandScheduleLoadFileParseOpponentTeam = new HashSet<VsandScheduleLoadFileParse>();
            VsandScheduleLoadFileParseTeam = new HashSet<VsandScheduleLoadFileParse>();
            VsandTeamContact = new HashSet<VsandTeamContact>();
            CustomCodes = new HashSet<VsandTeamCustomCode>();
            RosterEntries = new HashSet<VsandTeamRoster>();
            VsandTeamScheduleOpponent = new HashSet<VsandTeamSchedule>();
            Schedules = new HashSet<VsandTeamSchedule>();
            ScheduleTeamEntries = new HashSet<VsandTeamScheduleTeam>();
        }

        public int TeamId { get; set; }
        public string Name { get; set; }
        public int? SchoolId { get; set; }
        public int SportId { get; set; }
        public int ScheduleYearId { get; set; }
        public string Superintendent { get; set; }
        public string Principal { get; set; }
        public string AthleticDirector { get; set; }
        public string HeadCoach { get; set; }
        public string AssistantCoaches { get; set; }
        public string TeamNickname { get; set; }
        public string TeamColors { get; set; }
        public bool Validated { get; set; }
        public string ValidatedBy { get; set; }
        public int? ValidatedById { get; set; }

        public VsandScheduleYear ScheduleYear { get; set; }
        public VsandSchool School { get; set; }
        public VsandSport Sport { get; set; }
        public ICollection<VsandBookNote> VsandBookNote { get; set; }
        public ICollection<VsandGameReportTeam> GameReportEntries { get; set; }
        public ICollection<VsandScheduleLoadFileParse> VsandScheduleLoadFileParseOpponentTeam { get; set; }
        public ICollection<VsandScheduleLoadFileParse> VsandScheduleLoadFileParseTeam { get; set; }
        public ICollection<VsandTeamContact> VsandTeamContact { get; set; }
        public ICollection<VsandTeamCustomCode> CustomCodes { get; set; }
        public ICollection<VsandTeamRoster> RosterEntries { get; set; }
        public ICollection<VsandTeamSchedule> VsandTeamScheduleOpponent { get; set; }
        public ICollection<VsandTeamSchedule> Schedules { get; set; }
        public ICollection<VsandTeamScheduleTeam> ScheduleTeamEntries { get; set; }
    }
}
