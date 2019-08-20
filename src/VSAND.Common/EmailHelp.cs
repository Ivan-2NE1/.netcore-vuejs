using System.Net.Mail;

namespace VSAND.Common
{
    public static class EmailHelp
    {
        public static string CleanEmailAddress(string emailAddress)
        {
            emailAddress = emailAddress?.Trim().ToLower() ?? "";
            if (string.IsNullOrWhiteSpace(emailAddress))
            {
                return null;
            }

            try
            {
                var address = new MailAddress(emailAddress);
                emailAddress = address.Address; 
            }
            catch
            {
                return null;
            }

            return emailAddress;
        }
    }
}
