using Microsoft.AspNetCore.Identity;
using VSAND.Data.Entities;

namespace VSAND.Data.Identity
{
    public class ApplicationUser : IdentityUser<int>
    {
        public int AppxUserAdminId { get; set; }

        public AppxUser AppxUser { get; set; }
    }
}
