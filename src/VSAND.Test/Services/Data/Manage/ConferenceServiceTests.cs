using Microsoft.VisualStudio.TestTools.UnitTesting;
using VSAND.Data;
using VSAND.Data.Repositories;
using VSAND.Test.Data;
using VSAND.Data.Entities;
using VSAND.Services.Data.Manage.Conferences;
using System.Threading.Tasks;

namespace VSAND.Test.Services.Data.Manage
{
    [TestClass]
    public class ConferenceServiceSqlTests
    {
        [TestMethod]
        public async Task AddConferenceMethodInsertsConferenceValues()
        {
            //Arrange
            var options = DbUtil.GetOptions("AddConferenceMethodInsertsConferenceValuesWhenUnique");
            var context = DbUtil.GetContext(options);
            var uow = DbUtil.GetUow(context);

            const int newId = 2;
            const string newName = "Testonia";

            await uow.Conferences.Insert(new VsandConference() { ConferenceId = 1, Name = "New Jersey" });
            var bSetup = await uow.Save();

            var oSvc = new ConferenceService(uow);

            var oNewConference = new VsandConference() { ConferenceId = newId, Name = newName };

            // Act
            var oResult = await oSvc.AddConferenceAsync(oNewConference);

            // Assert
            Assert.IsTrue(oResult.Success, $"Result should be true (Had error" + oResult.Message + ")");

            // Double check that the record was added with the desired values
            var oConference = await uow.Conferences.GetById(newId);
            Assert.IsNotNull(oConference, $"Created conference should not be null");
            Assert.AreEqual(oConference.ConferenceId, oNewConference.ConferenceId, $"ConferenceId for return should equal " + newId);
            Assert.AreEqual(oConference.Name, oNewConference.Name, $"Name for return should equal " + newName);
        }

        [TestMethod]
        public async Task AddConferenceMethodInsertFailsWithDuplicateName()
        {
            var options = DbUtil.GetOptions("AddConferenceMethodInsertFailsWithDuplicateName");

            using (var context = new VsandContext(options))
            {
                var uow = new UnitOfWork(context);

                var sName = "My Conference";
                await uow.Conferences.Insert(new VsandConference() { ConferenceId = 1, Name = sName });
                await uow.Save();

                var oNewConference = new VsandConference() { ConferenceId = 2, Name = sName };

                var oSvc = new ConferenceService(uow);
                var oResult = await oSvc.AddConferenceAsync(oNewConference);
                Assert.IsFalse(oResult.Success);

                // verify the record didn't get added to the data set
                var oCheckConference = await uow.Conferences.GetById(2);
                Assert.IsNull(oCheckConference);
            }
        }

        [TestMethod]
        public async Task UpdateConferenceMethodUpdatesConferenceValues()
        {
            var options = DbUtil.GetOptions("UpdateConferenceMethodUpdatesConferenceValues");

            int conferenceId = 1;

            using (var context = new VsandContext(options))
            {
                var uow = new UnitOfWork(context);
                var oSvc = new ConferenceService(uow);

                var oConference = new VsandConference() { ConferenceId = conferenceId, Name = "A Very Good Conference" };
                await oSvc.AddConferenceAsync(oConference);
            }

            using (var context = new VsandContext(options))
            {
                var uow = new UnitOfWork(context);
                var oSvc = new ConferenceService(uow);

                // this entity is retrieved to make sure that the EF change tracker doesn't throw an exception when pulling a single record before updating
                var oConference = await oSvc.GetConferenceAsync(conferenceId);
                if (oConference == null)
                {
                    Assert.Fail("Where record at?");
                }

                var updateConference = new VsandConference() { ConferenceId = conferenceId, Name = "A Good Conference" };

                await oSvc.UpdateConferenceAsync(updateConference);
                var checkConference = await oSvc.GetConferenceAsync(conferenceId);

                Assert.AreNotEqual(oConference.Name, checkConference.Name);
            }
        }

        [TestMethod]
        public void UpdateConferenceMethodChangesConferenceValuesWhenUnique()
        {
            //TODO: Complete test setup for UpdateConferenceMethodChangesConferenceValuesWhenUnique
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void UpdateConferenceMethodFailsWhenNotUnique()
        {
            //TODO: Complete test setup for UpdateConferenceMethodFailsWhenNotUnique
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void DeleteConferenceMethodRemovesConferenceValues()
        {
            //TODO: Complete test setup for DeleteConferenceMethodRemovesConferenceValues
            Assert.IsTrue(true);
        }
    }
}
