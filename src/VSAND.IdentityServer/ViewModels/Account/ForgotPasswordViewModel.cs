using System.ComponentModel.DataAnnotations;

namespace VSAND.IdentityServer.ViewModels.Account
{
    public class ForgotPasswordViewModel
    {
        [Required]
        public string Username { get; set; }

        public string ReturnUrl { get; set; }
    }
}
