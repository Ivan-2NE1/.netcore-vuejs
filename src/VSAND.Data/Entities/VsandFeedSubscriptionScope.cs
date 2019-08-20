using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandFeedSubscriptionScope
    {
        public int ScopeId { get; set; }
        public int FeedSubscriptionId { get; set; }
        public int ScopeTypeId { get; set; }
        public int InScopeObjectId { get; set; }

        public VsandFeedSubscription FeedSubscription { get; set; }
        public VsandFeedSubscriptionScopeType ScopeType { get; set; }
    }
}
