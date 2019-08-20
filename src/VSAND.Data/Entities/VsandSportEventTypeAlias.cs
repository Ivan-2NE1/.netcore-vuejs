using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandSportEventTypeAlias
    {
        public int EventTypeAliasId { get; set; }
        public int EventTypeId { get; set; }
        public string Name { get; set; }

        public VsandSportEventType EventType { get; set; }
    }
}
