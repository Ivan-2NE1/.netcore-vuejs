using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class AppxMessageBlastCampaign
    {
        public int CampaignId { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public string RecipientTemplateField { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string ReplyTo { get; set; }
        public bool? TrackBounces { get; set; }
        public string Smtpserver { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int? Port { get; set; }
        public string Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string Htmlbody { get; set; }
        public string TextBody { get; set; }
    }
}
