using System;
using System.Collections.Generic;
using System.Text;
using VSAND.Data.Entities;

namespace VSAND.Data.ViewModels.GameReport
{
    public class GameReportEventPlayerGroupPlayer
    {
        public int PlayerGroupPlayerId { get; set; }
        public int PlayerGroupId { get; set; }
        public int? PlayerId { get; set; }
        public int? SortOrder { get; set; }

        public GameReportEventPlayerGroupPlayer()
        {

        }

        public GameReportEventPlayerGroupPlayer(VsandGameReportEventPlayerGroupPlayer grepgp)
        {
            this.PlayerGroupPlayerId = grepgp.PlayerGroupPlayerId;
            this.PlayerGroupId = grepgp.PlayerGroupId;
            this.PlayerId = grepgp.PlayerId;
            this.SortOrder = grepgp.SortOrder;
        }
    }
}
