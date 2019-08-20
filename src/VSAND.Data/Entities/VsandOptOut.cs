using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandOptOut
    {
        public string EmailAddress { get; set; }
        public DateTime OptOutDate { get; set; }
        public string Ipaddress { get; set; }
    }
}
