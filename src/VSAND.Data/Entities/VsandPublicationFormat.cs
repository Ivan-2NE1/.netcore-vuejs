using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandPublicationFormat
    {
        public int FormatId { get; set; }
        public int PublicationId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? SportId { get; set; }
        public string FormatType { get; set; }
    }
}
