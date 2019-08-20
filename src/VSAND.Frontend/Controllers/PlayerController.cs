using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VSAND.Services.Data.Players;

namespace VSAND.Frontend.Controllers
{
    public class PlayerController : Controller
    {
        private readonly IPlayerService _players;

        public PlayerController(IPlayerService players)
        {
            _players = players;
        }

		[Route ("player/{playerid:int}/{sportid:int?}/{scheduleyearid:int?}")]
        public async Task<IActionResult> Player([FromRoute(Name = "playerid")] int playerId, [FromRoute(Name = "sportid")] int? sportId, [FromRoute(Name = "scheduleyearid")] int? scheduleYearId)
        {
            var player = await _players.GetFullPlayerViewCachedAsync(playerId, sportId, scheduleYearId);
            ViewData["BasePath"] = HttpContext.Items["PlayerSlug"].ToString();
            return View(player);
        }
    }
}