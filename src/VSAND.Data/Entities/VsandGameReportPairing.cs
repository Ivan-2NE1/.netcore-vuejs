using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandGameReportPairing
    {
        public VsandGameReportPairing()
        {
            VsandGameReportPairingTeam = new HashSet<VsandGameReportPairingTeam>();
        }

        public int PairingId { get; set; }
        public int GameReportId { get; set; }

        public VsandGameReport GameReport { get; set; }
        public ICollection<VsandGameReportPairingTeam> VsandGameReportPairingTeam { get; set; }
    }
}
