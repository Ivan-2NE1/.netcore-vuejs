using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandPublicationSportSubscription
    {
        public int PubSportSubscriptionId { get; set; }
        public int PublicationId { get; set; }
        public int SportId { get; set; }
        public string SubscriptionType { get; set; }
        public int? FormatId { get; set; }
        public string SportAbbr { get; set; }
        public bool? Optional { get; set; }

        public VsandSystemFormat Format { get; set; }
        public VsandPublication Publication { get; set; }
        public VsandSport Sport { get; set; }
    }
}
