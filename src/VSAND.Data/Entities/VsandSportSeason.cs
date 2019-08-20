using System;

namespace VSAND.Data.Entities
{
    public partial class VsandSportSeason
    {
        public int SeasonId { get; set; }
        public int ScheduleYearId { get; set; }
        public int SportId { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public VsandScheduleYear ScheduleYear { get; set; }
        public VsandSport Sport { get; set; }
    }
}
