using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandBookMember
    {
        public int BookMemberId { get; set; }
        public int BookId { get; set; }
        public int SchoolId { get; set; }
        public bool IsActive { get; set; }

        public VsandBook Book { get; set; }
        public VsandSchool School { get; set; }
    }
}
