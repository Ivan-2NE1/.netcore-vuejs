using Microsoft.AspNetCore.Mvc;
using System;

namespace VSAND.Backend.Areas.Worksheet.Controllers
{
    [Area("Worksheet")]
    public class RoundupsController : Controller
    {
        public IActionResult Index()
        {
            var oRequestDate = DateTime.Now;
            //TODO: Need to implement query methods for Schedule controller
            //var oModel = await _gameReportService.ReverseChronologicalList(new Data.ViewModels.GameReport.SearchRequest()
            //{
            //    GameDate = oRequestDate,
            //    PageSize = 1000,
            //    PageNumber = 1
            //});
            ViewData["RequestDate"] = oRequestDate;
            return View();
        }

        [Route("Worksheet/Roundups/{requestedYear}/{requestedMonth}/{requestedDay}")]
        public IActionResult Index([FromRoute] int requestedYear, [FromRoute] int requestedMonth, [FromRoute] int requestedDay)
        {
            var oRequestDate = new DateTime(requestedYear, requestedMonth, requestedDay);
            //var oModel = await _gameReportService.ReverseChronologicalList(new Data.ViewModels.GameReport.SearchRequest()
            //{
            //    GameDate = oRequestDate,
            //    PageSize = 1000,
            //    PageNumber = 1
            //});
            ViewData["RequestDate"] = oRequestDate;
            return View();
        }
    }
}