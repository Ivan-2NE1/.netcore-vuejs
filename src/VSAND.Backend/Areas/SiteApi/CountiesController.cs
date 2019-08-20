using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using VSAND.Data.Entities;
using VSAND.Data.ViewModels;
using VSAND.Services.Data.Manage.Counties;

namespace VSAND.Backend.Areas.SiteApi
{
    [ApiController]
    [Produces("application/json")]
    [Route("SiteApi/[controller]")]
    public class CountiesController : ControllerBase
    {
        private readonly ICountyService _countyService;
        public CountiesController(ICountyService countyService)
        {
            _countyService = countyService;
        }

        // GET: api/counties/list
        [HttpGet("list", Name = "Get List of Counties")]
        public async Task<IEnumerable<ListItem<int>>> GetListAsync()
        {
            return await _countyService.GetList();
        }

        // GET: SiteApi/Counties
        [HttpGet]
        public async Task<List<VsandCounty>> Get()
        {
            return await _countyService.GetListAsync();
        }

        // POST: Site/Counties
        [HttpPost]
        public async Task<ApiResult<VsandCounty>> Post(VsandCounty vm)
        {
            var serviceResult = await _countyService.AddCountyAsync(vm);
            return new ApiResult<VsandCounty>(serviceResult);
        }

        // PUT: SiteApi/Counties/{id}
        [HttpPut("{id}")]
        public async Task<ApiResult<VsandCounty>> Put(int id, VsandCounty vm)
        {
            if (id != vm.CountyId)
            {
                return null;
            }

            var result = await _countyService.UpdateCountyAsync(vm);
            return new ApiResult<VsandCounty>(result);
        }

        // DELETE: SiteApi/Counties/{id}
        [HttpDelete("{id}")]
        public async Task<ApiResult<VsandCounty>> Delete(int id)
        {
            ServiceResult<VsandCounty> result = await _countyService.DeleteCountyAsync(id);
            return new ApiResult<VsandCounty>(result);
        }
    }
}
