using System;
using System.Collections.Generic;

namespace VSAND.Data.Integrations.LocalLive
{
    public class Schedule
    {
        public int NJScheduleID { get; set; }
        public DateTime LastUpdated { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int EventsCount { get; set; }
        public string Version { get; set; }
        public List<Event> Events { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}
