using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class AspnetUsers
    {
        public AspnetUsers()
        {
            AspnetPersonalizationPerUser = new HashSet<AspnetPersonalizationPerUser>();
            AspnetUsersInRoles = new HashSet<AspnetUsersInRoles>();
        }

        public Guid ApplicationId { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string LoweredUserName { get; set; }
        public string MobileAlias { get; set; }
        public bool IsAnonymous { get; set; }
        public DateTime LastActivityDate { get; set; }

        public AspnetApplications Application { get; set; }
        public AspnetMembership AspnetMembership { get; set; }
        public AspnetProfile AspnetProfile { get; set; }
        public ICollection<AspnetPersonalizationPerUser> AspnetPersonalizationPerUser { get; set; }
        public ICollection<AspnetUsersInRoles> AspnetUsersInRoles { get; set; }
    }
}
