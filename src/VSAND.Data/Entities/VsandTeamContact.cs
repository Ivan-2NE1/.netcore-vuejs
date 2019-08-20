using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandTeamContact
    {
        public int TeamContactId { get; set; }
        public int TeamId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JobTitle { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string SchoolPhone { get; set; }
        public string HomePhone { get; set; }
        public string CellPhone { get; set; }
        public string FaxNumber { get; set; }
        public string EmailAddress { get; set; }

        public VsandTeam Team { get; set; }
    }
}
