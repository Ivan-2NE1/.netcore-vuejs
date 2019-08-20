using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using VSAND.Backend.Controllers;
using VSAND.Data.Entities;
using VSAND.Data.ViewModels;
using VSAND.Services.Data.Manage.Users;
using VSAND.Services.Data.Schools;
using VSAND.Services.Data.Teams;

namespace VSAND.Backend.Areas.SiteApi
{
    [Route("siteapi/[controller]")]
    [ApiController]
    public class SchoolsController : BaseController
    {
        private ISchoolService _schools = null;
        private ITeamService _teams = null;
        public SchoolsController(ISchoolService schoolService, ITeamService teamService, IUserService userService) : base(userService)
        {
            _schools = schoolService;
            _teams = teamService;
        }

        // GET: api/schools/autocomplete
        [HttpGet("autocomplete")]
        public async Task<IEnumerable<ListItem<int>>> AutocompleteAsync([FromQuery] string k)
        {
            return await _schools.AutocompleteAsync(k);
        }

        // GET: api/schools/autocomplete
        [HttpGet("autocompleterestore")]
        public async Task<ListItem<int>> AutocompleteRestoreAsync([FromQuery] int schoolId)
        {
            return await _schools.AutocompleteRestoreAsync(schoolId);
        }

        // POST: api/Schools
        [HttpPost]
        public async Task<ApiResult<VsandSchool>> Post([FromBody] VsandSchool addSchool)
        {
            var result = await _schools.AddSchoolAsync(addSchool);
            return new ApiResult<VsandSchool>(result);
        }

        // PUT: api/Schools/5
        [HttpPut("{schoolId}")]
        public async Task<ApiResult<VsandSchool>> Put(int schoolId, [FromBody] VsandSchool chgSchool)
        {
            if (schoolId != chgSchool.SchoolId)
            {
                return null;
            }

            var result = await _schools.UpdateSchoolAsync(chgSchool);
            return new ApiResult<VsandSchool>(result);
        }

        // DELETE: api/Schools/5
        [HttpDelete("{schoolId}")]
        public async Task<ApiResult<VsandSchool>> Delete(int schoolId)
        {
            var result = await _schools.DeleteSchoolAsync(schoolId);
            return new ApiResult<VsandSchool>(result);
        }

        #region Teams
        [HttpPost("Teams/Add")]
        public async Task<int> AddTeam([FromForm] int scheduleYearId, [FromForm] int sportId, [FromForm] int schoolId)
        {
            var appxUser = ApplicationUser.AppxUser;
            return await _teams.AddTeamAsync(appxUser, scheduleYearId, sportId, schoolId);
        }
        #endregion

        #region Custom Codes
        // POST: siteapi/Schools/5/CustomCodes
        [HttpPost("{schoolId}/CustomCodes")]
        public async Task<ApiResult<VsandSchoolCustomCode>> AddCustomCode(int schoolId, VsandSchoolCustomCode viewModel)
        {
            var result = await _schools.AddCustomCodeAsync(viewModel);
            return new ApiResult<VsandSchoolCustomCode>(result);
        }

        // PUT: siteapi/Schools/5/CustomCodes/12
        [HttpPut("{schoolId}/CustomCodes/{customCodeId}")]
        public async Task<ApiResult<VsandSchoolCustomCode>> UpdateCustomCode(int schoolId, int customCodeId, VsandSchoolCustomCode viewModel)
        {
            if (schoolId != viewModel.SchoolId)
            {
                return null;
            }

            if (customCodeId != viewModel.CustomCodeId)
            {
                return null;
            }

            var result = await _schools.UpdateCustomCodeAsync(viewModel);
            return new ApiResult<VsandSchoolCustomCode>(result);
        }

        // DELETE: SiteApi/Schools/5/CustomCodes/12
        [HttpDelete("{schoolId}/CustomCodes/{customCodeId}")]
        public async Task<ApiResult<VsandSchoolCustomCode>> DeleteCustomCode(int customCodeId)
        {
            ServiceResult<VsandSchoolCustomCode> result = await _schools.DeleteCustomCodeAsync(customCodeId);
            return new ApiResult<VsandSchoolCustomCode>(result);
        }
        #endregion
    }
}
