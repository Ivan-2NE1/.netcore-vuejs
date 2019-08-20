using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandPublicationEditionSubscription
    {
        public int PublicationEditionId { get; set; }
        public int PublicationId { get; set; }
        public int EditionId { get; set; }

        public VsandEdition Edition { get; set; }
        public VsandPublication Publication { get; set; }
    }
}
