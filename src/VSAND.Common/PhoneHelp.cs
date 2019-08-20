using System.Text.RegularExpressions;

namespace VSAND.Common
{
    public static class PhoneHelp
    {
        public static string CleanMobilePhoneNumber(string phoneNumber)
        {
            phoneNumber = phoneNumber?.Trim() ?? "";

            Regex phoneRegex = new Regex("[^0-9]");
            phoneNumber = phoneRegex.Replace(phoneNumber, "");

            if (phoneNumber.Length != 10)
            {
                return null;
            }

            return phoneNumber;
        }
    }
}
