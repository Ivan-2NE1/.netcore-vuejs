using System;
using System.Collections.Generic;
using System.Text;
using VSAND.Data.Entities;

namespace VSAND.Data.ViewModels.GameReport
{
    public class GameReportRosterEntry
    {
        public int GameReportRosterId { get; set; }
        public int GameReportTeamId { get; set; }
        public int PlayerId { get; set; }
        public int? PositionId { get; set; }
        public int? RosterOrder { get; set; }
        public bool? PlayerOfRecord { get; set; }
        public int? RecordWins { get; set; }
        public int? RecordLosses { get; set; }
        public int? RecordTies { get; set; }

        public GameReportRosterEntry()
        {

        }

        public GameReportRosterEntry(VsandGameReportRoster roster)
        {
            GameReportRosterId = roster.GameReportRosterId;
            GameReportTeamId = roster.GameReportTeamId;
            PlayerId = roster.PlayerId;
            PositionId = roster.PositionId;
            RosterOrder = roster.RosterOrder;
            PlayerOfRecord = roster.PlayerOfRecord;
            RecordWins = roster.RecordWins;
            RecordLosses = roster.RecordLosses;
            RecordTies = roster.RecordTies;
        }
    }
}
