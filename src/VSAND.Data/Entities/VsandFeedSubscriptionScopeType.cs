using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandFeedSubscriptionScopeType
    {
        public VsandFeedSubscriptionScopeType()
        {
            VsandFeedSubscription = new HashSet<VsandFeedSubscription>();
            VsandFeedSubscriptionScope = new HashSet<VsandFeedSubscriptionScope>();
        }

        public int ScopeTypeId { get; set; }
        public string Name { get; set; }

        public ICollection<VsandFeedSubscription> VsandFeedSubscription { get; set; }
        public ICollection<VsandFeedSubscriptionScope> VsandFeedSubscriptionScope { get; set; }
    }
}
