using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VSAND.Data.Entities
{
    public partial class VsandSportTeamStatCategory
    {
        public VsandSportTeamStatCategory()
        {
            TeamStats = new HashSet<VsandSportTeamStat>();
        }

        public int SportTeamStatCategoryId { get; set; }

        [Required]
        public int SportId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public int SortOrder { get; set; }

        public int? DefaultSortStatId { get; set; }

        public VsandSport Sport { get; set; }

        public ICollection<VsandSportTeamStat> TeamStats { get; set; }
    }
}
