using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VSAND.Data.Entities;
using VSAND.Data.ViewModels;
using VSAND.Services.Data.Teams;
using VSAND.Services.Hubs;
using VSAND.Test.Data;

namespace VSAND.Test.Services.Data.Teams
{
    [TestClass]
    public class TeamServiceTests
    {
        #region "AutocompleteAsync Tests"
        [TestMethod]
        public async Task AutocompleteAsyncReturnsMatchedTeamsWithinCorrectSportAndScheduleYear()
        {
            // Arrange
            var options = DbUtil.GetOptions("AutocompleteAsyncReturnsMatchedTeamsWithinCorrectSportAndScheduleYear");
            var context = DbUtil.GetContext(options);
            var uow = DbUtil.GetUow(context);
            var cache = CacheUtil.GetInMemoryCache();

            var hub = HubUtil.GetHub<ProvisioningHub>();

            await uow.Teams.Insert(new VsandTeam() { TeamId = 1, Name = "A Test Team", SchoolId = 10, SportId = 10, ScheduleYearId = 10 });
            await uow.Teams.Insert(new VsandTeam() { TeamId = 2, Name = "A Test Team", SchoolId = 10, SportId = 11, ScheduleYearId = 10 });
            await uow.Teams.Insert(new VsandTeam() { TeamId = 3, Name = "A Test Team", SchoolId = 10, SportId = 10, ScheduleYearId = 11 });
            await uow.Save();

            var oTeamService = new TeamService(uow, hub, cache);

            // Act
            var oList = (List<ListItem<int>>) await oTeamService.AutocompleteAsync("A Test", 10, 10);

            // Assert
            Assert.IsNotNull(oList, "AutocompleteAsync should return empty list when no matches are found");
            Assert.IsTrue(oList.Count == 1, "AutocompleteAsync should return 1 matched result for this test");
            Assert.IsTrue(oList.First().id == 1, "GetTeamIdAsync should return id = 1");
        }
        #endregion

        #region "GetTeamId Tests"
        [TestMethod]
        public async Task GetTeamIdReturnsZeroForUnmatchedTeamCriteria()
        {
            // Arrange
            var options = DbUtil.GetOptions("GetTeamIdReturnsZeroForUnmatchedTeamCriteria");
            var context = DbUtil.GetContext(options);
            var uow = DbUtil.GetUow(context);
            var cache = CacheUtil.GetInMemoryCache();
            var hub = HubUtil.GetHub<ProvisioningHub>();

            await uow.Teams.Insert(new VsandTeam() { TeamId = 1, Name = "A Test Team", SchoolId = 10, SportId = 10, ScheduleYearId = 10 });
            await uow.Save();
            
            var oTeamService = new TeamService(uow, hub, cache);

            // Act
            var teamId = await oTeamService.GetTeamIdAsync(1, 1, 1);

            // Assert
            Assert.IsNotNull(teamId, "GetTeamIdAsync should return 0 when the team is not found");
            Assert.IsTrue(teamId == 0, "GetTeamIdAsync should not return a teamid when the team is not found");
        }

        [TestMethod]
        public async Task GetTeamIdReturnsCorrectTeamIdForMatchedCriteria()
        {
            // Arrange
            var options = DbUtil.GetOptions("GetTeamIdReturnsCorrectTeamIdForMatchedCriteria");
            var context = DbUtil.GetContext(options);
            var uow = DbUtil.GetUow(context);
            var cache = CacheUtil.GetInMemoryCache();
            var hub = HubUtil.GetHub<ProvisioningHub>();

            var selectTeamId = 2;
            var selectSchoolId = 10;
            var selectSportId = 10;
            var selectScheduleYearId = 1;

            await uow.Teams.Insert(new VsandTeam() { TeamId = 1, Name = "A Test Team", SchoolId = 10, SportId = 10, ScheduleYearId = 10 });
            await uow.Teams.Insert(new VsandTeam() { TeamId = selectTeamId, Name = "B Test Team", SchoolId = selectSchoolId, SportId = selectSportId, ScheduleYearId = selectScheduleYearId });
            await uow.Teams.Insert(new VsandTeam() { TeamId = 3, Name = "C Test Team", SchoolId = 10, SportId = 1, ScheduleYearId = 10 });
            await uow.Teams.Insert(new VsandTeam() { TeamId = 4, Name = "D Test Team", SchoolId = 1, SportId = 1, ScheduleYearId = 10 });
            await uow.Save();

            var oTeamService = new TeamService(uow, hub, cache);

            // Act
            var teamId = await oTeamService.GetTeamIdAsync(selectSchoolId, selectSportId, selectScheduleYearId);

            // Assert
            Assert.IsNotNull(teamId, "GetTeamIdAsync should match " + selectTeamId + " given the input criteria");
            Assert.IsTrue(teamId == selectTeamId, "GetTeamIdAsync should return " + selectTeamId + " for the input criteria");
        }

        #endregion

        #region "GetTeamAsync Tests"
        [TestMethod]
        public async Task GetTeamAsyncReturnsNullForUnmatchedTeamId()
        {
            // Arrange
            var options = DbUtil.GetOptions("GetTeamAsyncReturnsNullForUnmatchedTeamId");
            var context = DbUtil.GetContext(options);
            var uow = DbUtil.GetUow(context);
            var cache = CacheUtil.GetInMemoryCache();
            var hub = HubUtil.GetHub<ProvisioningHub>();

            var selectTeamId = 2;
            var selectSchoolId = 10;
            var selectSportId = 10;
            var selectScheduleYearId = 1;
            var testTeamId = 1;

            await uow.Teams.Insert(new VsandTeam() { TeamId = selectTeamId, Name = "B Test Team", SchoolId = selectSchoolId, SportId = selectSportId, ScheduleYearId = selectScheduleYearId });
            await uow.Save();

            var oTeamService = new TeamService(uow, hub, cache);

            // Act
            var oResult = await oTeamService.GetTeamAsync(testTeamId);

            // Assert
            Assert.IsNull(oResult, "GetTeamAsync should return null given unmatched teamId arg");
        }

        [TestMethod]
        public async Task GetTeamAsyncReturnsCorrectResultForMatchedTeamId()
        {
            // Arrange
            var options = DbUtil.GetOptions("GetTeamAsyncReturnsCorrectResultForMatchedTeamId");
            var context = DbUtil.GetContext(options);
            var uow = DbUtil.GetUow(context);
            var cache = CacheUtil.GetInMemoryCache();
            var hub = HubUtil.GetHub<ProvisioningHub>();

            await uow.Sports.Insert(new VsandSport() { SportId = 1, Name = "1 Test Sport", Enabled = true });
            await uow.Sports.Insert(new VsandSport() { SportId = 2, Name = "2 Test Sport", Enabled = true });
            await uow.ScheduleYears.Insert(new VsandScheduleYear() { ScheduleYearId = 1, Name = "1 Test SY" });
            await uow.ScheduleYears.Insert(new VsandScheduleYear() { ScheduleYearId = 2, Name = "2 Test SY" });
            await uow.Teams.Insert(new VsandTeam() { TeamId = 1, Name = "1 Test Team", SchoolId = 1, SportId = 1, ScheduleYearId = 1 });
            await uow.Save();

            var oTeamService = new TeamService(uow, hub, cache);

            // Act
            var oResult = await oTeamService.GetTeamAsync(1);

            // Assert
            Assert.IsNotNull(oResult, "GetTeamAsync should return team given valid teamid");
            Assert.IsTrue(oResult.TeamId == 1, "GetTeamAsync should return the team that matches the teamid supplied");
            Assert.IsNotNull(oResult.Sport, "GetTeamAsync should return the team object with the related sport loaded");
            Assert.IsTrue(oResult.Sport.SportId == 1, "GetTeamAsync should return the proper related sport record");
            Assert.IsNotNull(oResult.ScheduleYear, "GetTeamAsync should return the team object with the related schedule year loaded");
            Assert.IsTrue(oResult.ScheduleYear.ScheduleYearId == 1, "GetTeamAsync should return the proper related schedule year record");
        }

        #endregion
    }
}
