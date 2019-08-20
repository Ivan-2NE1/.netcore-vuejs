using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class AppxMessageBlastCampaignSend
    {
        public int SendId { get; set; }
        public int CampaignId { get; set; }
        public string TrackingNumber { get; set; }
        public string SentBy { get; set; }
        public int SentById { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
