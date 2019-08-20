using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandEdition
    {
        public VsandEdition()
        {
            VsandPublicationEditionSubscription = new HashSet<VsandPublicationEditionSubscription>();
            VsandRoundup = new HashSet<VsandRoundup>();
            VsandSchoolEdition = new HashSet<VsandSchoolEdition>();
        }

        public int EditionId { get; set; }
        public string Name { get; set; }

        public ICollection<VsandPublicationEditionSubscription> VsandPublicationEditionSubscription { get; set; }
        public ICollection<VsandRoundup> VsandRoundup { get; set; }
        public ICollection<VsandSchoolEdition> VsandSchoolEdition { get; set; }
    }
}
