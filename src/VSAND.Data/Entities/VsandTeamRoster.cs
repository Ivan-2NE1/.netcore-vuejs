using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace VSAND.Data.Entities
{
    public partial class VsandTeamRoster
    {
        public VsandTeamRoster()
        {
            VsandTeamRosterCustomCode = new HashSet<VsandTeamRosterCustomCode>();
        }

        public int RosterId { get; set; }
        public int TeamId { get; set; }
        public int PlayerId { get; set; }
        public string JerseyNumber { get; set; }
        [Obsolete("This property is deprecated. Only reference JerseyNumber going forward.")]
        public string AwayJerseyNumber { get; set; }
        public int? Position { get; set; }
        public int? Position2 { get; set; }

        public VsandPlayer Player { get; set; }
        public VsandSportPosition Position2Navigation { get; set; }
        public VsandSportPosition PositionNavigation { get; set; }
        public VsandTeam Team { get; set; }
        public ICollection<VsandTeamRosterCustomCode> VsandTeamRosterCustomCode { get; set; }

        [NotMapped]
        public List<int> PositionList
        {
            get
            {
                var positions = new List<int>();
                if (Position.HasValue)
                {
                    positions.Add(Position.Value);
                }
                if (Position2.HasValue)
                {
                    positions.Add(Position2.Value);
                }
                return positions;
            }
        }

        [NotMapped]
        public List<string> PositionNameList
        {
            get
            {
                var positions = new List<string>();
                if (Position.HasValue && PositionNavigation != null)
                {
                    positions.Add(PositionNavigation.Name);
                }
                if (Position2.HasValue && Position2Navigation != null)
                {
                    positions.Add(Position2Navigation.Name);
                }
                return positions;
            }
        }
    }
}