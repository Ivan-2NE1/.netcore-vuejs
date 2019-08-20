using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using VSAND.Data.Entities;
using VSAND.Data.Identity;
using VSAND.Data.Repositories;
using VSAND.Data.ViewModels;
using VSAND.Data.ViewModels.Users;

namespace VSAND.Services.Data.Manage.Users
{
    public interface IUserService
    {
        Task<PagedResult<ApplicationUser>> SearchAsync(string username, string email, string firstName, string lastName, bool isAdmin, int schoolId, int publicationId, int pageSize, int pageNumber);
        Task<ServiceResult<ApplicationUser>> CreateUserAsync(AppxUser createdBy, string email, string password, string firstName, string lastName, string phone);
        Task<ServiceResult<ApplicationUser>> CreateUserAsync(AppxUser createdBy, string email, string password, string firstName, string lastName, string phone, int? schoolId);
        Task<ServiceResult<ApplicationUser>> CreateUserAsync(AppxUser createdBy, string email, string password, string firstName, string lastName, string phone, string otherPhone, int? schoolId);
        Task<ServiceResult<List<int>>> UpdateInterestedSports(int appxUserAdminId, List<int> sports);
        Task<ServiceResult<ApplicationUser>> GetUserAsync(int applicationUserId);
        Task<ServiceResult<ApplicationUser>> GetUserAsync(string username);
        Task<ServiceResult<ApplicationUser>> GetUserFullAsync(int applicationUserId);
        Task<ServiceResult<ApplicationUser>> UpdateUserAsync(AppxUser updatedBy, int applicationUserId, UserEditViewModel userInfo);
        Task<ServiceResult<ApplicationUser>> UpdateUserWithRolesAsync(AppxUser updatedBy, int applicationUserId, UserEditViewModel userInfo);
        List<string> GetRoleMembership(int appxUserId);

        Task<int> GetMasterAccountIdAsync(int schoolId);
        Task<List<SchoolMasterAccount>> GetMasterAccounts();
        Task<ServiceResult<ApplicationUser>> UpdateSchoolMasterAccountPassword(AppxUser updatedBy, int applicationUserId, string password);
        Task<ServiceResult<ApplicationUser>> CreateSchoolMasterAccountAsync(AppxUser createdBy, int schoolId, string username, string password);
        Task<ServiceResult<string>> UpdateSchoolMasterAccountsFromFile(AppxUser updatedBy, IFormFile spreadsheet);
    }
}
