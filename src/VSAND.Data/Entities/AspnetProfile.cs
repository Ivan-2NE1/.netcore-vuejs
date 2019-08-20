using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class AspnetProfile
    {
        public Guid UserId { get; set; }
        public string PropertyNames { get; set; }
        public string PropertyValuesString { get; set; }
        public byte[] PropertyValuesBinary { get; set; }
        public DateTime LastUpdatedDate { get; set; }

        public AspnetUsers User { get; set; }
    }
}
