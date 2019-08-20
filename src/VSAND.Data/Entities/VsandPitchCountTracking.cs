using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandPitchCountTracking
    {
        public int TrackingId { get; set; }
        public int PlayerId { get; set; }
        public int TeamId { get; set; }
        public DateTime GameDate { get; set; }
        public int GameType { get; set; }
        public int Pit { get; set; }
    }
}
