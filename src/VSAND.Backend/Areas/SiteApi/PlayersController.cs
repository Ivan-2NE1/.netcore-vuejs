using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using VSAND.Data.Entities;
using VSAND.Data.ViewModels;
using VSAND.Services.Data.Players;
using VSAND.Data.ViewModels.Teams;

namespace VSAND.Backend.Areas.SiteApi
{
    [Route("SiteApi/[controller]")]
    [ApiController]
    public class PlayersController : Controller
    {
        private IPlayerService _playerService;
        public PlayersController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        // GET: SiteApi/Players
        [HttpGet("{teamId}")]
        public async Task<List<VsandPlayer>> Get(int teamid)
        {
            var oRet = await _playerService.GetListAsync(teamid);
            return oRet;
        }

        [HttpPost]
        public async Task<ApiResult<VsandPlayer>> Create(VsandPlayer viewModel)
        {
            // if a player is created by an admin, they are validated
            viewModel.Validated = true;

            var result = await _playerService.AddPlayerAsync(viewModel);
            return new ApiResult<VsandPlayer>(result);
        }

        [HttpPut("{playerId}")]
        public async Task<ApiResult<VsandPlayer>> Edit(int playerId, VsandPlayer viewModel)
        {
            if (playerId != viewModel.PlayerId)
            {
                return null;
            }

            var result = await _playerService.UpdatePlayerAsync(viewModel);
            return new ApiResult<VsandPlayer>(result);
        }

        [HttpDelete("{playerId}")]
        public async Task<ApiResult<VsandPlayer>> Delete(int playerId)
        {
            ServiceResult<VsandPlayer> result = await _playerService.DeletePlayerAsync(playerId);
            return new ApiResult<VsandPlayer>(result);
        }
    }
}
