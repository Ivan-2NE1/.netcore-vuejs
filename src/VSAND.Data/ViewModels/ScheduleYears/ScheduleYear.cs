using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using VSAND.Data.Entities;

namespace VSAND.Data.ViewModels
{
    public class ScheduleYear
    {
        [DisplayName("Schedule Year Id")]
        public int ScheduleYearId { get; }

        [Required]
        [MaxLength(10)]
        public string Name { get; set; }

        [Required]
        [DisplayName("End Year")]
        public int EndYear { get; set; }

        public bool Active { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<ScheduleYearSummary> Summary { get; set; } = null;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<ScheduleYearProvisioningSummary> ProvisioningSummary { get; set; } = null;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ListItem<int> SportInfo { get; set; } = null;

        public ScheduleYear()
        {

        }

        public ScheduleYear(VsandScheduleYear sy)
        {
            ScheduleYearId = sy.ScheduleYearId;
            Name = sy.Name;
            EndYear = sy.EndYear.HasValue ? sy.EndYear.Value : 0;
            Active = sy.Active.HasValue ? sy.Active.Value : false;
        }
    }
}
