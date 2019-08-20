using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class AppxMessageBlastCampaignSendStatus
    {
        public int StatusId { get; set; }
        public int SendId { get; set; }
        public DateTime StatusDate { get; set; }
        public bool ErrorStatus { get; set; }
        public string StatusMsg { get; set; }
    }
}
