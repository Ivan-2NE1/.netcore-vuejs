using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandBookFav
    {
        public int BookFavId { get; set; }
        public int SportId { get; set; }
        public int SchoolId { get; set; }
        public int AdminId { get; set; }

        public AppxUser Admin { get; set; }
        public VsandSchool School { get; set; }
        public VsandSport Sport { get; set; }
    }
}
