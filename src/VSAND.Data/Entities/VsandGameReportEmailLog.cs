using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandGameReportEmailLog
    {
        public int GameReportEmailId { get; set; }
        public int GameReportId { get; set; }
        public int UserId { get; set; }
        public string SendTo { get; set; }
        public string FromIp { get; set; }
        public DateTime? SentAt { get; set; }

        public VsandGameReport GameReport { get; set; }
        public AppxUser User { get; set; }
    }
}
