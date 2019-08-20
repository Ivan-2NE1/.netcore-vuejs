using System;
using System.Collections.Generic;
using System.Text;

namespace VSAND.Data.ViewModels.ScheduleYears
{
    public class ScheduleFileResolvedItem
    {
        public string Name { get; set; }
        public int SchoolId { get; set; }
        public string RenameTo { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public  bool PrivateSchool { get; set; }
        public int CountyId { get; set; }
        public bool SkipCreation { get; set; }

        public ScheduleFileResolvedItem()
        {

        }
    }
}
