using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandPublicationSchool
    {
        public int PublicationSchoolId { get; set; }
        public int PublicationId { get; set; }
        public int SchoolId { get; set; }

        public VsandPublication Publication { get; set; }
        public VsandSchool School { get; set; }
    }
}
