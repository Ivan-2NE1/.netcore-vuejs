using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandScheduleLoadImportSource
    {
        public int ImportSourceId { get; set; }
        public string Name { get; set; }
        public string SchoolListSource { get; set; }
        public string SchoolListSourceType { get; set; }
        public string SportListSource { get; set; }
        public string SportListSourceType { get; set; }
        public string RetrieveUrl { get; set; }
        public string RetrieveUrltype { get; set; }
    }
}
