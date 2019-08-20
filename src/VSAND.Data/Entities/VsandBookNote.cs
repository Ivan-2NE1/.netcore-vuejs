using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandBookNote
    {
        public int NoteId { get; set; }
        public int ScheduleYearId { get; set; }
        public int SportId { get; set; }
        public int? GameReportId { get; set; }
        public int? TeamId { get; set; }
        public int? PlayerId { get; set; }
        public int RevisionId { get; set; }
        public int NoteById { get; set; }
        public string NoteBy { get; set; }
        public DateTime NoteDate { get; set; }
        public string Note { get; set; }
        public int BookId { get; set; }

        public VsandBook Book { get; set; }
        public VsandGameReport GameReport { get; set; }
        public VsandPlayer Player { get; set; }
        public VsandScheduleYear ScheduleYear { get; set; }
        public VsandSport Sport { get; set; }
        public VsandTeam Team { get; set; }
    }
}
