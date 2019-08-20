using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class AppxUserRole
    {
        public AppxUserRole()
        {
            AppxUserRoleMember = new HashSet<AppxUserRoleMember>();
        }

        public int RoleId { get; set; }
        public string RoleCat { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }

        public ICollection<AppxUserRoleMember> AppxUserRoleMember { get; set; }
    }
}
