using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class AppxMailingList
    {
        public int MailingListId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Frequency { get; set; }
        public bool PublicSubscription { get; set; }
    }
}
