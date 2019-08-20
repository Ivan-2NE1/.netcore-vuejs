using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class AppxReferrerReferral
    {
        public int Rid { get; set; }
        public string Rkid { get; set; }
        public DateTime ReferralDate { get; set; }
        public string Ipaddress { get; set; }
        public string Browser { get; set; }
    }
}
