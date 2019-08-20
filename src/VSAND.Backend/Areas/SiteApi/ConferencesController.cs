using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using VSAND.Data.Entities;
using VSAND.Data.ViewModels;
using VSAND.Services.Data.Manage.Conferences;

namespace VSAND.Backend.Areas.SiteApi
{
    [ApiController]
    [Produces("application/json")]
    [Route("SiteApi/[controller]")]
    public class ConferencesController : ControllerBase
    {
        private readonly IConferenceService _conferenceService;
        public ConferencesController(IConferenceService conferenceService)
        {
            _conferenceService = conferenceService;
        }

        // GET: SiteApi/Conferences
        [HttpGet]
        public async Task<List<VsandConference>> Get()
        {
            return await _conferenceService.GetListAsync();
        }

        // POST: Site/Conferences
        [HttpPost]
        public async Task<ApiResult<VsandConference>> Post(VsandConference vm)
        {
            var serviceResult = await _conferenceService.AddConferenceAsync(vm);
            return new ApiResult<VsandConference>(serviceResult);
        }

        // PUT: SiteApi/Conferences/{id}
        [HttpPut("{id}")]
        public async Task<ApiResult<VsandConference>> Put(int id, VsandConference vm)
        {
            if (id != vm.ConferenceId)
            {
                return null;
            }

            var result = await _conferenceService.UpdateConferenceAsync(vm);
            return new ApiResult<VsandConference>(result);
        }

        // DELETE: SiteApi/Conferences/{id}
        [HttpDelete("{id}")]
        public async Task<ApiResult<VsandConference>> Delete(int id)
        {
            ServiceResult<VsandConference> result = await _conferenceService.DeleteConferenceAsync(id);
            return new ApiResult<VsandConference>(result);
        }
    }
}
