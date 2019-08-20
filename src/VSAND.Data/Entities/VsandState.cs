using System.ComponentModel.DataAnnotations;

namespace VSAND.Data.Entities
{
    public partial class VsandState
    {
        public int StateId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(2)]
        public string Abbreviation { get; set; }

        [Required]
        [MaxLength(10)]
        [Display(Name = "Publication Abbreviation")]
        public string PubAbbreviation { get; set; }
    }
}
