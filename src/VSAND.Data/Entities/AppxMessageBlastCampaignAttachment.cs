using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class AppxMessageBlastCampaignAttachment
    {
        public int AttachmentId { get; set; }
        public int CampaignId { get; set; }
        public string Name { get; set; }
    }
}
