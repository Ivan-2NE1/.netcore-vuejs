using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VSAND.Data.ViewModels.GameReport;
using VSAND.Services.Data.GameReports;

namespace VSAND.Backend.Controllers
{
    public class GamesController : Controller
    {
        IGameReportService _gameService = null;

        public GamesController(IGameReportService gameService)
        {
            _gameService = gameService;
        }

        public async Task<IActionResult> Index([FromQuery(Name = "q")] string query)
        {
            var oSearch = Newtonsoft.Json.JsonConvert.DeserializeObject<SearchRequest>(query);

            var oGames = await _gameService.ReverseChronologicalList(oSearch);
            ViewData["SearchRequest"] = oSearch;
            return View(oGames);
        }
    }
}