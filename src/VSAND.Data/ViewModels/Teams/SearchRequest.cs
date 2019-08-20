using System;
using System.Collections.Generic;
using System.Text;

namespace VSAND.Data.ViewModels.Teams
{
    public class SearchRequest
    {
        public int? SchoolId { get; set; }
        public List<int> Sports { get; set; }
        public int? ScheduleYearId { get; set; }
        public int PageSize { get; set; } = 500;
        public int PageNumber { get; set; } = 1;
    }
}
