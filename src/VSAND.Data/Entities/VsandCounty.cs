using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VSAND.Data.Entities
{
    public partial class VsandCounty
    {
        public VsandCounty()
        {
            GameReports = new HashSet<VsandGameReport>();
            Schools = new HashSet<VsandSchool>();
        }

        public int CountyId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        
        [Required]
        [MaxLength(2)]
        public string State { get; set; }

        [Required]
        [MaxLength(2)]
        [DisplayName("County Abbreviation")]
        public string CountyAbbr { get; set; }

        public ICollection<VsandGameReport> GameReports { get; set; }
        public ICollection<VsandSchool> Schools { get; set; }
    }
}
