using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VSAND.Data.Entities;
using VSAND.Data.Repositories;
using VSAND.Data.ViewModels;
using VSAND.Services.Cache;

namespace VSAND.Services.Data.Schools
{
    public class SchoolService : ISchoolService
    {
        private IUnitOfWork _uow;
        private ICache _cache;

        public SchoolService(IUnitOfWork uow, ICache cache)
        {
            _uow = uow ?? throw new ArgumentException("Unit of Work is null");
            _cache = cache ?? throw new ArgumentException("Cache is null");
        }

        private async Task<List<ListItem<string>>> GetFrontEndDisplayListAsync()
        {
            var schools = await _uow.Schools.List(s => s.FrontEndDisplay, s => s.OrderBy(vs => vs.Name));
            var schoolSlugs = await _uow.EntitySlugs.List(es => es.EntityType == "School");

            var schoolList = new List<ListItem<string>>();
            foreach(var school in schools)
            {
                var schoolSlug = schoolSlugs.FirstOrDefault(es => es.EntityId == school.SchoolId);
                if (schoolSlug != null)
                {
                    schoolList.Add(new ListItem<string>(schoolSlug.Slug, school.Name));
                }
            }

            return schoolList;
        }

        public async Task<List<ListItem<string>>> GetFrontEndDisplayListCachedAsync()
        {
            var cacheKey = Cache.Keys.FrontEndDisplaySchoolList();
            var ret = await _cache.GetAsync<List<ListItem<string>>>(cacheKey);
            if (ret != null && ret.Any())
            {
                return ret;
            }

            ret = await GetFrontEndDisplayListAsync();
            if (ret != null && ret.Any())
            {
                await _cache.SetAsync(cacheKey, ret);
            }

            return ret;
        }
        public async Task<List<ListItem<string>>> GetInCoverageSchoolList()
        {
            //TODO: This MUST be cached to improve performance
            var oSchools = await _uow.Schools.List(s => s.CoreCoverage, s => s.OrderBy(vs => vs.Name));
            var oRet = (from s in oSchools select new ListItem<string>(s.Slug, s.Name)).ToList();
            return oRet;
        }

        public async Task<IEnumerable<ListItem<int>>> AutocompleteAsync(string keyword)
        {
            var oRet = new List<ListItem<int>>();

            if (string.IsNullOrEmpty(keyword))
            {
                return oRet;
            }

            // TODO: This could be cached to improve performance (store the search keyword + the results);
            var oSchools = await _uow.Schools.PagedList(filter: s => s.Name.StartsWith(keyword) || s.AltName.StartsWith(keyword),
                orderBy: s => s.OrderBy(o => o.Name), null, 1, 10);

            foreach (VsandSchool oSchool in oSchools.Results)
            {
                oRet.Add(new ListItem<int>() { id = oSchool.SchoolId, name = oSchool.ListDisplayName });
            }

            return oRet;
        }

        public async Task<ListItem<int>> AutocompleteRestoreAsync(int schoolId)
        {
            var oRet = new ListItem<int>
            {
                id = schoolId
            };

            var oSchool = await _uow.Schools.GetById(schoolId);
            if (oSchool != null)
            {
                oRet.name = oSchool.Name;
            }
            else
            {
                oRet.name = "Could not load school " + schoolId;
            }

            return oRet;
        }

        public async Task<VsandSchool> GetSchoolAsync(int schoolId)
        {
            return await _uow.Schools.Single(s => s.SchoolId == schoolId);
        }

        public async Task<VsandSchool> GetFullSchoolAsync(int schoolId)
        {
            return await _uow.Schools.Single(s => s.SchoolId == schoolId, null, new List<string> {
                "Teams", "Editions", "Contacts", "CustomCodes"
            });
        }

        public async Task<PagedResult<SchoolSummary>> SearchAsync(string name, string city, string state, bool coreCoverage, int pageSize, int pageNumber)
        {
            var oRet = new PagedResult<SchoolSummary>(new List<SchoolSummary>(), 0, pageSize, pageNumber);
            var oResult = await _uow.Schools.Search(name, city, state, coreCoverage, pageSize, pageNumber);

            oRet.TotalResults = oResult.TotalResults;

            foreach (VsandSchool vschool in oResult.Results)
            {
                oRet.Results.Add(new SchoolSummary(vschool));
            }

            return oRet;
        }

        public async Task<ServiceResult<VsandSchool>> AddSchoolAsync(VsandSchool addSchool)
        {
            var oRet = new ServiceResult<VsandSchool>();

            addSchool.Name = addSchool.Name.Trim();

            // TODO: check for duplicate schools here
            if ((await _uow.Schools.List(s => s.Name == addSchool.Name)).Count() != 0)
            {
                oRet.obj = addSchool;
                oRet.Success = false;
                oRet.Message = "Another school was found with this name.";

                return oRet;
            }

            await _uow.Schools.Insert(addSchool);
            var bRet = await _uow.Save();

            if (bRet)
            {
                oRet.obj = addSchool;
                oRet.Success = true;
                oRet.Id = addSchool.SchoolId;

                // TODO: This is the layer that the cache engine should be invoked for Schools (frequently used)
            }

            return oRet;
        }

        public async Task<ServiceResult<VsandSchool>> UpdateSchoolAsync(VsandSchool chgSchool)
        {
            var oRet = new ServiceResult<VsandSchool>();

            chgSchool.Name = chgSchool.Name.Trim();

            if ((await _uow.Schools.List(s => s.Name == chgSchool.Name && s.SchoolId != chgSchool.SchoolId)).Count() != 0)
            {
                oRet.obj = chgSchool;
                oRet.Success = false;
                oRet.Message = "Another school was found with this name.";

                return oRet;
            }

            _uow.Schools.Update(chgSchool);

            var bRet = await _uow.Save();

            if (bRet)
            {
                oRet.obj = chgSchool;
                oRet.Success = true;
                oRet.Id = chgSchool.SchoolId;

                // TODO: This is the layer that the cache engine should be invoked for Schools (frequently used)
            }

            return oRet;
        }

        public async Task<ServiceResult<VsandSchool>> DeleteSchoolAsync(int schoolId)
        {
            var oRet = new ServiceResult<VsandSchool>();

            VsandSchool remSchool = await _uow.Schools.Single(s => s.SchoolId == schoolId);

            var schoolSummary = await GetSchoolSummaryAsync(schoolId);
            if (schoolSummary.PlayerCount != 0 || schoolSummary.TeamCount != 0)
            {
                oRet.obj = remSchool;
                oRet.Success = false;
                oRet.Message = $"This school cannot be deleted because it has {schoolSummary.PlayerCount} players and {schoolSummary.TeamCount} teams.";

                return oRet;
            }

            await _uow.Schools.Delete(remSchool.SchoolId);

            var bRet = await _uow.Save();

            if (bRet)
            {
                oRet.obj = remSchool;
                oRet.Success = true;
                oRet.Id = remSchool.SchoolId;
            }

            return oRet;
        }

        private async Task<SchoolSummary> GetSchoolSummaryAsync(int schoolId)
        {
            var school = await _uow.Schools.GetById(schoolId);
            var oRet = new SchoolSummary(school)
            {
                PlayerCount = await _uow.Players.Count(t => t.SchoolId.HasValue && t.SchoolId.Value == schoolId),
                TeamCount = await _uow.Teams.Count(t => t.SchoolId.HasValue && t.SchoolId.Value == schoolId)
            };

            return oRet;
        }

        #region Custom Codes
        public async Task<ServiceResult<VsandSchoolCustomCode>> AddCustomCodeAsync(VsandSchoolCustomCode addCustomCode)
        {
            var oRet = new ServiceResult<VsandSchoolCustomCode>();

            await _uow.SchoolCustomCodes.Insert(addCustomCode);

            var bRet = await _uow.Save();

            if (bRet)
            {
                oRet.obj = addCustomCode;
                oRet.Success = true;
                oRet.Id = addCustomCode.CustomCodeId;
            }

            return oRet;
        }

        public async Task<ServiceResult<VsandSchoolCustomCode>> UpdateCustomCodeAsync(VsandSchoolCustomCode chgCustomCode)
        {
            var oRet = new ServiceResult<VsandSchoolCustomCode>();

            _uow.SchoolCustomCodes.Update(chgCustomCode);

            var bRet = await _uow.Save();

            if (bRet)
            {
                oRet.obj = chgCustomCode;
                oRet.Success = true;
                oRet.Id = chgCustomCode.CustomCodeId;
            }

            return oRet;
        }

        public async Task<ServiceResult<VsandSchoolCustomCode>> DeleteCustomCodeAsync(int schoolId)
        {
            var oRet = new ServiceResult<VsandSchoolCustomCode>();

            VsandSchoolCustomCode remCustomCode = await _uow.SchoolCustomCodes.GetById(schoolId);
            await _uow.SchoolCustomCodes.Delete(remCustomCode.CustomCodeId);

            var bRet = await _uow.Save();

            if (bRet)
            {
                oRet.obj = remCustomCode;
                oRet.Success = true;
                oRet.Id = remCustomCode.CustomCodeId;
            }

            return oRet;
        }
        #endregion
    }
}
