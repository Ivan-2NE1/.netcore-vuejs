using System;
using System.Collections.Generic;
using System.Text;
using VSAND.Data.Entities;

namespace VSAND.Data.ViewModels.Teams
{
    public class TeamRoster
    {
        public int RosterId { get; set; }
        public int TeamId { get; set; }
        public int PlayerId { get; set; }
        public string PlayerName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int GraduationYear { get; set; }
        public string Height { get; set; }
        public string Class { get; set; }
        public string JerseyNumber { get; set; }
        public int? Position { get; set; }
        public int? Position2 { get; set; }
        public string PositionName { get; set; } = "";
        public string Position2Name { get; set; } = "";

        public TeamRoster()
        {

        }
        public TeamRoster(int teamId, VsandTeamRoster gr)
        {
            RosterId = gr.RosterId;
            TeamId = gr.TeamId;
            PlayerId = gr.Player.PlayerId;
            JerseyNumber = gr.JerseyNumber;

            Position = gr.Position;
            Position2 = gr.Position2;
            if (gr.PositionNavigation != null)
            {
                PositionName = gr.PositionNavigation.Name;
            }
            if (gr.Position2Navigation != null)
            {
                Position2Name = gr.Position2Navigation.Name;
            }

            var firstName = gr.Player.FirstName;
            var lastName = gr.Player.LastName;
            PlayerName = firstName +  ", " + lastName;
            FirstName = firstName;
            LastName = lastName;
            Height = gr.Player.Height;
            Class = VSAND.Common.ClassYearHelp.GraduationYearToClass(gr.Player.GraduationYear, DateTime.Now.Year);
        }
    }
}
