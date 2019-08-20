using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VSAND.Data.Entities
{
    public partial class VsandSportEvent
    {
        public VsandSportEvent()
        {
            GameReportEvents = new HashSet<VsandGameReportEvent>();
            EventStats = new HashSet<VsandSportEventStat>();
        }

        public int SportEventId { get; set; }

        [Required]
        public int SportId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(5)]
        public string Abbreviation { get; set; }

        [Required]
        public int DefaultSort { get; set; }

        [Required]
        [MaxLength(20)]
        public string ResultType { get; set; }

        [MaxLength(100)]
        public string ResultHandler { get; set; }

        [Required]
        public bool DefaultActivated { get; set; }

        [Required]
        public int MaxResults { get; set; }

        public bool? Enabled { get; set; }

        public VsandSport Sport { get; set; }
        public ICollection<VsandGameReportEvent> GameReportEvents { get; set; }
        public ICollection<VsandSportEventStat> EventStats { get; set; }
    }
}
