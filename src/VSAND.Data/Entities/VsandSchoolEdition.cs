using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandSchoolEdition
    {
        public int SchoolEditionId { get; set; }
        public int SchoolId { get; set; }
        public int EditionId { get; set; }

        public VsandEdition Edition { get; set; }
        public VsandSchool School { get; set; }
    }
}
