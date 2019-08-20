using System;

namespace VSAND.Data.Entities
{
    public partial class AppxAudit
    {
        public int AuditId { get; set; }
        public string AuditTable { get; set; }
        public int AuditKey { get; set; }
        public string AuditAction { get; set; }
        public string AuditUser { get; set; }
        public int AuditUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string AuditData { get; set; }
    }
}
