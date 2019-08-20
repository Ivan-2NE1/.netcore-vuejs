using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandPublicationRouteCode
    {
        public int PublicationRouteCodeId { get; set; }
        public int PublicationId { get; set; }
        public string RoutingCode { get; set; }
        public int SortOrder { get; set; }

        public VsandPublication Publication { get; set; }
    }
}
