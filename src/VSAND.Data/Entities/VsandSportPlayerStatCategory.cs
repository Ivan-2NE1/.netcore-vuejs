using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VSAND.Data.Entities
{
    public partial class VsandSportPlayerStatCategory
    {
        public VsandSportPlayerStatCategory()
        {
            PlayerStats = new HashSet<VsandSportPlayerStat>();
        }

        public int SportPlayerStatCategoryId { get; set; }

        [Required]
        public int SportId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public int SortOrder { get; set; }

        public int? DefaultSortStatId { get; set; }

        public VsandSport Sport { get; set; }

        public ICollection<VsandSportPlayerStat> PlayerStats { get; set; }
    }
}
