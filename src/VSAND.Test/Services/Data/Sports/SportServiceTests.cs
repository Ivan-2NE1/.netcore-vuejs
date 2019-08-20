using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSAND.Data.ViewModels;
using VSAND.Services.Data.CMS;
using VSAND.Services.Data.Sports;
using VSAND.Test.Data;

namespace VSAND.Test.Services.Data.Sports
{
    [TestClass]
    public class SportServiceTests
    {
        #region "GetList Tests"
        [TestMethod]
        public async Task GetListReturnsOnlyEnabledSports()
        {
            var options = DbUtil.GetOptions("GetListReturnsOnlyEnabledSports");
            var context = DbUtil.GetContext(options);
            var uow = DbUtil.GetUow(context);

            var cache = CacheUtil.GetInMemoryCache();
            var appxConfig = new ConfigService(uow, cache);

            // Arrange
            var enabledSportId = 2;
            await uow.Sports.Insert(new VSAND.Data.Entities.VsandSport() { SportId = 1, Name = "Disabled Sport", Enabled = false });
            await uow.Sports.Insert(new VSAND.Data.Entities.VsandSport() { SportId = enabledSportId, Name = "Enabled Sport", Enabled = true });
            await uow.Save();

            var oSportSvc = new SportService(uow, cache, appxConfig);

            // Act
            var oResult = await oSportSvc.GetActiveListAsync();

            // Assert
            Assert.IsTrue(oResult.Count() == 1, "Test created 2 sports, 1 disabled, 1 enabled; Result should only contain 1 record but it contains " + oResult.Count());
            Assert.IsTrue(oResult.First().id == enabledSportId, "Test created 2 sports, 1 disabled, 1 enabled; Result should only contain enabled record");
        }

        [TestMethod]
        public async Task GetListReturnsSortedByNameAscending()
        {
            var options = DbUtil.GetOptions("GetListReturnsSortedByNameAscending");
            var context = DbUtil.GetContext(options);
            var uow = DbUtil.GetUow(context);

            var cache = CacheUtil.GetInMemoryCache();
            var appxConfig = new ConfigService(uow, cache);

            // Arrange
            var firstAlphaSportId = 3;
            var lastAlphaSportId = 1;
            await uow.Sports.Insert(new VSAND.Data.Entities.VsandSport() { SportId = lastAlphaSportId, Name = "C Sport", Enabled = true });
            await uow.Sports.Insert(new VSAND.Data.Entities.VsandSport() { SportId = 2, Name = "B Sport", Enabled = true });
            await uow.Sports.Insert(new VSAND.Data.Entities.VsandSport() { SportId = firstAlphaSportId, Name = "A Sport", Enabled = true });
            await uow.Save();

            var oSportSvc = new SportService(uow, cache, appxConfig);

            // Act
            var oResult = await oSportSvc.GetActiveListAsync();

            // Assert
            Assert.IsTrue(oResult.Count() == 3, "Test created 3 enabled sports; Result should contain all 3 records but it contains " + oResult.Count());
            Assert.IsTrue(oResult.First().id == firstAlphaSportId, "Test expects that 'A Sport' should be first in the result list");
            Assert.IsTrue(oResult.Last().id == lastAlphaSportId, "Test expects that 'C Sport' should be last in the result list");
        }
        #endregion
    }
}
