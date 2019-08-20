using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class AppxUserInfo
    {
        public int UserInfoId { get; set; }
        public int AdminId { get; set; }
        public string Value { get; set; }
    }
}
