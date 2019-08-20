using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VSAND.Backend.Controllers;
using VSAND.Data.Entities;
using VSAND.Data.ViewModels;
using VSAND.Data.ViewModels.GameReport;
using VSAND.Services.Data.GameReports;
using VSAND.Services.Data.Manage.Users;

namespace VSAND.Backend.Areas.SiteApi
{
    [ApiController]
    [Route("siteapi/[controller]")]
    public class GameController : BaseApiController
    {
        private IGameReportService _gameService = null;
        public GameController(IUserService userService, IGameReportService gameService) : base(userService)
        {
            _gameService = gameService;
        }

        // POST: SiteApi/Game/5
        [HttpPost("{gameReportId}")]
        public async Task<ApiResult<GameReport>> SaveGameOverview(int gameReportId, [FromBody] GameReport gameReport)
        {
            var oRet = new ApiResult<GameReport>();

            if (gameReport.GameReportId != gameReportId)
            {
                // mismatach, return error
                oRet.Message = "Mismatched report id values.";
                return oRet;
            }

            var result = await _gameService.UpdateGameReportOverview(ApplicationUser.AppxUser, gameReport);

            return new ApiResult<GameReport>(result);
        }
    }
}