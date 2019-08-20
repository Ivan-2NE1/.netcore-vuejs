using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using VSAND.Data.ViewModels;
using VSAND.Services.Data.Manage.ScheduleYears;

namespace VSAND.Backend.Filters
{
    public class SetViewDataFilter : IAsyncActionFilter
    {
        readonly IScheduleYearService _scheduleYears;

        public SetViewDataFilter(IScheduleYearService scheduleYears)
        {
            _scheduleYears = scheduleYears;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Execute the rest of the MVC filter pipeline
            var resultContext = await next();

            if (resultContext.Result is ViewResult view)
            {
                var activeScheduleYear = await GetActiveScheduleYear();
                view.ViewData["ActiveScheduleYear"] = activeScheduleYear;
                if (activeScheduleYear != null)
                {
                    view.ViewData["ActiveScheduleYearId"] = activeScheduleYear.ScheduleYearId;
                }
                view.ViewData["HomeState"] = Environment.GetEnvironmentVariable("HomeState");

                view.ViewData["LogoUrl"] = Environment.GetEnvironmentVariable("LogoUrl");
                view.ViewData["SiteName"] = Environment.GetEnvironmentVariable("SiteName");
                view.ViewData["SiteUrl"] = Environment.GetEnvironmentVariable("SiteUrl");
            }
        }

        private async Task<ScheduleYear> GetActiveScheduleYear()
        {
            //TODO: This result needs to be stored in the cache, cache updated when active schedule year changes
            return await _scheduleYears.GetActiveScheduleYear();
        }
    }
}
