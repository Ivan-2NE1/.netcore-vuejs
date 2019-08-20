using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class AppxCmsContentMessage
    {
        public int ContentMessageId { get; set; }
        public string PageRef { get; set; }
        public string Placeholder { get; set; }
        public string Description { get; set; }
        public string ContentClass { get; set; }
        public string Content { get; set; }
    }
}
