using System.Collections.Generic;

namespace VSAND.Data.ViewModels.Users
{
    public class UserEditViewModel
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobilePhone { get; set; }
        public string OtherPhone { get; set; }
        public bool IsAdmin { get; set; }
        public List<int> UserRoles { get; set; }
        public int? PublicationId { get; set; }
        public int? SchoolId { get; set; }
    }
}
