using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandSportEventType
    {
        public VsandSportEventType()
        {
            GameReports = new HashSet<VsandGameReport>();
            VsandSportEventTypeAlias = new HashSet<VsandSportEventTypeAlias>();
            Rounds = new HashSet<VsandSportEventTypeRound>();
            Sections = new HashSet<VsandSportEventTypeSection>();
        }

        public int EventTypeId { get; set; }
        public int SportId { get; set; }
        public int? ScheduleYearId { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string Venue { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int ScoreboardTypeId { get; set; }
        public bool? DefaultSelected { get; set; }
        public int? SortOrder { get; set; }
        public string SectionLabel { get; set; }
        public string GroupLabel { get; set; }
        public string CustomCodeName { get; set; }
        public int? CustomFormId { get; set; }
        public string ParticpatingTeamsType { get; set; }
        public string ParticipatingTeamsFilter { get; set; }

        public VsandScheduleYear ScheduleYear { get; set; }
        public VsandSport Sport { get; set; }
        public ICollection<VsandGameReport> GameReports { get; set; }
        public ICollection<VsandSportEventTypeAlias> VsandSportEventTypeAlias { get; set; }
        public ICollection<VsandSportEventTypeRound> Rounds { get; set; }
        public ICollection<VsandSportEventTypeSection> Sections { get; set; }
    }
}
