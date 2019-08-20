using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using VSAND.Data.Entities;
using VSAND.Data.ViewModels;
using VSAND.Services.Data.Manage.States;

namespace VSAND.Backend.Areas.SiteApi
{
    [ApiController]
    [Produces("application/json")]
    [Route("SiteApi/[controller]")]
    public class StatesController : ControllerBase
    {
        private readonly IStateService _stateService;
        public StatesController(IStateService stateService)
        {
            _stateService = stateService;
        }
        
        // GET: SiteApi/States
        [HttpGet]
        public async Task<List<VsandState>> Get()
        {
            return await _stateService.GetListAsync();
        }

        // GET: siteapi/states/list
        [HttpGet("list", Name = "Get List of States")]
        public async Task<IEnumerable<ListItem<string>>> GetActiveListAsync()
        {
            return await _stateService.List();
        }

        // POST: Site/States
        [HttpPost]
        public async Task<ApiResult<VsandState>> Post(VsandState vm)
        {
            var serviceResult = await _stateService.AddStateAsync(vm);
            return new ApiResult<VsandState>(serviceResult);
        }

        // PUT: SiteApi/States/{id}
        [HttpPut("{stateId}")]
        public async Task<ApiResult<VsandState>> Put(int stateId, VsandState vm)
        {
            if (stateId != vm.StateId)
            {
                return null;
            }

            var result = await _stateService.UpdateStateAsync(vm);
            return new ApiResult<VsandState>(result);
        }

        // DELETE: SiteApi/States/{id}
        [HttpDelete("{stateId}")]
        public async Task<ApiResult<VsandState>> Delete(int stateId)
        {
            ServiceResult<VsandState> result = await _stateService.DeleteStateAsync(stateId);
            return new ApiResult<VsandState>(result);
        }
    }
}
