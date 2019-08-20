using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VSAND.Data.ViewModels;
using VSAND.Services.Data.CMS;
using VSAND.Services.Data.Manage.ScheduleYears;
using VSAND.Services.Data.Sports;

namespace VSAND.FrontEnd.Filters
{
    public class SetViewDataFilter : IAsyncActionFilter
    {
        readonly IScheduleYearService _scheduleYears;
        readonly ISportService _sports;
        readonly IConfigService _appxConfig;

        public SetViewDataFilter(IScheduleYearService scheduleYears, 
            ISportService sports,
            IConfigService appxConfig)
        {
            _scheduleYears = scheduleYears;
            _sports = sports;
            _appxConfig = appxConfig;
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

                var activeSeason = await _appxConfig.GetConfigValueCachedAsync<string>("FrontEnd", "ActiveSeason");
                if (string.IsNullOrWhiteSpace(activeSeason))
                {
                    activeSeason = VSAND.Common.DateHelp.SeasonName();
                }
                view.ViewData["ActiveSeason"] = activeSeason;

                // Not used per Chris F.
                //var sortedSeasons = _sports.GetSeasons(activeSeason);
                //view.ViewData["SortedSeasons"] = sortedSeasons;

                // Not used per Chris F.
                //var featuredSports = await _sports.GetFeaturedSportItemsCachedAsync();
                //view.ViewData["FeaturedSportsNavItems"] = featuredSports;

                // Replace with simplified menu display for sports - Alpha By gender
                //var sportsBySeasons = await _sports.GetSportNavBySeasonCachedAsync();
                //view.ViewData["SportsBySeason"] = sportsBySeasons;
                var sportsByGender = await _sports.GetSportNavByGenderCachedAsync();
                view.ViewData["SportsByGender"] = sportsByGender;

                view.ViewData["SiteTitle"] = Environment.GetEnvironmentVariable("SiteTitle");
                view.ViewData["PubUrl"] = Environment.GetEnvironmentVariable("PubUrl");
                view.ViewData["HomeState"] = Environment.GetEnvironmentVariable("HomeState");
                view.ViewData["PubLogo"] = Environment.GetEnvironmentVariable("PubLogo");
                view.ViewData["PubHomeTitle"] = Environment.GetEnvironmentVariable("PubHomeTitle");
                view.ViewData["AppLogoDesktop"] = Environment.GetEnvironmentVariable("AppLogoDesktop");
                view.ViewData["AppLogoMobile"] = Environment.GetEnvironmentVariable("AppLogoMobile");
            }
        }

        async Task<ScheduleYear> GetActiveScheduleYear()
        {
            //TODO: This result needs to be stored in the cache, cache updated when active schedule year changes
            return await _scheduleYears.GetActiveScheduleYearCachedAsync();
        }
    }
}
