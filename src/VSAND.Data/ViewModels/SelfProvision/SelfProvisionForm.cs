using System.Collections.Generic;

namespace VSAND.Data.ViewModels.SelfProvision
{
    public class SelfProvisionForm
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string ConfirmPassword { get; set; } = "";
        public string Phone { get; set; } = "";
        public string OtherPhone { get; set; } = "";
        public List<int> Sports { get; set; } = new List<int>();
    }
}
