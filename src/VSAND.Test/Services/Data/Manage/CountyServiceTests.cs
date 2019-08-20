using Microsoft.VisualStudio.TestTools.UnitTesting;
using VSAND.Data;
using VSAND.Data.Repositories;
using VSAND.Test.Data;
using VSAND.Data.Entities;
using VSAND.Services.Data.Manage.Counties;
using System.Threading.Tasks;

namespace VSAND.Test.Services.Data.Manage
{
    [TestClass]
    public class CountyServiceSqlTests
    {
        [TestMethod]
        public async Task AddCountyMethodInsertsCountyValues()
        {
            //Arrange
            var options = DbUtil.GetOptions("AddCountyMethodInsertsCountyValuesWhenUnique");
            var context = DbUtil.GetContext(options);
            var uow = DbUtil.GetUow(context);

            const int newId = 2;
            const string newName = "Testonia";
            const string newAbbr = "TS";
            const string newState = "Test.";

            await uow.Counties.Insert(new VsandCounty() { CountyId = 1, Name = "New Jersey", CountyAbbr = "NJ", State = "NJ" });
            var bSetup = await uow.Save();

            var oSvc = new CountyService(uow);

            var oNewCounty = new VsandCounty() { CountyId = newId, Name = newName, CountyAbbr = newAbbr, State = newState };

            // Act
            var oResult = await oSvc.AddCountyAsync(oNewCounty);

            // Assert
            Assert.IsTrue(oResult.Success, $"Result should be true (Had error" + oResult.Message + ")");

            // Double check that the record was added with the desired values
            var oCounty = await uow.Counties.GetById(newId);
            Assert.IsNotNull(oCounty, $"Created county should not be null");
            Assert.AreEqual(oCounty.CountyId, oNewCounty.CountyId, $"CountyId for return should equal " + newId);
            Assert.AreEqual(oCounty.Name, oNewCounty.Name, $"Name for return should equal " + newName);
            Assert.AreEqual(oCounty.CountyAbbr, oNewCounty.CountyAbbr, $"Abbreviation for return should equal " + newAbbr);
            Assert.AreEqual(oCounty.State, oNewCounty.State, $"PubAbbreviation for return should equal " + newState);
        }

        [TestMethod]
        public async Task AddCountyMethodInsertFailsWithDuplicateName()
        {
            var options = DbUtil.GetOptions("AddCountyMethodInsertFailsWithDuplicateName");

            using (var context = new VsandContext(options))
            {
                var uow = new UnitOfWork(context);

                var sName = "New Jersey";
                await uow.Counties.Insert(new VsandCounty() { CountyId = 1, Name = sName, CountyAbbr = "NJ", State = "NJ" });
                await uow.Save();

                var oNewCounty = new VsandCounty() { CountyId = 2, Name = sName, CountyAbbr = "PA", State = "Penna." };

                var oSvc = new CountyService(uow);
                var oResult = await oSvc.AddCountyAsync(oNewCounty);
                Assert.IsFalse(oResult.Success);

                // verify the record didn't get added to the data set
                var oCheckCounty = await uow.Counties.GetById(2);
                Assert.IsNull(oCheckCounty);
            }
        }

        [TestMethod]
        public async Task UpdateCountyMethodUpdatesCountyValues()
        {
            var options = DbUtil.GetOptions("UpdateCountyMethodUpdatesCountyValues");

            int countyId = 1;

            using (var context = new VsandContext(options))
            {
                var uow = new UnitOfWork(context);
                var oSvc = new CountyService(uow);

                var oCounty = new VsandCounty() { CountyId = countyId, Name = "A Very Good County", CountyAbbr = "AG", State = "AG" };
                await oSvc.AddCountyAsync(oCounty);
            }

            using (var context = new VsandContext(options))
            {
                var uow = new UnitOfWork(context);
                var oSvc = new CountyService(uow);

                // this entity is retrieved to make sure that the EF change tracker doesn't throw an exception when pulling a single record before updating
                var oCounty = await oSvc.GetCountyAsync(countyId);
                if (oCounty == null)
                {
                    Assert.Fail("Where record at?");
                }

                var updateCounty = new VsandCounty() { CountyId = countyId, Name = "A Good County", CountyAbbr = "AG", State = "AG" };

                await oSvc.UpdateCountyAsync(updateCounty);
                var checkCounty = await oSvc.GetCountyAsync(countyId);

                Assert.AreNotEqual(oCounty.Name, checkCounty.Name);
                Assert.AreEqual(oCounty.CountyAbbr, checkCounty.CountyAbbr);
                Assert.AreEqual(oCounty.State, checkCounty.State);
            }
        }

        [TestMethod]
        public void UpdateCountyMethodChangesCountyValuesWhenUnique()
        {
            //TODO: Complete test setup for UpdateCountyMethodChangesCountyValuesWhenUnique
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void UpdateCountyMethodFailsWhenNotUnique()
        {
            //TODO: Complete test setup for UpdateCountyMethodFailsWhenNotUnique
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void DeleteCountyMethodRemovesCountyValues()
        {
            //TODO: Complete test setup for DeleteCountyMethodRemovesCountyValues
            Assert.IsTrue(true);
        }
    }
}
