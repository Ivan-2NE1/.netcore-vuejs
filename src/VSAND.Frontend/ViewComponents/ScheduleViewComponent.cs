using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VSAND.Services.Data.GameReports;

namespace VSAND.Frontend.ViewComponents
{
    public class ScheduleViewComponent : ViewComponent
    {
        private IGameReportService _gameService;

        public ScheduleViewComponent(IGameReportService gameService)
        {
            _gameService = gameService;
        }

        //public async Task<IViewComponentResult> InvokeAsync(int? sportId, int? schoolId, int? scheduleYearId)
        //{
        //    var items = await GetItemsAsync(maxPriority, isDone);
        //    return View(items);
        //}
        //private Task<List<TodoItem>> GetItemsAsync(int maxPriority, bool isDone)
        //{
        //    return db.ToDo.Where(x => x.IsDone == isDone && x.Priority <= maxPriority).ToListAsync();
        //}

    }
}
