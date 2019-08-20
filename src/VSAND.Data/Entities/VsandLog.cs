using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandLog
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Thread { get; set; }
        public string ErrorLevel { get; set; }
        public string Logger { get; set; }
        public string RequestUrl { get; set; }
        public string UserIdentity { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        public bool Acknowledged { get; set; }
        public string AcknowledgedUser { get; set; }
        public DateTime? AcknowledgedDate { get; set; }
    }
}
