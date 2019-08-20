using Microsoft.VisualStudio.TestTools.UnitTesting;
using VSAND.Data;
using VSAND.Data.Repositories;
using VSAND.Test.Data;
using VSAND.Data.Entities;
using VSAND.Services.Data.Manage.States;
using System.Threading.Tasks;

namespace VSAND.Test.Services.Data.Manage
{
    [TestClass]
    public class StateServiceSqlTests
    {
        [TestMethod]
        public async Task AddStateMethodInsertsStateValues()
        {
            //Arrange
            var options = DbUtil.GetOptions("AddStateMethodInsertsStateValuesWhenUnique");
            var context = DbUtil.GetContext(options);
            var uow = DbUtil.GetUow(context);

            const int newId = 2;
            const string newName = "Testonia";
            const string newAbbr = "TS";
            const string newPubAbbr = "Test.";

            await uow.States.Insert(new VsandState() { StateId = 1, Name = "New Jersey", Abbreviation = "NJ", PubAbbreviation = "NJ" });
            var bSetup = await uow.Save();

            var oSvc = new StateService(uow);

            var oNewState = new VsandState() { StateId = newId, Name = newName, Abbreviation = newAbbr, PubAbbreviation = newPubAbbr };

            // Act
            var oResult = await oSvc.AddStateAsync(oNewState);

            // Assert
            Assert.IsTrue(oResult.Success, $"Result should be true (Had error" + oResult.Message + ")");

            // Double check that the record was added with the desired values
            var oState = await uow.States.GetById(newId);
            Assert.IsNotNull(oState, $"Created state should not be null");
            Assert.AreEqual(oState.StateId, oNewState.StateId, $"StateId for return should equal " + newId);
            Assert.AreEqual(oState.Name, oNewState.Name, $"Name for return should equal " + newName);
            Assert.AreEqual(oState.Abbreviation, oNewState.Abbreviation, $"Abbreviation for return should equal " + newAbbr);
            Assert.AreEqual(oState.PubAbbreviation, oNewState.PubAbbreviation, $"PubAbbreviation for return should equal " + newPubAbbr);
        }

        [TestMethod]
        public async Task AddStateMethodInsertFailsWithDuplicateName()
        {
            var options = DbUtil.GetOptions("AddStateMethodInsertFailsWithDuplicateName");

            using (var context = new VsandContext(options))
            {
                var uow = new UnitOfWork(context);
                
                var sName = "New Jersey";
                await uow.States.Insert(new VsandState() { StateId = 1, Name = sName, Abbreviation = "NJ", PubAbbreviation = "NJ" });
                await uow.Save();

                var oNewState = new VsandState() { StateId = 2, Name = sName, Abbreviation = "PA", PubAbbreviation = "Penna." };

                var oSvc = new StateService(uow);
                var oResult = await oSvc.AddStateAsync(oNewState);
                Assert.IsFalse(oResult.Success);

                // verify the record didn't get added to the data set
                var oCheckState = await uow.States.GetById(2);
                Assert.IsNull(oCheckState);
            }
        }

        [TestMethod]
        public async Task UpdateStateMethodUpdatesStateValues()
        {
            var options = DbUtil.GetOptions("UpdateStateMethodUpdatesStateValues");

            int stateId = 1;

            using (var context = new VsandContext(options))
            {
                var uow = new UnitOfWork(context);
                var oSvc = new StateService(uow);

                var oState = new VsandState() { StateId = stateId, Name = "A Very Good State", Abbreviation = "AG", PubAbbreviation = "AG" };
                await oSvc.AddStateAsync(oState);
            }

            using (var context = new VsandContext(options))
            {
                var uow = new UnitOfWork(context);
                var oSvc = new StateService(uow);

                // this entity is retrieved to make sure that the EF change tracker doesn't throw an exception when pulling a single record before updating
                var oState = await oSvc.GetStateAsync(stateId);
                if (oState == null)
                {
                    Assert.Fail("Where record at?");
                }
                
                var updateState = new VsandState() { StateId = stateId, Name = "A Good State", Abbreviation = "AG", PubAbbreviation = "AG" };

                await oSvc.UpdateStateAsync(updateState);
                var checkState = await oSvc.GetStateAsync(stateId);

                Assert.AreNotEqual(oState.Name, checkState.Name);
                Assert.AreEqual(oState.Abbreviation, checkState.Abbreviation);
                Assert.AreEqual(oState.PubAbbreviation, checkState.PubAbbreviation);
            }
        }

        [TestMethod]
        public void UpdateStateMethodChangesStateValuesWhenUnique()
        {
            //TODO: Complete test setup for UpdateStateMethodChangesStateValuesWhenUnique
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void UpdateStateMethodFailsWhenNotUnique()
        {
            //TODO: Complete test setup for UpdateStateMethodFailsWhenNotUnique
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void DeleteStateMethodRemovesStateValues()
        {
            //TODO: Complete test setup for DeleteStateMethodRemovesStateValues
            Assert.IsTrue(true);
        }
    }
}
