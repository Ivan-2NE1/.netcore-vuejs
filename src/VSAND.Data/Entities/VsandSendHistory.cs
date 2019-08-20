using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandSendHistory
    {
        public int HistoryId { get; set; }
        public string ViewType { get; set; }
        public int ReferenceId { get; set; }
        public DateTime? ViewDate { get; set; }
        public int SportId { get; set; }
        public int PublicationId { get; set; }
        public string SentBy { get; set; }
        public int SentById { get; set; }
        public DateTime? SentDate { get; set; }
    }
}
