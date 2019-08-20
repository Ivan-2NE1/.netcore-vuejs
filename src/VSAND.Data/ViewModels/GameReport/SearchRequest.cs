using System;
using System.Collections.Generic;
using System.Text;

namespace VSAND.Data.ViewModels.GameReport
{
    public class SearchRequest
    {
        public int? SchoolId { get; set; }
        public List<int> Sports { get; set; }
        public List<int> Counties { get; set; }
        public List<string> Conferences { get; set; }
        public int? ScheduleYearId { get; set; }
        public DateTime? GameDate { get; set; }
        public int PageSize { get; set; } = 50;
        public int PageNumber { get; set; } = 1;

    }
}
