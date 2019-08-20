using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VSAND.Common;
using VSAND.Data.Entities;
using VSAND.Data.Identity;
using VSAND.Data.Repositories;
using VSAND.Data.ViewModels;
using VSAND.Data.ViewModels.Users;
using VSAND.Services.Files;

namespace VSAND.Services.Data.Manage.Users
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _uow;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IFileService _fileService;
        private readonly IExcelService _excelService;

        public UserService(IUnitOfWork uow, UserManager<ApplicationUser> userManager, IFileService fileService, IExcelService excelService)
        {
            _uow = uow;
            _userManager = userManager;
            _fileService = fileService;
            _excelService = excelService;
        }

        public async Task<PagedResult<ApplicationUser>> SearchAsync(string username, string email, string firstName, string lastName, bool isAdmin, int schoolId, int publicationId, int pageSize, int pageNumber)
        {
            return await _uow.Users.SearchAsync(username, email, firstName, lastName, isAdmin, schoolId, publicationId, pageSize, pageNumber);
        }

        public async Task<ServiceResult<ApplicationUser>> CreateUserAsync(AppxUser createdBy, string email, string password, string firstName, string lastName, string phone)
        {
            return await CreateUserAsync(createdBy, email, password, firstName, lastName, phone, phone, null);
        }

        public async Task<ServiceResult<ApplicationUser>> CreateUserAsync(AppxUser createdBy, string email, string password, string firstName, string lastName, string phone, int? schoolId)
        {
            return await CreateUserAsync(createdBy, email, password, firstName, lastName, phone, phone, schoolId);
        }

        public async Task<ServiceResult<ApplicationUser>> CreateUserAsync(AppxUser createdBy, string email, string password, string firstName, string lastName, string phone, string otherPhone, int? schoolId)
        {
            var oRet = new ServiceResult<ApplicationUser>();
            var errors = new List<string>();

            if (string.IsNullOrEmpty(email))
            {
                errors.Add("Email is required.");
            }

            if (!string.IsNullOrEmpty(phone))
            {
                phone = PhoneHelp.CleanMobilePhoneNumber(phone);
                if (phone == null)
                {
                    errors.Add("Mobile phone number is invalid");
                }
            }

            if (string.IsNullOrEmpty(password))
            {
                errors.Add("Password is required.");
            }

            if (string.IsNullOrEmpty(firstName))
            {
                errors.Add("First name is required.");
            }

            if (string.IsNullOrEmpty(lastName))
            {
                errors.Add("Last name is required.");
            }

            if (errors.Count > 0)
            {
                oRet.Success = false;
                oRet.Message = string.Join("\n", errors);
                return oRet;
            }

            _uow.BeginTransaction();

            var appxUser = new AppxUser
            {
                UserId = email,
                EmailAddress = email,
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = otherPhone,
                RegistrationDate = DateTime.Now,
                ConfirmationDate = DateTime.Now,
                Token = "",
                SchoolId = schoolId
            };

            await _uow.AppxUsers.Insert(appxUser);

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                AppxUserAdminId = appxUser.AdminId,
                PhoneNumber = phone
            };

            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded == false)
            {
                oRet.Success = false;
                oRet.Message = string.Join("\n", result.Errors.Select(e => e.Description));
                return oRet;
            }

            var bRet = await _uow.CommitTransaction();
            if (!bRet)
            {
                oRet.Success = false;
                oRet.Message = "An error occurred while trying to create the user";
                return oRet;
            }

            _uow.Audit.AuditChange("appxUser", "AdminId", appxUser.AdminId, "Insert", createdBy.UserId, createdBy.AdminId);

            oRet.Success = true;
            oRet.Id = user.Id;
            oRet.obj = user;

            return oRet;
        }

        public async Task<ServiceResult<List<int>>> UpdateInterestedSports(int appxUserAdminId, List<int> sports)
        {
            var oRet = new ServiceResult<List<int>>();

            var userSports = sports.Select(sportId => new VsandUserSport
            {
                AdminId = appxUserAdminId,
                SportId = sportId
            }).ToList();

            var dbUserSports = await _uow.UserSports.List(us => us.AdminId == appxUserAdminId);

            foreach (var dbUserSport in dbUserSports)
            {
                if (!sports.Contains(dbUserSport.SportId))
                {
                    _uow.UserSports.Delete(dbUserSport);
                }
            }

            foreach (var userSport in userSports)
            {
                if (!dbUserSports.Any(us => us.SportId == userSport.SportId))
                {
                    await _uow.UserSports.Insert(userSport);
                }
            }

            var bSaved = await _uow.Save();
            if (bSaved == false)
            {
                oRet.Success = false;
                oRet.Message = oRet.Message = "There was an error saving the interested sports.";
                return oRet;
            }

            oRet.Success = true;

            return oRet;
        }

        public async Task<ServiceResult<ApplicationUser>> GetUserAsync(int applicationUserId)
        {
            var oRet = new ServiceResult<ApplicationUser>();

            var user = await _uow.Users.GetUserAsync(applicationUserId);
            if (user == null)
            {
                oRet.Success = false;
                oRet.Message = "No user found with the given ID";
                return oRet;
            }

            oRet.Success = true;
            oRet.obj = user;

            return oRet;
        }

        public async Task<ServiceResult<ApplicationUser>> GetUserAsync(string username)
        {
            var oRet = new ServiceResult<ApplicationUser>();

            var user = await _uow.Users.GetUserAsync(username);
            if (user == null)
            {
                oRet.Success = false;
                oRet.Message = "No user found with the given username";
                return oRet;
            }

            oRet.Success = true;
            oRet.obj = user;

            return oRet;
        }

        public async Task<ServiceResult<ApplicationUser>> GetUserFullAsync(int applicationUserId)
        {
            var oRet = new ServiceResult<ApplicationUser>();

            var user = await _uow.Users.GetUserFullAsync(applicationUserId);
            if (user == null)
            {
                oRet.Success = false;
                oRet.Message = "No user found with the given ID";
                return oRet;
            }

            oRet.Success = true;
            oRet.obj = user;

            return oRet;
        }

        public async Task<ServiceResult<ApplicationUser>> UpdateUserAsync(AppxUser updatedBy, int applicationUserId, UserEditViewModel userInfo)
        {
            var oRet = new ServiceResult<ApplicationUser>();
            var errors = new List<string>();

            // do validation / cleanup here for phone numbers, emails, etc.
            if (!string.IsNullOrEmpty(userInfo.MobilePhone))
            {
                userInfo.MobilePhone = PhoneHelp.CleanMobilePhoneNumber(userInfo.MobilePhone);
                if (userInfo.MobilePhone == null)
                {
                    errors.Add("Mobile phone is invalid.");
                }
            }

            userInfo.Email = EmailHelp.CleanEmailAddress(userInfo.Email);
            if (userInfo.Email == null)
            {
                errors.Add("Email address is invalid.");
            }
            else
            {
                if (await _uow.Users.CheckEmailInUse(userInfo.Email, applicationUserId))
                {
                    errors.Add("Email address is already in use.");
                }
            }

            if (string.IsNullOrEmpty(userInfo.FirstName))
            {
                errors.Add("First name is required.");
            }

            if (string.IsNullOrEmpty(userInfo.LastName))
            {
                errors.Add("Last name is required.");
            }

            if (errors.Count > 0)
            {
                oRet.Success = false;
                oRet.Message = string.Join("\n", errors);
                return oRet;
            }

            // update the application user
            var applicationUser = await _uow.Users.GetUserFullAsync(applicationUserId);

            if (userInfo.Email != applicationUser.Email)
            {
                if (applicationUser.Email == applicationUser.UserName)
                {
                    // this user logs in with their email
                    applicationUser.Email = userInfo.Email;
                    applicationUser.UserName = userInfo.Email;

                    // TODO: add message that says login will take place with new email
                    // needs to bubble up to the controller level and either log the user out or update their claims
                }
                else
                {
                    // this user has a username to log in, only update their email
                    applicationUser.Email = userInfo.Email;
                }
            }

            applicationUser.PhoneNumber = userInfo.MobilePhone;

            if (!string.IsNullOrEmpty(userInfo.Password))
            {
                applicationUser.PasswordHash = _userManager.PasswordHasher.HashPassword(applicationUser, userInfo.Password);
            }

            _uow.BeginTransaction();

            var result = await _userManager.UpdateAsync(applicationUser);
            if (result.Succeeded == false)
            {
                oRet.Success = false;
                oRet.Message = string.Join("\n", result.Errors.Select(e => e.Description));
                return oRet;
            }

            // update the appx user
            var appxUser = applicationUser.AppxUser;
            appxUser.FirstName = userInfo.FirstName;
            appxUser.LastName = userInfo.LastName;
            appxUser.PhoneNumber = userInfo.OtherPhone;

            if (userInfo.Email != appxUser.EmailAddress)
            {
                if (applicationUser.Email == applicationUser.UserName)
                {
                    // this user logs in with their email
                    appxUser.EmailAddress = userInfo.Email;
                    appxUser.UserId = userInfo.Email;
                }
                else
                {
                    // this user has a username to log in, only update their email
                    appxUser.EmailAddress = userInfo.Email;
                }
            }

            _uow.AppxUsers.Update(appxUser);

            var bSaved = await _uow.Save();
            if (bSaved == false)
            {
                oRet.Success = false;
                oRet.Message = "An error occurred while saving the user.";
                return oRet;
            }

            var bCommitted = await _uow.CommitTransaction();
            if (bCommitted == false)
            {
                oRet.Success = false;
                oRet.Message = "An error occurred while committing the transaction.";
                return oRet;
            }

            _uow.Audit.AuditChange("appxUser", "AdminId", appxUser.AdminId, "Update", updatedBy.UserId, updatedBy.AdminId);

            oRet.Success = true;
            oRet.obj = applicationUser;

            return oRet;
        }

        public async Task<ServiceResult<ApplicationUser>> UpdateUserWithRolesAsync(AppxUser updatedBy, int applicationUserId, UserEditViewModel userInfo)
        {
            var oRet = new ServiceResult<ApplicationUser>();
            var errors = new List<string>();

            // do validation / cleanup here for phone numbers, emails, etc.
            if (!string.IsNullOrEmpty(userInfo.MobilePhone))
            {
                userInfo.MobilePhone = PhoneHelp.CleanMobilePhoneNumber(userInfo.MobilePhone);
                if (userInfo.MobilePhone == null)
                {
                    errors.Add("Mobile phone is invalid.");
                }
            }

            userInfo.Email = EmailHelp.CleanEmailAddress(userInfo.Email);
            if (userInfo.Email == null)
            {
                errors.Add("Email address is invalid.");
            }
            else
            {
                if (await _uow.Users.CheckEmailInUse(userInfo.Email, applicationUserId))
                {
                    errors.Add("Email address is already in use.");
                }
            }

            if (string.IsNullOrEmpty(userInfo.FirstName))
            {
                errors.Add("First name is required.");
            }

            if (string.IsNullOrEmpty(userInfo.LastName))
            {
                errors.Add("Last name is required.");
            }

            if (userInfo.IsAdmin == false && (userInfo.SchoolId == null || userInfo.SchoolId == 0))
            {
                errors.Add("School is required.");
            }

            if (errors.Count > 0)
            {
                oRet.Success = false;
                oRet.Message = string.Join("\n", errors);
                return oRet;
            }

            // update the application user
            var applicationUser = await _uow.Users.GetUserFullAsync(applicationUserId);
            applicationUser.UserName = userInfo.Username;
            applicationUser.Email = userInfo.Email;
            applicationUser.PhoneNumber = userInfo.MobilePhone;

            if (!string.IsNullOrEmpty(userInfo.Password))
            {
                applicationUser.PasswordHash = _userManager.PasswordHasher.HashPassword(applicationUser, userInfo.Password);
            }

            _uow.BeginTransaction();

            var result = await _userManager.UpdateAsync(applicationUser);
            if (result.Succeeded == false)
            {
                oRet.Success = false;
                oRet.Message = string.Join("\n", result.Errors.Select(e => e.Description));
                return oRet;
            }

            // update the appx user
            var appxUser = applicationUser.AppxUser;
            appxUser.UserId = userInfo.Username;
            appxUser.EmailAddress = userInfo.Email;
            appxUser.FirstName = userInfo.FirstName;
            appxUser.LastName = userInfo.LastName;
            appxUser.PhoneNumber = userInfo.OtherPhone;
            appxUser.IsAdmin = userInfo.IsAdmin;

            if (appxUser.IsAdmin)
            {
                // if the user is a part of staff, they have no school affiliation but they should have a publication affiliation
                appxUser.SchoolId = null;
                appxUser.PublicationId = userInfo.PublicationId;

                var dbRoles = appxUser.AppxUserRoles;

                var roleMembers = (await _uow.AppxUserRoles.List(r => userInfo.UserRoles.Contains(r.RoleId)))
                    .Select(r => new AppxUserRoleMember
                    {
                        AdminId = appxUser.AdminId,
                        RoleId = r.RoleId
                    }).ToList();

                foreach (var role in dbRoles)
                {
                    if (!roleMembers.Any(r => r.RoleId == role.RoleId))
                    {
                        _uow.AppxUserRoleMembers.Delete(role);
                    }
                }

                foreach (var role in roleMembers)
                {
                    if (!dbRoles.Any(r => r.RoleId == role.RoleId))
                    {
                        await _uow.AppxUserRoleMembers.Insert(role);
                    }
                }

                bool bRolesSaved = await _uow.Save();
                if (bRolesSaved == false)
                {
                    oRet.Success = false;
                    oRet.Message = "An error occurred while updaing user roles.";
                    return oRet;
                }
            }
            else
            {
                // if the user is not on staff, they must have a school affiliation
                appxUser.PublicationId = null;
                appxUser.SchoolId = userInfo.SchoolId;

                // remove this user from all roles
                var memberRoles = await _uow.AppxUserRoleMembers.List(r => r.AdminId == appxUser.AdminId);
                _uow.AppxUserRoleMembers.DeleteRange(memberRoles);

                var bRemoved = await _uow.Save();
                if (bRemoved == false)
                {
                    oRet.Success = false;
                    oRet.Message = "An error occurred while updating user roles.";
                    return oRet;
                }

                appxUser.AppxUserRoles = new List<AppxUserRoleMember>();

                // add the user to the SchoolMasterAccount role is applicable
                var schoolMasterAccountRole = await _uow.AppxUserRoles.Single(r => r.RoleCat == "UserFunction" && r.RoleName == "SchoolMasterAccount");
                if (userInfo.UserRoles != null && userInfo.UserRoles.Contains(schoolMasterAccountRole.RoleId))
                {
                    var schoolMasterAccountRoleMember = new AppxUserRoleMember
                    {
                        AdminId = appxUser.AdminId,
                        RoleId = schoolMasterAccountRole.RoleId
                    };

                    await _uow.AppxUserRoleMembers.Insert(schoolMasterAccountRoleMember);
                    var bRolesSaved = await _uow.Save();
                    if (bRolesSaved == false)
                    {
                        oRet.Success = false;
                        oRet.Message = "An error occurred while updating user roles.";
                        return oRet;
                    }

                    appxUser.AppxUserRoles.Add(schoolMasterAccountRoleMember);
                }
            }

            _uow.AppxUsers.Update(appxUser);

            var bSaved = await _uow.Save();
            if (bSaved == false)
            {
                oRet.Success = false;
                oRet.Message = "An error occurred while saving the user.";
                return oRet;
            }

            var bCommitted = await _uow.CommitTransaction();
            if (bCommitted == false)
            {
                oRet.Success = false;
                oRet.Message = "An error occurred while committing the transaction.";
                return oRet;
            }

            _uow.Audit.AuditChange("appxUser", "AdminId", appxUser.AdminId, "Update", updatedBy.UserId, updatedBy.AdminId);

            oRet.Success = true;
            oRet.obj = applicationUser;

            return oRet;
        }

        public List<string> GetRoleMembership(int appxUserId)
        {
            return _uow.Users.GetRoleMembership(appxUserId);
        }

        #region School Master Accounts
        public async Task<int> GetMasterAccountIdAsync(int schoolId)
        {
            var masterAccount = await _uow.AppxUsers.Single(u => u.SchoolId == schoolId && u.AppxUserRoles.Any(r => r.Role.RoleCat == "UserFunction" && r.Role.RoleName == "SchoolMasterAccount"));
            if (masterAccount != null)
            {
                var applicationUser = await _uow.Users.GetUserByAppxUserId(masterAccount.AdminId);
                if (applicationUser != null)
                {
                    return applicationUser.Id;
                }
            }

            return 0;
        }

        public async Task<List<SchoolMasterAccount>> GetMasterAccounts()
        {
            var oRet = new List<SchoolMasterAccount>();

            var coreCoverageSchools = await _uow.Schools.List(s => s.CoreCoverage);
            var coreCoverageIds = coreCoverageSchools.Select(s => s.SchoolId);

            var masterAccounts = await _uow.AppxUsers.List(u => u.SchoolId.HasValue && coreCoverageIds.Contains(u.SchoolId.Value) && u.AppxUserRoles.Any(r => r.Role.RoleCat == "UserFunction" && r.Role.RoleName == "SchoolMasterAccount"));

            // use the first master account for each school
            masterAccounts = masterAccounts.GroupBy(ma => ma.SchoolId).Select(g => g.FirstOrDefault()).ToList();

            if (masterAccounts.Count > 0)
            {
                // we are updating school master accounts
                oRet = masterAccounts.Select(ma => new SchoolMasterAccount
                {
                    SchoolId = ma.SchoolId.Value,
                    SchoolName = coreCoverageSchools.FirstOrDefault(s => s.SchoolId == ma.SchoolId.Value)?.Name,
                    Username = ma.UserId
                }).ToList();
            }
            else
            {
                // we are creating new school master accounts
                var userNameRgx = new Regex("[^A-Z0-9a-z]");

                oRet = coreCoverageSchools.Select(s => new SchoolMasterAccount
                {
                    SchoolId = s.SchoolId,
                    SchoolName = s.Name,
                    Username = userNameRgx.Replace(s.Name, "")
                }).ToList();
            }

            return oRet;
        }

        public async Task<ServiceResult<ApplicationUser>> UpdateSchoolMasterAccountPassword(AppxUser updatedBy, int applicationUserId, string password)
        {
            var oRet = new ServiceResult<ApplicationUser>();
            var errors = new List<string>();

            if (string.IsNullOrEmpty(password))
            {
                errors.Add("Password is required.");
            }

            if (errors.Count > 0)
            {
                oRet.Success = false;
                oRet.Message = string.Join("\n", errors);
                return oRet;
            }

            var applicationUser = await _userManager.FindByIdAsync(applicationUserId.ToString());
            applicationUser.PasswordHash = _userManager.PasswordHasher.HashPassword(applicationUser, password);

            var result = await _userManager.UpdateAsync(applicationUser);

            if (!result.Succeeded)
            {
                oRet.Success = false;
                oRet.Message = result.Errors.FirstOrDefault()?.Description;
                return oRet;
            }

            _uow.Audit.AuditChange("appxUser", "AdminId", applicationUser.AppxUserAdminId, "Update", updatedBy.UserId, updatedBy.AdminId);

            oRet.Success = true;
            oRet.Id = applicationUser.Id;
            oRet.obj = applicationUser;

            return oRet;
        }

        public async Task<ServiceResult<ApplicationUser>> CreateSchoolMasterAccountAsync(AppxUser createdBy, int schoolId, string username, string password)
        {
            var oRet = new ServiceResult<ApplicationUser>();
            var errors = new List<string>();

            if (string.IsNullOrEmpty(username))
            {
                errors.Add("Username is required.");
            }

            if (string.IsNullOrEmpty(password))
            {
                errors.Add("Password is required.");
            }

            if ((await _uow.Users.GetUserAsync(username)) != null)
            {
                errors.Add("This username is already in use.");
            }

            if (errors.Count > 0)
            {
                oRet.Success = false;
                oRet.Message = string.Join("\n", errors);
                return oRet;
            }

            var school = await _uow.Schools.Single(s => s.SchoolId == schoolId);

            _uow.BeginTransaction();

            var appxUser = new AppxUser
            {
                UserId = username,
                EmailAddress = null,
                FirstName = school.Name,
                LastName = "Master Account",
                PhoneNumber = null,
                RegistrationDate = DateTime.Now,
                ConfirmationDate = DateTime.Now,
                Token = "",
                SchoolId = schoolId
            };

            await _uow.AppxUsers.Insert(appxUser);

            var user = new ApplicationUser
            {
                UserName = username,
                Email = "",
                AppxUserAdminId = appxUser.AdminId,
                PhoneNumber = null
            };

            var identityResult = await _userManager.CreateAsync(user, password);
            if (identityResult.Succeeded == false)
            {
                oRet.Success = false;
                oRet.Message = string.Join("\n", identityResult.Errors.Select(e => e.Description));
                return oRet;
            }

            // add this account to the School Master Account role
            var schoolMasterAccountRole = await _uow.AppxUserRoles.Single(r => r.RoleCat == "UserFunction" && r.RoleName == "SchoolMasterAccount");
            var schoolMasterAccountRoleMember = new AppxUserRoleMember
            {
                AdminId = appxUser.AdminId,
                RoleId = schoolMasterAccountRole.RoleId
            };

            await _uow.AppxUserRoleMembers.Insert(schoolMasterAccountRoleMember);
            var bRolesSaved = await _uow.Save();
            if (bRolesSaved == false)
            {
                oRet.Success = false;
                oRet.Message = "An error occurred while updating user roles.";
                return oRet;
            }

            var bRet = await _uow.CommitTransaction();
            if (!bRet)
            {
                oRet.Success = false;
                oRet.Message = "An error occurred while trying to create the user";
                return oRet;
            }

            _uow.Audit.AuditChange("appxUser", "AdminId", appxUser.AdminId, "Insert", createdBy.UserId, createdBy.AdminId);

            oRet.Success = true;
            oRet.Id = user.Id;
            oRet.obj = user;

            return oRet;
        }

        public async Task<ServiceResult<string>> UpdateSchoolMasterAccountsFromFile(AppxUser updatedBy, IFormFile spreadsheet)
        {
            var oResult = new ServiceResult<string>();
            var errors = new List<string>();

            if (spreadsheet.Length <= 0)
            {
                oResult.Success = false;
                oResult.Message = "The uploaded file is empty.";
                return oResult;
            }

            var allowedExtensions = new List<string> { ".xlsx" };
            if (!_fileService.IsAllowedType(allowedExtensions, spreadsheet.FileName))
            {
                oResult.Success = false;
                oResult.Message = $"The uploaded file must be one of the following: {string.Join(", ", allowedExtensions)}";
                return oResult;
            }

            var fileExt = _fileService.GetExtension(spreadsheet.FileName);
            var storagePath = "SchoolMasterAccountImport";
            var baseDir = new DirectoryInfo(_fileService.GetAppDataStoragePath(storagePath));

            // full path to file in temp location
            string fileName = $"SchoolMasterAccounts_{DateTime.Now.ToString("yyyy-dd-M_HH-mm-ss")}{fileExt}";
            var saveResult = await _fileService.SaveUploadedFile(storagePath, fileName, true, spreadsheet);
            if (!saveResult.Success)
            {
                oResult.Success = false;
                oResult.Message = saveResult.Message;
                return oResult;
            }

            string fullPath = _fileService.GetFullName("SchoolMasterAccountImport", fileName);

            var schoolMasterAccounts = _excelService.GetDataToList(fullPath, SchoolMasterAccount.ImportRow);
            foreach (var masterAccount in schoolMasterAccounts)
            {
                // don't make any changes if the password is empty
                if (masterAccount.Invalid)
                {
                    errors.Add($"{masterAccount.SchoolName} has invalid data in the row.");
                    continue;
                }

                var applicationUserId = await GetMasterAccountIdAsync(masterAccount.SchoolId);
                if (applicationUserId == 0)
                {
                    // we need to create the master account
                    var userCreateResult = await CreateSchoolMasterAccountAsync(updatedBy, masterAccount.SchoolId, masterAccount.Username, masterAccount.Password);
                    if (!userCreateResult.Success)
                    {
                        if (userCreateResult.Message.ToLower().Contains("password"))
                        {
                            errors.Add($"Password for {masterAccount.SchoolName} does not meet the requirements.");
                        }
                        else
                        {
                            errors.Add(userCreateResult.Message);
                        }
                    }
                }
                else
                {
                    // we need to update the master account
                    var userUpdateResult = await UpdateSchoolMasterAccountPassword(updatedBy, applicationUserId, masterAccount.Password);
                    if (!userUpdateResult.Success)
                    {
                        errors.Add(userUpdateResult.Message);
                    }
                }
            }

            oResult.Success = true;
            oResult.Message = string.Join("\n", errors);

            return oResult;
        }
        #endregion
    }
}
