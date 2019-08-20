using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class AppxMessageBlastCampaignSendRecipient
    {
        public int RecipientId { get; set; }
        public int SendId { get; set; }
        public string RecipientEmail { get; set; }
        public bool? MessageSent { get; set; }
        public DateTime? MessageSentDate { get; set; }
        public int? ReadCount { get; set; }
        public bool? Bounced { get; set; }
        public string BounceData { get; set; }
        public int? ClickthroughCount { get; set; }
        public string RecipientData { get; set; }
    }
}
