using System.Collections.Generic;
using System.Threading.Tasks;
using VSAND.Data.Identity;

namespace VSAND.Data.Repositories
{
    // this shouldn't extend IRepository, because we update Identity users through the user manager, not the bare context
    public interface IUserRepository
    {
        Task<PagedResult<ApplicationUser>> SearchAsync(string username, string email, string firstName, string lastName, bool isAdmin, int schoolId, int publicationId, int pageSize, int pageNumber);
        Task<ApplicationUser> GetUserAsync(int applicationUserId);
        Task<ApplicationUser> GetUserAsync(string username);
        Task<ApplicationUser> GetUserByAppxUserId(int appxUserId);
        Task<ApplicationUser> GetUserFullAsync(int applicationUserId);
        List<string> GetRoleMembership(int appxUserId);
        Task<bool> CheckEmailInUse(string sEmail, int applicationUserId);
    }
}
