using System;
using System.Collections.Generic;
using System.Text;
using VSAND.Data.Entities;

namespace VSAND.Data.ViewModels
{
    public class PlayerSummary
    {
        public int PlayerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? GraduationYear { get; set; }
        public int? SchoolId { get; set; }
        public string SchoolName { get; set; }
        public string FullName
        {
            get
            {
                return this.FirstName.Trim() + " " + this.LastName.Trim();
            }
        }

        public PlayerSummary()
        {

        }

        public PlayerSummary(VsandPlayer oPlayer)
        {
            this.PlayerId = oPlayer.PlayerId;
            this.FirstName = oPlayer.FirstName;
            this.LastName = oPlayer.LastName;
            this.GraduationYear = oPlayer.GraduationYear;
            this.SchoolId = oPlayer.SchoolId;
        }
    }
}
