using System;

namespace VSAND.Data.Entities
{
    public partial class AppxCmsEvent
    {
        public int EventId { get; set; }
        public string EventType { get; set; }
        public string EventSubType { get; set; }
        public int EventStartMonth { get; set; }
        public int EventStartDay { get; set; }
        public int EventStartYear { get; set; }
        public int EventStartHour { get; set; }
        public int EventStartMin { get; set; }
        public DateTime? EventStartDate { get; set; }
        public int EventEndMonth { get; set; }
        public int EventEndDay { get; set; }
        public int EventEndYear { get; set; }
        public int EventEndHour { get; set; }
        public int EventEndMin { get; set; }
        public DateTime? EventEndDate { get; set; }
        public bool Enabled { get; set; }
        public string EventTitle { get; set; }
        public string EventDesc { get; set; }
        public string EventSummary { get; set; }
    }
}
