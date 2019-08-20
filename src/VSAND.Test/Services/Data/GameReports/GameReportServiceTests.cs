using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VSAND.Data.ViewModels.GameReport;
using VSAND.Services.Data.GameReports;
using VSAND.Test.Data;

namespace VSAND.Test.Services.Data.GameReports
{
    [TestClass]
    public class GameReportServiceTests
    {
        #region "ReverseChronologicalList Tests"
        [TestMethod]
        public async Task ReverseChronologicalListReturnsResultsWithNoCriteria()
        {
            // Arrange
            var options = DbUtil.GetOptions("ReverseChronologicalListReturnsResultsWithNoCriteria");
            var context = DbUtil.GetContext(options);
            var uow = DbUtil.GetUow(context);
            var cache = CacheUtil.GetInMemoryCache();

            await uow.GameReports.Insert(new VSAND.Data.Entities.VsandGameReport() {
                GameReportId = 1,
                Name = "A Test Game Report",
                GameDate = DateTime.Now,
                SportId = 1,
                ScheduleYearId = 1,
                GameTypeId = 1,
                ReportedByName = "Test User",
                ReportedDate = DateTime.Now,
                Source = "Unit Test"
            });
            await uow.Sports.Insert(new VSAND.Data.Entities.VsandSport() { SportId = 1, Name = "Test Sport", Enabled = true });
            await uow.Save();

            var oGameService = new GameReportService(uow, cache);

            var oSearch = new SearchRequest();
            // Act
            var oResult = await oGameService.ReverseChronologicalList(oSearch);

            // Assert
            Assert.IsNotNull(oResult, "ReverseChronologicalList should return a non-null result for all requests");
            Assert.IsTrue(oResult.TotalResults == 1, "ReverseChronologicalList should return 1 for TotalResults given 1 game report");
        }
        #endregion
    }
}
