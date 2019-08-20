using System;
using System.Collections.Generic;
using VSAND.Data.ViewModels;
using VSAND.Data.ViewModels.GameReport;

namespace VSAND.Frontend.Models
{
    public class ScheduleModel
    {
        public DateTime ViewDate { get; }
        public DateTime PrevViewStartDate { get; }
        public DateTime NextViewStartDate { get; }
        public List<DateTime> ViewDates { get; } = new List<DateTime>();

        public List<GameReportSummary> Games { get; }

        public ScheduleModel()
        {
        }

        public ScheduleModel(DateTime startDate, List<GameReportSummary> games)
        {
            ViewDate = new DateTime(startDate.Year, startDate.Month, startDate.Day);
            for(var i = 3; i > 0; i--)
            {
                ViewDates.Add(ViewDate.AddDays(-i));
            }
            ViewDates.Add(ViewDate);
            for(var i = 1; i <= 3; i++)
            {
                ViewDates.Add(ViewDate.AddDays(i));
            }

            PrevViewStartDate = ViewDate.AddDays(-4);
            NextViewStartDate = ViewDate.AddDays(4);

            Games = games;
        }
    }
}
