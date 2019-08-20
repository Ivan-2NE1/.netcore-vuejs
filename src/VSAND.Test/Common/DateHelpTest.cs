using Microsoft.VisualStudio.TestTools.UnitTesting;
using VSAND.Common;

namespace VSAND.Test.Common
{
    [TestClass]
    public class DateHelpTest
    {
        [TestMethod]
        public void LastDayOfMonthReturnsCorrectValue()
        {
            int day = DateHelp.LastDayOfMonth(9);
            Assert.AreEqual(day, 30);

            day = DateHelp.LastDayOfMonth(2, 2016);
            Assert.AreEqual(day, 29);

            day = DateHelp.LastDayOfMonth(2, 2017);
            Assert.AreEqual(day, 28);
        }
    }
}
