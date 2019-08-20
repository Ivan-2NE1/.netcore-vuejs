using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VSAND.Data.ViewModels;
using VSAND.Services.Data.Manage.Publications;

namespace VSAND.Backend.Areas.SiteApi
{
    [Route("siteapi/[controller]")]
    [ApiController]
    public class PublicationsController : ControllerBase
    {
        private IPublicationService _publicationService = null;

        public PublicationsController(IPublicationService publicationService)
        {
            _publicationService = publicationService;
        }

        // GET: siteapi/ScheduleYears
        [HttpGet("list")]
        public async Task<IEnumerable<ListItem<int>>> GetListAsync()
        {
            return await _publicationService.GetList();
        }
    }
}