using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class AppxMemberUser
    {
        public int MemberId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ZipCode { get; set; }
        public string EmailAddress { get; set; }
        public string UserAlias { get; set; }
        public string Password { get; set; }
        public string PasswordReminder { get; set; }
        public string RegistrationKey { get; set; }
        public string ConfirmationKey { get; set; }
        public DateTime? ConfirmationDate { get; set; }
        public string ConfirmationIp { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public bool? Suspended { get; set; }
    }
}
