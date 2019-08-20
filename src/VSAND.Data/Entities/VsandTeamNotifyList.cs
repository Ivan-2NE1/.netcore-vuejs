using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandTeamNotifyList
    {
        public int NotifyId { get; set; }
        public int SportId { get; set; }
        public int SchoolId { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }

        public VsandSchool School { get; set; }
        public VsandSport Sport { get; set; }
    }
}
