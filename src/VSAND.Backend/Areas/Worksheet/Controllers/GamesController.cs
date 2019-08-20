using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VSAND.Services.Data.GameReports;

namespace VSAND.Backend.Areas.Worksheet.Controllers
{
    [Area("Worksheet")]
    public class GamesController : Controller
    {
        private readonly IGameReportService _gameReportService;
        public GamesController(IGameReportService gameReportService)
        {
            _gameReportService = gameReportService;
        }

        public async Task<IActionResult> Index()
        {
            var oRequestDate = DateTime.Now;
            var oModel = await _gameReportService.ReverseChronologicalList(new Data.ViewModels.GameReport.SearchRequest()
            {
                GameDate = oRequestDate,
                PageSize = 1000,
                PageNumber = 1
            });
            ViewData["RequestDate"] = oRequestDate;
            return View(oModel);
        }

        [Route("Worksheet/Games/{requestedYear}/{requestedMonth}/{requestedDay}")]
        public async Task<IActionResult> Index([FromRoute] int requestedYear, [FromRoute] int requestedMonth, [FromRoute] int requestedDay)
        {
            var oRequestDate = new DateTime(requestedYear, requestedMonth, requestedDay);
            var oModel = await _gameReportService.ReverseChronologicalList(new Data.ViewModels.GameReport.SearchRequest()
            {
                GameDate = oRequestDate,
                PageSize = 1000,
                PageNumber = 1
            });
            ViewData["RequestDate"] = oRequestDate;
            return View(oModel);
        }
    }
}