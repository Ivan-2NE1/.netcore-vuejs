using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandScheduleLoadFileParse
    {
        public int FileParseId { get; set; }
        public int FileId { get; set; }
        public string TeamSchoolName { get; set; }
        public int? TeamSchoolId { get; set; }
        public int? TeamId { get; set; }
        public string OpponentSchoolName { get; set; }
        public int? OpponentSchoolId { get; set; }
        public int? OpponentTeamId { get; set; }
        public DateTime? EventDate { get; set; }
        public string HomeAway { get; set; }
        public string Venue { get; set; }
        public int? SourceId { get; set; }
        public string SourceType { get; set; }
        public int? ScheduleId { get; set; }

        public VsandScheduleLoadFile File { get; set; }
        public VsandSchool OpponentSchool { get; set; }
        public VsandTeam OpponentTeam { get; set; }
        public VsandTeam Team { get; set; }
        public VsandSchool TeamSchool { get; set; }
    }
}
