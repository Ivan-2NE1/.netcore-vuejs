using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class AppxCmsContentVirtual
    {
        public int VcontentId { get; set; }
        public string PageTemplate { get; set; }
        public string PageRef { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string ExpirationAction { get; set; }
        public string ExpirationContent { get; set; }
        public string ContentData { get; set; }
    }
}
