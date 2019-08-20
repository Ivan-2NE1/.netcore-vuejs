using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandSystemFormat
    {
        public VsandSystemFormat()
        {
            PublicationSportSubscriptions = new HashSet<VsandPublicationSportSubscription>();
        }

        public int FormatId { get; set; }
        public string FormatType { get; set; }
        public string FormatClass { get; set; }
        public int SportId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }

        public VsandSport Sport { get; set; }
        public ICollection<VsandPublicationSportSubscription> PublicationSportSubscriptions { get; set; }
    }
}
