using System.Collections.Generic;
using VSAND.Data.Entities;

namespace VSAND.Data.ViewModels.Users
{
    public class AppxUserRoleCategory
    {
        public string RoleCat { get; set; }
        public List<AppxUserRole> Roles { get; set; }
    }
}
