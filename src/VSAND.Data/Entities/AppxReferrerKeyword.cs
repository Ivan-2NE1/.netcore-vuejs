using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class AppxReferrerKeyword
    {
        public int Rkid { get; set; }
        public int Ruid { get; set; }
        public string Keyword { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
