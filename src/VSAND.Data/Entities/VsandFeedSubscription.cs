using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandFeedSubscription
    {
        public VsandFeedSubscription()
        {
            VsandFeedSubscriptionScope = new HashSet<VsandFeedSubscriptionScope>();
            VsandFeedSubscriptionScopeDefinition = new HashSet<VsandFeedSubscriptionScopeDefinition>();
        }

        public int FeedSubscriptionId { get; set; }
        public string Name { get; set; }
        public string SubscriptionKey { get; set; }
        public bool Enabled { get; set; }
        public string FeedType { get; set; }
        public string SubscriptionScope { get; set; }
        public int DeliveryDelayHours { get; set; }
        public int ScopeTypeId { get; set; }

        public VsandFeedSubscriptionScopeType ScopeType { get; set; }
        public ICollection<VsandFeedSubscriptionScope> VsandFeedSubscriptionScope { get; set; }
        public ICollection<VsandFeedSubscriptionScopeDefinition> VsandFeedSubscriptionScopeDefinition { get; set; }
    }
}
