using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VSAND.Data.Entities
{
    public partial class VsandSportGameMeta
    {
        public VsandSportGameMeta()
        {
            VsandGameReportMeta = new HashSet<VsandGameReportMeta>();
        }

        public int SportGameMetaId { get; set; }

        [Required]
        public int SportId { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public int SortOrder { get; set; }

        [Required]
        [MaxLength(50)]
        public string ValueType { get; set; }

        [MaxLength(200)]
        public string PromptHelp { get; set; }

        public VsandSport Sport { get; set; }
        public ICollection<VsandGameReportMeta> VsandGameReportMeta { get; set; }
    }
}
