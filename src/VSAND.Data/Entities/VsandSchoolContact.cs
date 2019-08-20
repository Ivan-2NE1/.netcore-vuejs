using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandSchoolContact
    {
        public int SchoolContactId { get; set; }
        public int SchoolId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
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

        public VsandSchool School { get; set; }
    }
}
