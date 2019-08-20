using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class AppxMemberPermission
    {
        public int MemberPermissionId { get; set; }
        public int MemberId { get; set; }
        public string PermissionType { get; set; }
        public string PermissionName { get; set; }
        public string PermissionPath { get; set; }
    }
}
