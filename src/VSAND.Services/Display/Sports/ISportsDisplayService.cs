using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VSAND.Data.ViewModels.StatAggregation;

namespace VSAND.Services.Display.Sports
{
    public interface ISportsDisplayService
    {
        Task<SportStatsHomeView> GetSportStatsHomeView(int sportId);
    }
}
