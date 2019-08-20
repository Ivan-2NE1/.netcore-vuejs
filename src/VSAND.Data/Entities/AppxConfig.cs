using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class AppxConfig
    {
        public int ConfigId { get; set; }
        public string ConfigCat { get; set; }
        public string ConfigName { get; set; }
        public string ConfigVal { get; set; }
    }
}
