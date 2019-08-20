using System.Collections.Generic;
using System.Threading.Tasks;
using VSAND.Data.Entities;
using VSAND.Data.Identity;
using VSAND.Data.ViewModels;

namespace VSAND.Data.Repositories
{
    public interface ISchoolRepository : IRepository<VsandSchool>
    {
        List<VsandSchool> GetSchoolList();

        List<VsandSchool> GetSchoolList(string Keyword);

        VsandSchool GetSchool(int SchoolId);

        VsandSchool GetReviewSchool(int SchoolId);

        int GetSchoolIdByName(string Name);

        int GetSchoolIdByTeam(int TeamId);

        int GetSchoolIdByName(string sSchool, string sState);

        string GetSchoolName(int SchoolId);

        int AddSchool(string Name, string AltName, string Address1, string Address2, string City, string State, string ZipCode, string PhoneNumber, bool PrivateSchool, int CountyId, string Nickname, string Mascot, string Color1, string Color2, string Color3, string Url, string Graphic, bool GraphicApproved, ref string sMsg, ApplicationUser user);

        int SchoolQuickAdd(string Name, string State);
        Task<int> SchoolQuickAddAsync(string Name, string State);

        bool UpdateSchool(int SchoolId, string Name, string AltName, string Address1, string Address2, string City, string State, string ZipCode, string PhoneNumber, bool PrivateSchool, int CountyId, string Nickname, string Mascot, string Color1, string Color2, string Color3, string Url, string Graphic, bool GraphicApproved, ApplicationUser user);

        bool UpdateSchool(int SchoolId, string Name, string AltName, string Address1, string Address2, string City, string State, string ZipCode, string PhoneNumber, bool PrivateSchool, int CountyId, string Nickname, string Mascot, string Color1, string Color2, string Color3, string Url, string Graphic, bool GraphicApproved, bool ForceReapplyName, ApplicationUser user);

        bool DeleteSchool(int SchoolId, int UserId, string Username, ref string sMsg);

        List<ListItem<int>> FindPossibleAltSchoolNameList(string schoolname);

        Task<PagedResult<VsandSchool>> Search(string Name, string City, string State, bool coreCoverage, int pageSize, int pageNumber);

        List<VsandSchool> GetSchoolsWithoutSportsTeam(int SportId);

        List<VsandSchool> GetSchoolsWithoutSportsTeam(int ScheduleYearId, int SportId);

        List<VsandSchool> GetValidatedSchoolsWithoutSportsTeam(int ScheduleYearId, int SportId);

        List<VsandSchool> GetUnvalidated(bool bHome);

        bool ValidateSchool(int SchoolId, int UserId, string Username);

        List<VsandSchoolsWithoutAccounts> SchoolsWithoutUsers();

        List<VsandSchool> GetSubscribedByEdition(int EditionId);

        List<VsandSchool> GetUnSubscribedByEdition(int EditionId);
    }
}
