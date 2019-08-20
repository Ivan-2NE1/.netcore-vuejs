using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using VSAND.Data.Identity;

namespace VSAND.Data.Repositories
{
    // this shouldn't extend Repository, because we update Identity users through the user manager, not the bare context
    public class UserRepository : IUserRepository
    {
        private readonly VsandContext _context;
        public UserRepository(VsandContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<ApplicationUser>> SearchAsync(string username, string email, string firstName, string lastName, bool isAdmin, int schoolId, int publicationId, int pageSize, int pageNumber)
        {
            var oRet = new PagedResult<ApplicationUser>(null, 0, pageSize, pageNumber);

            pageNumber -= 1;
            if (pageNumber < 0)
            {
                pageNumber = 0;
            }

            IQueryable<ApplicationUser> oQuery = _context.Users.Include(u => u.AppxUser);

            if (!string.IsNullOrEmpty(username))
            {
                oQuery = oQuery.Where(u => u.UserName.Contains(username.Trim()));
            }

            if (!string.IsNullOrEmpty(email))
            {
                oQuery = oQuery.Where(u => u.Email.Contains(email.Trim()));
            }

            if (!string.IsNullOrEmpty(firstName))
            {
                oQuery = oQuery.Where(u => u.AppxUser.FirstName.Contains(firstName.Trim()));
            }

            if (!string.IsNullOrEmpty(lastName))
            {
                oQuery = oQuery.Where(u => u.AppxUser.LastName.Contains(lastName.Trim()));
            }

            if (isAdmin == true)
            {
                oQuery = oQuery.Where(u => u.AppxUser.IsAdmin == isAdmin);
            }

            if (schoolId != 0)
            {
                oQuery = oQuery.Where(u => u.AppxUser.SchoolId == schoolId);
            }

            if (publicationId != 0)
            {
                oQuery = oQuery.Where(u => u.AppxUser.PublicationId == publicationId);
            }

            oRet.TotalResults = await oQuery.CountAsync();
            oRet.Results = await oQuery.OrderBy(u => u.AppxUserAdminId).Skip(pageNumber * pageSize).Take(pageSize).ToListAsync();

            return oRet;
        }

        public async Task<ApplicationUser> GetUserAsync(int applicationUserId)
        {
            return await _context.Users.Include(u => u.AppxUser).FirstOrDefaultAsync(u => u.Id == applicationUserId);
        }

        public async Task<ApplicationUser> GetUserAsync(string username)
        {
            return await _context.Users.Include(u => u.AppxUser).FirstOrDefaultAsync(u => u.UserName == username);
        }
        
        public async Task<ApplicationUser> GetUserByAppxUserId(int appxUserId)
        {
            return await _context.Users.Include(u => u.AppxUser).FirstOrDefaultAsync(u => u.AppxUser.AdminId == appxUserId);
        }

        public async Task<ApplicationUser> GetUserFullAsync(int applicationUserId)
        {
            return await _context.Users.Include(u => u.AppxUser).ThenInclude(au => au.AppxUserRoles).FirstOrDefaultAsync(u => u.Id == applicationUserId);
        }

        public List<string> GetRoleMembership(int appxUserId)
        {
            var oRet = new List<string>();

            if (!_context.Database.IsSqlServer())
            {
                return null;
            }

            var oConn = _context.Database.GetDbConnection();
            if (oConn != null)
            {
                if (oConn.State == ConnectionState.Closed)
                {
                    oConn.Open();
                }
            }

            using (SqlCommand oCmd = new SqlCommand("AppxUser_GetRoleMembership", (SqlConnection)oConn))
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.Parameters.AddWithValue("@UserId", appxUserId);

                using (var oRdr = oCmd.ExecuteReader())
                {
                    while (oRdr.Read())
                    {
                        string sPermission = oRdr[0].ToString() + "." + oRdr[1].ToString();
                        oRet.Add(sPermission);
                    }
                }
            }

            return oRet;
        }

        public async Task<bool> CheckEmailInUse(string sEmail, int applicationUserId)
        {
            return await _context.Users.AnyAsync(u => u.Email == sEmail && u.Id != applicationUserId);
        }
    }
}
