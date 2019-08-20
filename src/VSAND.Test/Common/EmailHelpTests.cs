using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Mail;
using VSAND.Common;

namespace VSAND.Test.Common
{
    [TestClass]
    public class EmailHelpTests
    {
        [TestMethod]
        public void CleanEmailHandlesNullProperly()
        {
            string emailResult = EmailHelp.CleanEmailAddress(null);
            Assert.IsNull(emailResult);
        }

        [TestMethod]
        public void CleanEmailHandlesEmptyStringProperly()
        {
            string emailResult = EmailHelp.CleanEmailAddress("");
            Assert.IsNull(emailResult);
        }

        [TestMethod]
        public void CleanEmailHandlesStandardEmailAddressFormat()
        {
            string testEmail = "Test@applicationx.net";
            string emailResult = EmailHelp.CleanEmailAddress(testEmail);
            Assert.IsTrue(testEmail.Equals(emailResult, System.StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public void CleanEmailReturnsEmailPartOnlyOfFullAddress()
        {
            string testEmail = "Test@applicationx.net";
            MailAddress testAddress = new MailAddress(testEmail, "FirstName LastName");
            string addressTest = testAddress.ToString();
            string emailResult = EmailHelp.CleanEmailAddress(addressTest);            
            Assert.IsTrue(testEmail.Equals(emailResult, System.StringComparison.OrdinalIgnoreCase), $"Comparing {testEmail} against {addressTest}");
        }

        [TestMethod]
        public void CleanEmailReturnsNullForInvalidEmailAddressFormat()
        {
            string testEmail = "Testapplicationx.net";
            string emailResult = EmailHelp.CleanEmailAddress(testEmail);
            Assert.IsNull(emailResult);
        }
    }
}
