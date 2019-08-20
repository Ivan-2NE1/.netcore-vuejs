using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VSAND.Data.ViewModels.FrontEndPreferences;

namespace VSAND.Frontend.Areas.SiteApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PreferencesController : ControllerBase
    {
        [HttpPost("follow")]
        public Task<Preferences> SavePreferences([FromForm] int? teamId, [FromForm] int schoolId)
        {
            //TODO: Need to implement storage and methos for preferences saving
            return null;
        }
    }
}