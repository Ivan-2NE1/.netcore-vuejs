using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class AppxMessageBlastCampaignDistribution
    {
        public int CampaignDistributionId { get; set; }
        public int CampaignId { get; set; }
        public int DistributionListId { get; set; }
    }
}
