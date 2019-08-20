using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandTeamSchedule
    {
        public VsandTeamSchedule()
        {
            VsandTeamScheduleTeam = new HashSet<VsandTeamScheduleTeam>();
        }

        public int ScheduleId { get; set; }
        public int TeamId { get; set; }
        public int? EventMonth { get; set; }
        public int? EventDay { get; set; }
        public int? EventYear { get; set; }
        public int? EventHour { get; set; }
        public int? EventMin { get; set; }
        public int? OpponentId { get; set; }
        public string OpponentName { get; set; }
        public int? HomeAwayFlag { get; set; }
        public string Location { get; set; }
        public int GameType { get; set; }
        public int? TournamentId { get; set; }
        public string TournamentName { get; set; }
        public int? RoundId { get; set; }
        public int? SectionId { get; set; }
        public int? GroupId { get; set; }
        public bool TriPlus { get; set; }

        public VsandTeam Opponent { get; set; }
        public VsandTeam Team { get; set; }
        public ICollection<VsandTeamScheduleTeam> VsandTeamScheduleTeam { get; set; }
    }
}
