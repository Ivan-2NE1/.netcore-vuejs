using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class AppxUserRoleMember
    {
        public int AdminRoleId { get; set; }
        public int AdminId { get; set; }
        public int RoleId { get; set; }

        public AppxUser Admin { get; set; }
        public AppxUserRole Role { get; set; }
    }
}
