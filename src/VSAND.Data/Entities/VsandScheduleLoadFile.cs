using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandScheduleLoadFile
    {
        public VsandScheduleLoadFile()
        {
            FileRows = new HashSet<VsandScheduleLoadFileParse>();
        }

        public int FileId { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public int? FileSize { get; set; }
        public string FullName { get; set; }
        public DateTime? DateLoaded { get; set; }
        public int ScheduleYearId { get; set; }
        public int SportId { get; set; }
        public DateTime? ImportDate { get; set; }
        public bool? Deleted { get; set; }

        public VsandScheduleYear ScheduleYear { get; set; }
        public VsandSport Sport { get; set; }
        public ICollection<VsandScheduleLoadFileParse> FileRows { get; set; }
    }
}
