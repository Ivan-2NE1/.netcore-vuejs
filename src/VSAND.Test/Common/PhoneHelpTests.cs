using Microsoft.VisualStudio.TestTools.UnitTesting;
using VSAND.Common;

namespace VSAND.Test.Common
{
    [TestClass]
    public class PhoneHelpTests
    {
        [TestMethod]
        public void CleanMobilePhoneNumberHandlesNullProperly()
        {
            string phoneResult = PhoneHelp.CleanMobilePhoneNumber(null);
            Assert.IsNull(phoneResult);
        }

        [TestMethod]
        public void CleanMobilePhoneNumberHandlesEmptyStringProperly()
        {
            string phoneResult = PhoneHelp.CleanMobilePhoneNumber("");
            Assert.IsNull(phoneResult);
        }

        [TestMethod]
        public void CleanMobilePhoneNumberHandlesStandardPhoneNumberFormat()
        {
            string phoneResult = PhoneHelp.CleanMobilePhoneNumber("(908) 652-4774");
            Assert.AreEqual(phoneResult, "9086524774");
        }

        [TestMethod]
        public void CleanMobilePhoneNumberRejectsPhoneNumberWithExtension()
        {
            string phoneResult = PhoneHelp.CleanMobilePhoneNumber("(908) 652-4774 ext. 123");
            Assert.IsNull(phoneResult);
        }
    }
}
