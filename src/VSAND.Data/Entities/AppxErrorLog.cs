using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class AppxErrorLog
    {
        public int ErrorId { get; set; }
        public DateTime? ErrorDate { get; set; }
        public bool? Acknowledged { get; set; }
        public string AcknowledgedUser { get; set; }
        public string ErrorClass { get; set; }
        public string ErrorMessage { get; set; }
    }
}
