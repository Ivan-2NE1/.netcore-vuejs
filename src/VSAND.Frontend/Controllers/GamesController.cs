using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VSAND.Services.Data.GameReports;

namespace VSAND.Frontend.Controllers
{
    public class GamesController : Controller
    {
        private readonly IGameReportService _gameService;

        public GamesController(IGameReportService gameService)
        {
            _gameService = gameService;
        }

        [Route("game/{gameid:int}")]
        public async Task<IActionResult> Game([FromRoute(Name = "gameid")] int gameId)
        {
            var game = await _gameService.GetFullGameReportCachedAsync(gameId);
            if (game == null)
            {
                //TODO: Throw a 404 error here
                return View("GameError");
            }

            var preview = !game.Final;
            var teamCount = game.Teams.Count;
            if (teamCount == 1)
            {
                // this is a game with only one participant
            }
            else if (teamCount == 2)
            {
                // this is a traditional match-up
                if (preview)
                {
                    return View("PreGame", game);
                }
                else
                {
                    return View("Game", game);
                }
            }
            else
            {
                return View("GameMulti", game);
            }

            return View("Game", game);
        }
    }
}