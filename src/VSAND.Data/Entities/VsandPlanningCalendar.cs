using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandPlanningCalendar
    {
        public int CalendarId { get; set; }
        public string Entry { get; set; }
        public int? DisplayMonth { get; set; }
        public int? DisplayDay { get; set; }
        public int? DisplayYear { get; set; }
        public DateTime? DisplayDate { get; set; }
        public int? EndMonth { get; set; }
        public int? EndDay { get; set; }
        public int? EndYear { get; set; }
        public DateTime? EndDate { get; set; }
        public string DateQualifier { get; set; }
        public int? DateType { get; set; }
        public bool Deleted { get; set; }
        public int? PublicationId { get; set; }

        public VsandPublication Publication { get; set; }
    }
}
