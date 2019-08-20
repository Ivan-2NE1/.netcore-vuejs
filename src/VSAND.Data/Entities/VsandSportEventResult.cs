using System.ComponentModel.DataAnnotations;

namespace VSAND.Data.Entities
{
    public partial class VsandSportEventResult
    {
        public int SportEventResultId { get; set; }

        [Required]
        public int SportId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public int SortOrder { get; set; }

        [Required]
        public bool IsTie { get; set; }

        public VsandSport Sport { get; set; }
    }
}
