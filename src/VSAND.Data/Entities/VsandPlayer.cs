using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VSAND.Data.Entities
{
    public partial class VsandPlayer
    {
        public VsandPlayer()
        {
            VsandBookNote = new HashSet<VsandBookNote>();
            GameReportEventPlayer = new HashSet<VsandGameReportEventPlayer>();
            GameReportEventPlayerGroupPlayer = new HashSet<VsandGameReportEventPlayerGroupPlayer>();
            GameReportPlayerStats = new HashSet<VsandGameReportPlayerStat>();
            GameReportRosters = new HashSet<VsandGameReportRoster>();
            VsandPlayerRecruiting = new HashSet<VsandPlayerRecruiting>();
            TeamRosters = new HashSet<VsandTeamRoster>();
        }

        public VsandPlayer(VsandTeamRoster teamRoster)
        {
            this.FirstName = teamRoster.Player.FirstName;
            this.LastName = teamRoster.Player.LastName;
            this.GraduationYear = teamRoster.Player.GraduationYear;
            this.PlayerId = teamRoster.Player.PlayerId;
            this.Validated = teamRoster.Player.Validated;
            this.ValidatedBy = teamRoster.Player.ValidatedBy;
            this.ValidatedById = teamRoster.Player.ValidatedById;

        }

        public int PlayerId { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        public int GraduationYear { get; set; }

        [MaxLength(10)]
        public string Height { get; set; }

        public int? Weight { get; set; }

        public DateTime? BirthDate { get; set; }

        public bool? Active { get; set; }

        public int? SchoolId { get; set; }

        public bool Validated { get; set; }

        public string ValidatedBy { get; set; }

        public int? ValidatedById { get; set; }

        public VsandSchool School { get; set; }
        public ICollection<VsandBookNote> VsandBookNote { get; set; }
        public ICollection<VsandGameReportEventPlayer> GameReportEventPlayer { get; set; }
        public ICollection<VsandGameReportEventPlayerGroupPlayer> GameReportEventPlayerGroupPlayer { get; set; }
        public ICollection<VsandGameReportPlayerStat> GameReportPlayerStats { get; set; }
        public ICollection<VsandGameReportRoster> GameReportRosters { get; set; }
        public ICollection<VsandPlayerRecruiting> VsandPlayerRecruiting { get; set; }
        public ICollection<VsandTeamRoster> TeamRosters { get; set; }

        /// <summary>
        /// Given a school year, determine which class name the player belonged to (Senior, Junior, etc) using their graduation year
        /// </summary>
        /// <param name="contextYear">Full 4-digit year to check against</param>
        /// <returns>String. One of Senior, Junior, Sophomore, Freshman, Graduated: [year]</returns>     
        public string ClassYear(int contextYear)
        {
            return Common.ClassYearHelp.GraduationYearToClass(GraduationYear, contextYear);            
        }

        public string ClassYearAbbr(int contextYear)
        {
            return Common.ClassYearHelp.GraduationYearToClassAbbr(GraduationYear, contextYear);
        }
    }
}
