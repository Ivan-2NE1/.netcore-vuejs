using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace VSAND.Data.ViewModels.ScheduleYears
{
    public class ScheduleFileUploadRequest
    {
        public int SportId { get; set; }
        public int ScheduleYearId { get; set; }
        public string SportName { get; set; }
        public string ScheduleYearName { get; set; }
        public IFormFile File { get; set; }

        public ScheduleFileUploadRequest()
        {

        }
    }
}
