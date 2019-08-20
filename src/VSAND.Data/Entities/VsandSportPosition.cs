using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VSAND.Data.Entities
{
    public partial class VsandSportPosition
    {
        public VsandSportPosition()
        {
            VsandGameReportRoster = new HashSet<VsandGameReportRoster>();
            VsandTeamRosterPosition2Navigation = new HashSet<VsandTeamRoster>();
            VsandTeamRosterPositionNavigation = new HashSet<VsandTeamRoster>();
        }

        public int SportPositionId { get; set; }

        [Required]
        public int SportId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(10)]
        public string Abbreviation { get; set; }

        public int? SortOrder { get; set; }

        public string FeaturedStatIds { get; set; }

        [NotMapped]
        public string DisplayName
        {
            get
            {
                if (Abbreviation != null && !string.IsNullOrWhiteSpace(Abbreviation))
                {
                    return Abbreviation;
                }
                return Name;
            }
        }

        public VsandSport Sport { get; set; }
        public ICollection<VsandGameReportRoster> VsandGameReportRoster { get; set; }
        public ICollection<VsandTeamRoster> VsandTeamRosterPosition2Navigation { get; set; }
        public ICollection<VsandTeamRoster> VsandTeamRosterPositionNavigation { get; set; }
    }
}
