using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandGameReportPairingTeam
    {
        public int PairingTeamId { get; set; }
        public int PairingId { get; set; }
        public int GameReportTeamId { get; set; }
        public double Score { get; set; }

        public VsandGameReportTeam GameReportTeam { get; set; }
        public VsandGameReportPairing Pairing { get; set; }
    }
}
