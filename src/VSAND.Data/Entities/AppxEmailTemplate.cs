using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class AppxEmailTemplate
    {
        public int EmailId { get; set; }
        public string DisplayName { get; set; }
        public string EmailType { get; set; }
        public string ToAddress { get; set; }
        public string FromAddress { get; set; }
        public string ReplyToAddress { get; set; }
        public string Cclist { get; set; }
        public string Bcclist { get; set; }
        public string Subject { get; set; }
        public bool? IsHtml { get; set; }
        public string Body { get; set; }
    }
}
