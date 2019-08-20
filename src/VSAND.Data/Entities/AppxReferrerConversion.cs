using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class AppxReferrerConversion
    {
        public int Roid { get; set; }
        public int Rid { get; set; }
        public int ConversionId { get; set; }
        public string ConversionType { get; set; }
    }
}
