using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandBookSubscription
    {
        public int SubscriptionId { get; set; }
        public int BookId { get; set; }
        public int AdminId { get; set; }

        public AppxUser Admin { get; set; }
        public VsandBook Book { get; set; }
    }
}
