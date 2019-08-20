using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class AppxCmsContent
    {
        public int ContentId { get; set; }
        public string PageRef { get; set; }
        public string ContentArea { get; set; }
        public string ContentData { get; set; }
    }
}
