using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandSystemMessageSport
    {
        public int MessageSportId { get; set; }
        public int MessageId { get; set; }
        public int SportId { get; set; }

        public VsandSystemMessage Message { get; set; }
        public VsandSport Sport { get; set; }
    }
}
