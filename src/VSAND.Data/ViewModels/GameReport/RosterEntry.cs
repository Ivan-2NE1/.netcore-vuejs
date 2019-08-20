using System;
using System.Collections.Generic;
using System.Text;
using VSAND.Data.Entities;

namespace VSAND.Data.ViewModels.GameReport
{
    public class RosterEntry
    {
        public int PlayerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JerseyNumber { get; set; }
        public int? Position1 { get; set; }
        public int? Position2 { get; set; }

        public string DisplayName
        {
            get
            {
                var name = (LastName + ", " + FirstName).Trim();
                if (name.EndsWith(","))
                {
                    name = LastName;
                }
                return name;
            }
        }
        public RosterEntry()
        {
            
        }

        public RosterEntry(VsandTeamRoster roster)
        {
            PlayerId = roster.PlayerId;
            if (roster.Player != null)
            {
                FirstName = string.IsNullOrWhiteSpace(roster.Player.FirstName) ? "" : roster.Player.FirstName.Trim();
                LastName = string.IsNullOrWhiteSpace(roster.Player.LastName) ? "" : roster.Player.LastName.Trim();
            }
            JerseyNumber = roster.JerseyNumber;
            Position1 = roster.Position;
            Position2 = roster.Position2;
            
        }
    }
}
