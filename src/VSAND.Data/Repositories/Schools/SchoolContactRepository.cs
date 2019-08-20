using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using VSAND.Data.Entities;
using VSAND.Data.Identity;

namespace VSAND.Data.Repositories
{
    public class SchoolContactRepository : Repository<VsandSchoolContact>, ISchoolContactRepository
    {
        private readonly VsandContext _context;
        public SchoolContactRepository(VsandContext context) : base(context)
        {
            _context = context ?? throw new ArgumentException("Context is null");
        }

        public List<VsandSchoolContact> GetSchoolContactList(int SchoolId)
        {
            List<VsandSchoolContact> oDS = null;
            if (SchoolId > 0)
            {
                IEnumerable<VsandSchoolContact> oData = null;

                oData = from sc in _context.VsandSchoolContact
                        where sc.School.SchoolId == SchoolId
                        orderby sc.LastName ascending, sc.FirstName ascending
                        select sc;
                oDS = oData.ToList();
            }
            return oDS;
        }

        public VsandSchoolContact GetSchoolContact(int SchoolContactId)
        {
            VsandSchoolContact oContact = null;

            if (SchoolContactId > 0)
            {
                oContact = (from sc in _context.VsandSchoolContact.Include(sc => sc.School)
                            where sc.SchoolContactId == SchoolContactId
                            select sc).FirstOrDefault();
            }
            return oContact;
        }

        public bool AddSchoolContact(string FirstName, string LastName, string Title, string Address1, string Address2, string City, string State, string ZipCode, string SchoolPhone,
            string HomePhone, string CellPhone, string FaxNumber, string EmailAddress, int SchoolId, ref string ErrorMessage, ApplicationUser user)
        {
            bool bAdded = false;
            VsandSchoolContact oContact = new VsandSchoolContact
            {
                FirstName = FirstName,
                LastName = LastName,
                Title = Title,
                Address1 = Address1,
                Address2 = Address2,
                City = City,
                State = State,
                ZipCode = ZipCode,
                SchoolPhone = SchoolPhone,
                HomePhone = HomePhone,
                CellPhone = CellPhone,
                FaxNumber = FaxNumber,
                EmailAddress = EmailAddress,
                SchoolId = SchoolId
            };

            _context.VsandSchoolContact.Add(oContact);

            int iChangedRows;
            try
            {
                iChangedRows = _context.SaveChanges();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.ToString();
                iChangedRows = 0;
            }

            if (iChangedRows > 0)
            {
                AuditRepository.AuditChange(_context, "vsand_SchoolContact", "SchoolContactId", oContact.SchoolContactId, "Insert", user.AppxUser.UserId, user.AppxUser.AdminId);
                bAdded = true;
            }

            return bAdded;
        }

        public bool AddSchoolContact(string FirstName, string LastName, string Title, string Address1, string Address2, string City, string State, string ZipCode, string SchoolPhone, string HomePhone, string CellPhone, string FaxNumber, string EmailAddress, int SchoolId, ApplicationUser user)
        {
            string sErr = "";
            return AddSchoolContact(FirstName, LastName, Title, Address1, Address2, City, State, ZipCode, SchoolPhone, HomePhone, CellPhone, FaxNumber, EmailAddress, SchoolId, ref sErr, user);
        }

        public bool UpdateSchoolContact(int ContactId, string FirstName, string LastName, string Title, string Address1, string Address2, string City, string State,
            string ZipCode, string SchoolPhone, string HomePhone, string CellPhone, string FaxNumber, string EmailAddress, int SchoolId, ref string ErrorMessage, ApplicationUser user)
        {
            bool bUpdated = false;

            VsandSchoolContact oContact = (from scc in _context.VsandSchoolContact
                                           where scc.SchoolContactId == ContactId
                                           select scc).First();

            if (oContact != null)
            {
                oContact.FirstName = FirstName;
                oContact.LastName = LastName;
                oContact.Title = Title;
                oContact.Address1 = Address1;
                oContact.Address2 = Address2;
                oContact.City = City;
                oContact.State = State;
                oContact.ZipCode = ZipCode;
                oContact.SchoolPhone = SchoolPhone;
                oContact.HomePhone = HomePhone;
                oContact.CellPhone = CellPhone;
                oContact.FaxNumber = FaxNumber;
                oContact.EmailAddress = EmailAddress;

                if (SchoolId > 0)
                {
                    oContact.SchoolId = SchoolId;
                }

                AuditRepository.AuditChange(_context, "vsand_SchoolContact", "SchoolContactID", ContactId, "Update", user.AppxUser.UserId, user.AppxUser.AdminId);

                int iRowsChanged = 0;
                try
                {
                    iRowsChanged = _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    ErrorMessage = ex.ToString();
                }

                if (iRowsChanged > 0)
                    bUpdated = true;
            }

            return bUpdated;
        }

        public bool UpdateSchoolContact(int ContactId, string FirstName, string LastName, string Title, string Address1, string Address2, string City, string State,
            string ZipCode, string SchoolPhone, string HomePhone, string CellPhone, string FaxNumber, string EmailAddress, int SchoolId, ApplicationUser user)
        {
            string sErr = "";
            return UpdateSchoolContact(ContactId, FirstName, LastName, Title, Address1, Address2, City, State, ZipCode, SchoolPhone, HomePhone, CellPhone, FaxNumber, EmailAddress, SchoolId, ref sErr, user);
        }

        public bool DeleteSchoolContact(int ContactId, ref string ErrorMessage, ApplicationUser user)
        {
            bool bDeleted = false;

            VsandSchoolContact oContact = (from sc in _context.VsandSchoolContact
                                           where sc.SchoolContactId == ContactId
                                           select sc).First();

            AuditRepository.AuditChange(_context, "vsand_SchoolContact", "SchoolContactID", ContactId, "Delete", user.AppxUser.UserId, user.AppxUser.AdminId);
            _context.VsandSchoolContact.Remove(oContact);

            int iChangedRows = 0;

            try
            {
                iChangedRows = _context.SaveChanges();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.ToString();
            }

            if (iChangedRows > 0)
            {
                bDeleted = true;
            }

            return bDeleted;
        }

        public bool DeleteSchoolContact(int ContactId, ApplicationUser user)
        {
            string sErr = "";
            return DeleteSchoolContact(ContactId, ref sErr, user);
        }
    }
}
