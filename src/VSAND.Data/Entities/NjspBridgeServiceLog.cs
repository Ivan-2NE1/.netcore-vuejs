using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class NjspBridgeServiceLog
    {
        public int BridgeMsgId { get; set; }
        public string MsmqmsgId { get; set; }
        public int Uid { get; set; }
        public int Rev { get; set; }
        public int MessageType { get; set; }
        public string MessageTypeName { get; set; }
        public DateTime? Created { get; set; }
    }
}
