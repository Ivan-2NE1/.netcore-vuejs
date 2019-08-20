using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandFeedSubscriptionScopeDefinition
    {
        public int SubscriptionSchoolId { get; set; }
        public int FeedSubscriptionId { get; set; }
        public int SchoolId { get; set; }

        public VsandFeedSubscription FeedSubscription { get; set; }
        public VsandSchool School { get; set; }
    }
}
