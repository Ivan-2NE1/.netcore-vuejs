using System.ComponentModel;

namespace VSAND.Data.ViewModels
{
    public class ScheduleYearSummary
    {
        public int SportId { get; set; } = 0;
        public string Sport { get; set; } = "";
        public string Season { get; set; } = "Not Set";
        [DisplayName("Teams")]
        public int TeamCount { get; set; } = 0;
        [DisplayName("Event Types")]
        public int EventCount { get; set; } = 0;
        public bool EnablePowerPoints { get; set; } = false;
        [DisplayName("Power Points Config")]
        public bool HasPowerPointsConfig { get; set; } = false;
        [DisplayName("League Rules Config")]
        public bool HasLeagueRulesConfig { get; set; } = false;
    }
}
