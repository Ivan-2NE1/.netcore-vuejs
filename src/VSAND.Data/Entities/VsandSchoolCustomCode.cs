using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandSchoolCustomCode
    {
        public int CustomCodeId { get; set; }
        public string CodeName { get; set; }
        public string CodeValue { get; set; }
        public int SchoolId { get; set; }
        public int? SportId { get; set; }

        public VsandSchool School { get; set; }
        public VsandSport Sport { get; set; }
    }
}
