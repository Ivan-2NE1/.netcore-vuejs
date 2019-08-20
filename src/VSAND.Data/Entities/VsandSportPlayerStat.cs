using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VSAND.Data.Entities
{
    public partial class VsandSportPlayerStat
    {
        public VsandSportPlayerStat()
        {
            GameReportPlayerStats = new HashSet<VsandGameReportPlayerStat>();
        }

        public int SportPlayerStatId { get; set; }

        [Required]
        public int SportPlayerStatCategoryId { get; set; }

        [Required]
        public int SportId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string Abbreviation { get; set; }

        [Required]
        [MaxLength(50)]
        public string DataType { get; set; }

        [Required]
        public int SortOrder { get; set; }

        [Required]
        public bool Enabled { get; set; }

        public bool? Calculated { get; set; }

        [NotMapped]
        public string DisplayName
        {
            get
            {
                if (Abbreviation != null && !string.IsNullOrEmpty(Abbreviation))
                {
                    return Abbreviation;
                }
                return Name;
            }
        }

        public VsandSport Sport { get; set; }
        public VsandSportPlayerStatCategory SportPlayerStatCategory { get; set; }
        public ICollection<VsandGameReportPlayerStat> GameReportPlayerStats { get; set; }
    }
}
