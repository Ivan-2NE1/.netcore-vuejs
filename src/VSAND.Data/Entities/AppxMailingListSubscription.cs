using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class AppxMailingListSubscription
    {
        public int SubscriptionId { get; set; }
        public int MailingListId { get; set; }
        public int MemberId { get; set; }
        public DateTime SubscribeDate { get; set; }
        public string SubscribeIp { get; set; }
        public string ConfirmationKey { get; set; }
        public string ConfirmationIp { get; set; }
        public DateTime? ConfirmationDate { get; set; }
    }
}
