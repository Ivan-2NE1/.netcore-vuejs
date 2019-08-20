using System.Collections.Generic;
using VSAND.Data.Entities;
using VSAND.Data.Identity;

namespace VSAND.Data.Repositories
{
    public interface ISchoolContactRepository : IRepository<VsandSchoolContact>
    {
        List<VsandSchoolContact> GetSchoolContactList(int SchoolId);

        VsandSchoolContact GetSchoolContact(int SchoolContactId);

        bool AddSchoolContact(string FirstName, string LastName, string Title, string Address1, string Address2, string City, string State, string ZipCode, string SchoolPhone, string HomePhone, string CellPhone, string FaxNumber, string EmailAddress, int SchoolId, ref string ErrorMessage, ApplicationUser user);

        bool AddSchoolContact(string FirstName, string LastName, string Title, string Address1, string Address2, string City, string State, string ZipCode, string SchoolPhone, string HomePhone, string CellPhone, string FaxNumber, string EmailAddress, int SchoolId, ApplicationUser user);

        bool UpdateSchoolContact(int ContactId, string FirstName, string LastName, string Title, string Address1, string Address2, string City, string State, string ZipCode, string SchoolPhone, string HomePhone, string CellPhone, string FaxNumber, string EmailAddress, int SchoolId, ref string ErrorMessage, ApplicationUser user);

        bool UpdateSchoolContact(int ContactId, string FirstName, string LastName, string Title, string Address1, string Address2, string City, string State, string ZipCode, string SchoolPhone, string HomePhone, string CellPhone, string FaxNumber, string EmailAddress, int SchoolId, ApplicationUser user);

        bool DeleteSchoolContact(int ContactId, ref string ErrorMessage, ApplicationUser user);

        bool DeleteSchoolContact(int ContactId, ApplicationUser user);
    }
}
